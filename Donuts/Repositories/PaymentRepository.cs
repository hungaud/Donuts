using Donuts.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Donuts.Repositories
{

    public interface IPaymentRepository
    {
        Task<Payment> GetPayment(int id);
        Task<Payment> GetPaymentFromCustomer(int customerId);
        Task<Payment> UpdatePayment(Payment payment);
        Task<Payment> AddPayment(Payment payment);

    }

    public class PaymentRepository : IPaymentRepository
    {
        private DonutsContext _context;

        public PaymentRepository(DonutsContext donutsContext)
        {
            _context = donutsContext;
        }

        public async Task<Payment> AddPayment(Payment payment)
        {
            await _context.Payment.AddAsync(payment);
            await _context.SaveChangesAsync();
            return payment;
        }

        public async Task<Payment> GetPayment(int id)
        {
            return await _context.Payment.Where(p => p.PaymentId == id).FirstOrDefaultAsync();
        }

        public async Task<Payment> GetPaymentFromCustomer(int customerId)
        {
            return await _context.Payment.Where(p => p.CustomerId == customerId).FirstOrDefaultAsync();
        }

        public async Task<Payment> UpdatePayment(Payment payment)
        {
            _context.Payment.Update(payment);
            await _context.SaveChangesAsync();
            return payment;
        }
    }
}
