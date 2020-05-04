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
    [Route("api/verificationproviders")]
    [Produces("application/json")]
    public class VerificationProvidersController : ControllerBase
    {
        IVerificationProviderRepository _verificationProviderRepository;

        public VerificationProvidersController(IVerificationProviderRepository verificationProviderRepository)
        {
            _verificationProviderRepository = verificationProviderRepository;
        }

        // GET: api/verificationProviders/
        [HttpGet("{username:alpha}")]
        [Produces(typeof(DbSet<VerificationProvider>))]
        public IActionResult GetAllProviderByName([FromRoute] string name)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var providers = _verificationProviderRepository.GetAllByName(name);

            if (providers == null)
            {
                return NotFound();
            }
            return Ok(providers);
        }

        // GET: api/verificationProviders/
        [HttpGet]
        [Produces(typeof(DbSet<VerificationProvider>))]
        public IActionResult GetAllProvider()
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var providers = _verificationProviderRepository.GetAllProviders();

            if (providers == null)
            {
                return NotFound();
            }
            return Ok(providers);
        }

        [HttpPost]
        [Produces(typeof(DbSet<VerificationProvider>))]
        public async Task<IActionResult> PostPayment([FromBody] VerificationProvider provider)
        {

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            await _verificationProviderRepository.AddVerificationProvider(provider);

            return CreatedAtAction("GetPayment", new { id = provider.VerificationProviderId }, provider);
        }

    }
}