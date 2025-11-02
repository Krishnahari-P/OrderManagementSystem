using Microsoft.EntityFrameworkCore;
using OrderManagementSystem.Entity.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderManagementSystem.Services.Repository
{
    public class PaymentRepository:IPaymentRepository
    {
        private readonly AppDbContext _context;

        public PaymentRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task AddPaymentAsync(Payment payment)
        {
            _context.PaymentSet.Add(payment);
            await _context.SaveChangesAsync();
        }

        public async Task DeletePaymentAsync(int id)
        {
            var payment = await _context.PaymentSet.FindAsync(id);
            if (payment != null)
            {
                _context.PaymentSet.Remove(payment);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<List<Payment>> GetAllPaymentsAsync(int paymentId, DateTime paymentDate, decimal amountPaid)
        {
            var query = from payment in _context.PaymentSet select payment;
            if (paymentId > 0)
            {
                query = query.Where(x => x.PaymentId == paymentId);
            }
            if (paymentDate != default)
            {
                query = query.Where(x => x.PaymentDate == paymentDate);
            }
            if (amountPaid>0)
            {
                query = query.Where(x => x.AmountPaid == amountPaid);
            }
            return await query.ToListAsync();
        }

        public async Task<Payment> GetPaymentByIdAsync(int id)
        {
            var payment = await _context.PaymentSet.FindAsync(id);
            return payment ?? throw new KeyNotFoundException("Payment record not found");
        }

        public async Task UpdatePaymentAsync(Payment payment)
        {
            _context.PaymentSet.Update(payment);
            await _context.SaveChangesAsync();
        }
    }
}
