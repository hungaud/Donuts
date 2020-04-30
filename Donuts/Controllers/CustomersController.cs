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
    [Route("api/users")]
    public class CustomersController : ControllerBase
    {
        IUserRepository _userRepository;

        public CustomersController(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }


        [HttpGet]
        public async Task<IActionResult> GetUser()
            {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var user = _userRepository.GetAllCustomers();

            if (user == null)
            {
                return NotFound();
            }

            return Ok(user);
        }

        [HttpGet("{id}")]
        [Produces(typeof(DbSet<Customer>))]
        public async Task<IActionResult> GetUser([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var user = await _userRepository.GetCustomer(id);

            if (user == null)
            {
                return NotFound();
            }

            return Ok(user);
        }


    }
}