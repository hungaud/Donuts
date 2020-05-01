﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Donuts.Models;
using Donuts.Models.Enums;
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
        ICustomerRepository _customerRepository;

        public DomainsController (IDomainRepository domainRepository, ICustomerRepository customerRepository)
        {
            _domainRepository = domainRepository;
            _customerRepository = customerRepository;
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

        // GET: api/domain/fromcustomer/5
        [HttpGet("fromcustomer/{id}")]
        [Produces(typeof(DbSet<Domain>))]
        public IActionResult GetAllDomainFromCustomer([FromRoute] int id)
        {
            return new ObjectResult(_domainRepository.GetAllDomainFromCustomer(id));
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

            if (domain.Customer == null || domain.CustomerId == null)
            {
                return BadRequest("Domain is not associated with a valid customer");
            }

            var customer = _customerRepository.GetCustomer(domain.CustomerId.GetValueOrDefault());
            if (customer == null)
            {
                return BadRequest("Customer is not valid for domain");
            }

            try
            {
                await _domainRepository.UpdateDomain(domain);
                return Ok(domain);
            }
            catch (DbUpdateConcurrencyException)
            {
                return BadRequest();
            }
        }

        // POST: api/routines
        [HttpPost("{timeduration}/{length}")]
        [Produces(typeof(DbSet<Domain>))]
        public async Task<IActionResult> PostDomain([FromBody] Domain domain, [FromRoute] TimeDuration timeduration, [FromRoute] int length)
        {

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            if(domain.Customer == null || domain.CustomerId == null)
            {
                return BadRequest("Domain is not associated with a valid user");
            }

            var customer = _customerRepository.GetCustomer(domain.CustomerId.Value).Result;
            if (customer == null)
            {
                return BadRequest("customer is not valid for domain");
            }

            await _domainRepository.AddDomain(domain);

            return CreatedAtAction("GetDomain", new { id = domain.DomainId }, domain);
        }

        [HttpDelete("{id}")]
        [Produces(typeof(Domain))]
        public async Task<IActionResult> DeleteDomain([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var domain = _domainRepository.GetDomain(id);
            if (domain == null)
            {
                return NotFound();
            }

            await _domainRepository.DeleteDomain(id);

            return Ok();
        }
    }
}