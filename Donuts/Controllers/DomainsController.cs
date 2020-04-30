using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Donuts.Models;
using Donuts.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Donuts.Controllers
{
    [Route("api/domains")]
    [Produces("application/json")]
    public class DomainsController : ControllerBase
    {

        IDomainRepository _domainRepository;
        IUserRepository _userRepository;

        public DomainsController (IDomainRepository domainRepository, IUserRepository userRepository)
        {
            _domainRepository = domainRepository;
            _userRepository = userRepository;
        }

        [HttpGet("{id}")]
        [Produces(typeof(DbSet<Domain>))]
        public async Task<IActionResult> GetDomain([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var domain = await _domainRepository.GetDomain(id);

            if (domain == null)
            {
                return NotFound();
            }

            return Ok(domain);
        }

        // GET: api/Patients/
        [HttpGet("{username:alpha}")]
        [Produces(typeof(DbSet<Domain>))]
        public async Task<IActionResult> GetDomain([FromRoute] string name)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var domain = _domainRepository.GetDomain(name);

            if (domain == null)
            {
                return NotFound();
            }

            return new ObjectResult(domain);
        }

        // GET: api/domain/fromuser/5
        [HttpGet("fromuser/{id}")]
        [Produces(typeof(DbSet<Domain>))]
        public IActionResult GetAllDomainFromUser([FromRoute] int id)
        {
            return new ObjectResult(_domainRepository.GetAllDomainFromUser(id));
        }

        [HttpGet]
        [Produces(typeof(DbSet<Domain>))]
        public async Task<IActionResult> GetAllDomain()
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var domain = _domainRepository.GetAllDomains();

            if (domain == null)
            {
                return NotFound();
            }

            return Ok(domain);
        }

        [HttpPut("{id}")]
        [Produces(typeof(Domain))]
        public async Task<IActionResult> PutDomain([FromRoute] int id, [FromBody] Domain domain)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != domain.DomainId)
            {
                return BadRequest();
            }

            if (domain.User == null || domain.UserId == null)
            {
                return BadRequest("Domain is not associated with a valid user");
            }

            var user = _userRepository.GetCustomer(domain.UserId.GetValueOrDefault());
            if (user == null)
            {
                return BadRequest("User is not valid for domain");
            }

            try
            {
                await _domainRepository.UpdateDomain(domain);
                return Ok(id);
            }
            catch (DbUpdateConcurrencyException)
            {
                return BadRequest();
            }
        }

        // POST: api/routines
        [HttpPost]
        [Produces(typeof(DbSet<Domain>))]
        public async Task<IActionResult> PostDomain([FromBody] Domain domain)
        {

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            if(domain.User == null || domain.UserId == null)
            {
                return BadRequest("Domain is not associated with a valid user");
            }

            var user = _userRepository.GetCustomer(domain.UserId.Value).Result;
            if (user == null)
            {
                return BadRequest("User is not valid for domain");
            }


            await _domainRepository.AddDomain(domain);

            return CreatedAtAction("GetDomain", new { id = domain.DomainId }, domain);
        }
    }
}