using Donuts.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Donuts.Repositories
{

    public interface IDomainRepository
    {
        Task<Domain> GetDomain(int id);
        IEnumerable<Domain> GetAllDomainFromCustomer(int id);
        Task<Domain> UpdateDomain(Domain domain);
        Task<Domain> DeleteDomain(int id);
        Task<Domain> AddDomain(Domain domain);
        Task<Domain> GetDomain(string name);
        IEnumerable<Domain> GetAllDomains();
    }


    public class DomainRepository : IDomainRepository
    {
        private DonutsContext _context;

        public DomainRepository (DonutsContext donutsContext)
        {
            _context = donutsContext;
        }

        public async Task<Domain> AddDomain(Domain domain)
        {
            await _context.Domain.AddAsync(domain);
            await _context.SaveChangesAsync();
            return domain;
        }

        public async Task<Domain> DeleteDomain(int id)
        {
            var domain = _context.Domain.Single(a => a.DomainId == id);
            _context.Domain.Remove(domain);
            await _context.SaveChangesAsync();
            return domain;
        }

        public IEnumerable<Domain> GetAllDomainFromCustomer(int id)
        {
            return _context.Domain.Where(d => d.DomainId == id);
        }

        public async Task<Domain> GetDomain(int id)
        {
            return await _context.Domain.Where(d => d.DomainId == id).FirstOrDefaultAsync();

        }

        public async Task<Domain> GetDomain(string name)
        {
            return await _context.Domain.Where(d => d.Name == name).FirstOrDefaultAsync();
        }

        public async Task<Domain> UpdateDomain(Domain domain)
        {
            _context.Domain.Update(domain);
            await _context.SaveChangesAsync();
            return domain;
        }

        public IEnumerable<Domain> GetAllDomains()
        {
            return _context.Domain;
        }
    }
}
