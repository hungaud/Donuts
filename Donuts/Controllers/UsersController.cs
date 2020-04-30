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
    public class UsersController : ControllerBase
    {
        IUserRepository _userService;

        public UsersController(IUserRepository userService)
        {
            _userService = userService;
        }


        [HttpGet]
        public async Task<IActionResult> GetUser()
        {
            var allUser = _userService.GetAllUsers();
            return Ok(allUser);
        }

        [HttpGet("{id}")]
        [Produces(typeof(DbSet<User>))]
        public async Task<IActionResult> GetUser([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var user = await _userService.GetUser(id);

            if (user == null)
            {
                return NotFound();
            }

            return Ok(user);
        }


    }
}