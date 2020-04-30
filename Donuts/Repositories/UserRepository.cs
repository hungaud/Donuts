using Donuts.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Donuts.Repositories
{

    public interface IUserRepository
    {
        Task<User> GetUser(int id);
        IEnumerable<User> GetAllUsers();
    }

    public class UserRepository : IUserRepository
    {

        private DonutsContext _context;

        public UserRepository(DonutsContext context)
        {
            _context = context;
        }

        public IEnumerable<User> GetAllUsers()
        {
            return _context.User;
        }

        public async Task<User> GetUser(int id)
        {
            return await _context.User.SingleOrDefaultAsync(u => u.UserId == id);
        }

    }
}
