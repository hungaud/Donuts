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

        public CustomersController(ICustomerRepository customerRepository)
        {
            _customerRepository = customerRepository;
        }


        [HttpGet]
        public async Task<IActionResult> GetCustomer()
            {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var customer = _customerRepository.GetAllCustomers();

            if (customer == null)
            {
                return NotFound();
            }

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


    }
}