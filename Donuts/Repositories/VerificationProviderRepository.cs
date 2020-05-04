using Donuts.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Donuts.Repositories
{
    public interface IVerificationProviderRepository
    {
        IEnumerable<VerificationProvider> GetAllProviders();
        IEnumerable<VerificationProvider> GetAllByName(string name);
        Task<VerificationProvider> UpdateVerificationProvider(VerificationProvider verificationProvider);
        Task<VerificationProvider> DeleteVerificationProvider(int id);
        Task<VerificationProvider> AddVerificationProvider(VerificationProvider verificationProvider);
    }

    public class VerificationProviderRepository : IVerificationProviderRepository
    {
        private DonutsContext _context;

        public VerificationProviderRepository(DonutsContext donutsContext)
        {
            _context = donutsContext;
        }

        public async Task<VerificationProvider> AddVerificationProvider(VerificationProvider verificationProvider)
        {
            await _context.VerificationProvider.AddAsync(verificationProvider);
            await _context.SaveChangesAsync();
            return verificationProvider;
        }

        public async Task<VerificationProvider> DeleteVerificationProvider(int id)
        {
            var verificationProvider = _context.VerificationProvider.Single(a => a.VerificationProviderId == id);
            _context.VerificationProvider.Remove(verificationProvider);
            await _context.SaveChangesAsync();
            return verificationProvider;
        }

        public IEnumerable<VerificationProvider> GetAllProviders()
        {
            return _context.VerificationProvider;
        }

        public IEnumerable<VerificationProvider> GetAllByName(string name)
        {
            return _context.VerificationProvider.Where(vp => vp.ProvidersName == name);
        }

        public async Task<VerificationProvider> UpdateVerificationProvider(VerificationProvider verificationProvider)
        {
            _context.Update(verificationProvider);
            await _context.SaveChangesAsync();
            return verificationProvider;
        }
    }
}
