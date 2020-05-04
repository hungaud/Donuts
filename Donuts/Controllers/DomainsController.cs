﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text.RegularExpressions;
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
        IVerificationProviderRepository _verificationProviderRepository;

        public DomainsController (IDomainRepository domainRepository, ICustomerRepository customerRepository, 
            IVerificationProviderRepository verificationProviderRepository)
        {
            _domainRepository = domainRepository;
            _customerRepository = customerRepository;
            _verificationProviderRepository = verificationProviderRepository;
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

        // GET: api/Domain/
        [HttpGet("{username:alpha}")]
        [Produces(typeof(DbSet<Domain>))]
        public async Task<IActionResult> GetDomain([FromRoute] string name)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var domain = await _domainRepository.GetDomain(name);

            if (domain == null)
            {
                return NotFound();
            }

            return Ok(domain);
        }

        // GET: api/domain/fromcustomer/5
        [HttpGet("fromcustomer/{id}")]
        [Produces(typeof(DbSet<Domain>))]
        public IActionResult GetAllDomainFromCustomer([FromRoute] int id)
        {
            return Ok(_domainRepository.GetAllDomainFromCustomer(id));
        }

        [HttpGet]
        [Produces(typeof(DbSet<Domain>))]
        public IActionResult GetAllDomain()
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

        [HttpPut("{username:alpha/{timeduration}/{length}}")]
        [HttpPost("{timeduration}/{length}")]
        [Produces(typeof(Domain))]
        public async Task<IActionResult> PutDomain([FromRoute] string name, [FromRoute] TimeDuration timeduration, 
            [FromRoute] int length, [FromBody] Domain domain)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var originalDomain = _domainRepository.GetDomain(name).Result;
            if (originalDomain == null || originalDomain.DomainId != domain.DomainId)
            {
                return BadRequest();
            }

            if (domain.Customer == null || domain.CustomerId == null)
            {
                return BadRequest("Domain is not associated with a valid customer");
            }

            var customer = await _customerRepository.GetCustomer(domain.CustomerId.GetValueOrDefault());
            if (customer == null)
            {
                return BadRequest("Customer is not valid for domain");
            }

            try
            {
                var originalExpiration = originalDomain.ExpiriationDate;
                domain.ExpiriationDate = timeduration == TimeDuration.YEAR ? originalExpiration.AddYears(length) : originalExpiration.AddMonths(length);
                await _domainRepository.UpdateDomain(domain);
                return Ok(domain);
            }
            catch (DbUpdateConcurrencyException)
            {
                return BadRequest();
            }
        }

        // POST: api/domain
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

            // verify Customer's Contact-ID
            // This is where it would verify public key as well, but it is not implemented.
            if(!string.IsNullOrEmpty(customer.ProviderName))
            {
                var providers = _verificationProviderRepository.GetAllByName(customer.ProviderName);
                var verified = providers.Where(p => p.Format.IsMatch(customer.ContactId));
                if(verified.Count() == 0)
                {
                    return BadRequest("No valid verification from Customer");
                }
            } else
            {
                var providers = _verificationProviderRepository.GetAllProviders();
                var verified = providers.Where(p => p.Format.IsMatch(customer.ContactId));
                if (verified.Count() == 0)
                {
                    return BadRequest("No valid verification from Customer");
                }
            }

            var today = DateTime.Today;
            domain.ExpiriationDate = timeduration == TimeDuration.YEAR ? today.AddYears(length) : today.AddMonths(length);

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
            var domain = await _domainRepository.GetDomain(id);
            if (domain == null)
            {
                return NotFound();
            }

            await _domainRepository.DeleteDomain(id);

            return Ok();
        }
    }
}