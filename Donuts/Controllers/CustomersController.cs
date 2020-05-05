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
    [Produces("application/json")]
    [Route("api/customers")]
    public class CustomersController : ControllerBase
    {
        ICustomerRepository _customerRepository;
        IVerificationProviderRepository _verificationProviderRepository;

        public CustomersController(ICustomerRepository customerRepository,
            IVerificationProviderRepository verificationProviderRepository)
        {
            _customerRepository = customerRepository;
            _verificationProviderRepository = verificationProviderRepository;
        }


        [HttpGet]
        public IActionResult GetCustomer()
            {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var customer = _customerRepository.GetAllCustomers();

            return Ok(customer);
        }

        [HttpGet("{id}")]
        [Produces(typeof(DbSet<Customer>))]
        public async Task<IActionResult> GetCustomer([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var customer = await _customerRepository.GetCustomer(id);

            if (customer == null)
            {
                return NotFound();
            }

            return Ok(customer);
        }

        [HttpPost]
        [Produces(typeof(DbSet<Customer>))]
        public async Task<IActionResult> PostCustomer([FromBody] Customer customer)
        {

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var verified = GetProviders(customer);
            if (verified.Count() == 0)
            {
                return BadRequest("No valid verification from Customer");
            }

            customer.ProviderName = verified.FirstOrDefault().ProvidersName;

            await _customerRepository.AddCustomer(customer);

            return CreatedAtAction("GetCustomer", new { id = customer.CustomerId }, customer);
        }

        [HttpPut("{id}")]
        [Produces(typeof(DbSet<Customer>))]
        public async Task<IActionResult> PutCustomer([FromRoute] int id, [FromBody] Customer customer)
        {

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            if(id != customer.CustomerId)
            {
                return BadRequest();
            }

            var verified = GetProviders(customer);
            if(verified.Count() == 0)
            {
                return BadRequest("No valid verification from Customer");
            }

            try
            {
                if(string.IsNullOrEmpty(customer.ProviderName))
                {
                    customer.ProviderName = verified.FirstOrDefault().ProvidersName;
                }
                await _customerRepository.AddCustomer(customer);
                return Ok(customer);
            }
            catch (DbUpdateConcurrencyException)
            {
                return BadRequest();
            }
        }

        // verify Customer's Contact-ID
        // This is where it would verify public key as well, but it is not implemented.
        private IEnumerable<VerificationProvider> GetProviders(Customer customer)
        {
            IEnumerable<VerificationProvider> verified;
            if (!string.IsNullOrEmpty(customer.ProviderName))
            {
                var providers = _verificationProviderRepository.GetAllByName(customer.ProviderName);
                verified = providers.Where(p => p.Format.IsMatch(customer.ContactId));
            }
            else
            {
                var providers = _verificationProviderRepository.GetAllProviders();
                verified = providers.Where(p => p.Format.IsMatch(customer.ContactId));
            }
            return verified;
        }
    }
}