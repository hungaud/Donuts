using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Donuts.Models;
using Donuts.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Donuts.Controllers
{
    [Route("api/payments")]
    [Produces("application/json")]
    public class PaymentsController : ControllerBase
    {
        IPaymentRepository _paymentRepository;
        ICustomerRepository _customerRepository;

        public PaymentsController(IPaymentRepository paymentRepository, CustomerRepository customerRepository)
        {
            _paymentRepository = paymentRepository;
            _customerRepository = customerRepository;
        }

        [HttpGet("{id}")]
        [Produces(typeof(DbSet<Payment>))]
        public async Task<IActionResult> GetPayment([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var payment = await _paymentRepository.GetPayment(id);

            if (payment == null)
            {
                return NotFound();
            }

            return Ok(payment);
        }

        // GET: api/payment/fromcustomer/5
        [HttpGet("fromcustomer/{id}")]
        [Produces(typeof(DbSet<Payment>))]
        public async Task<IActionResult> GetPaymentFromCustomer([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var payment = await _paymentRepository.GetPaymentFromCustomer(id);

            if (payment == null)
            {
                return NotFound();
            }

            return Ok(payment);
        }

        [HttpPut("{id}")]
        [Produces(typeof(Payment))]
        public async Task<IActionResult> PutPayment([FromRoute] int id, [FromBody] Payment payment)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            if (id != payment.PaymentId)
            {
                return BadRequest();
            }
            if (payment.Customer == null || payment.CustomerId == null || payment.Domain == null || payment.DomainId == null)
            {
                return BadRequest("payment is not associated with a valid customer");
            }
            var dueDate = GetLastDayOfMonth();
            if (payment.DueDate != dueDate)
            {
                return BadRequest("Due Date is not at the last day of the Month");
            }

            var customer = _customerRepository.GetCustomer(payment.CustomerId);
            if (customer == null)
            {
                return BadRequest("Customer is not valid for domain");
            }

            try
            {
                await _paymentRepository.UpdatePayment(payment);
                return Ok(payment);
            }
            catch (DbUpdateConcurrencyException)
            {
                return BadRequest();
            }
        }

        // POST: api/routines
        [HttpPost]
        [Produces(typeof(DbSet<Payment>))]
        public async Task<IActionResult> PostDomain([FromBody] Payment payment)
        {

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            if (payment.Customer == null || payment.CustomerId == null)
            {
                return BadRequest("payment is not associated with a valid user");
            }
            var dueDate = GetLastDayOfMonth();
            if(payment.DueDate != dueDate)
            {
                return BadRequest("Due Date is not at the last day of the Month");
            }

            var customer = _customerRepository.GetCustomer(payment.CustomerId).Result;
            if (customer == null)
            {
                return BadRequest("customer is not valid for payment");
            }

            await _paymentRepository.AddPayment(payment);

            return CreatedAtAction("GetPayment", new { id = payment.PaymentId}, payment);
        }

        private DateTime GetLastDayOfMonth()
        {
            DateTime date = DateTime.Today;
            var firstDayOfMonth = new DateTime(date.Year, date.Month, 1);
            return firstDayOfMonth.AddMonths(1).AddDays(-1);
        }
    }
}