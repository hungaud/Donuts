using Donuts.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Donuts.Repositories
{

    public interface ICustomerRepository
    {
        Task<Customer> GetCustomer(int id);
        IEnumerable<Customer> GetAllCustomers();
        Task<Customer> AddCustomer(Customer customer);
    }

    public class CustomerRepository : ICustomerRepository
    {

        private DonutsContext _context;

        public CustomerRepository(DonutsContext context)
        {
            _context = context;
        }

        public async Task<Customer> AddCustomer(Customer customer)
        {
            await _context.Customer.AddAsync(customer);
            await _context.SaveChangesAsync();
            return customer;
        }

        public IEnumerable<Customer> GetAllCustomers()
        {
            return _context.Customer;
        }

        public async Task<Customer> GetCustomer(int id)
        {
            return await _context.Customer.SingleOrDefaultAsync(u => u.CustomerId == id);
        }

    }
}
