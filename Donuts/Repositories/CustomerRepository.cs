using Donuts.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Donuts.Repositories
{

    public interface IUserRepository
    {
        Task<Customer> GetCustomer(int id);
        IEnumerable<Customer> GetAllCustomers();
    }

    public class CustomerRepository : IUserRepository
    {

        private DonutsContext _context;

        public CustomerRepository(DonutsContext context)
        {
            _context = context;
        }

        public IEnumerable<Customer> GetAllCustomers()
        {
            return _context.User;
        }

        public async Task<Customer> GetCustomer(int id)
        {
            return await _context.User.SingleOrDefaultAsync(u => u.CustomerId == id);
        }

    }
}
