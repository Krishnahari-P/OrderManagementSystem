using Microsoft.AspNetCore.Mvc;
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

        //Payment

        public async Task<Order?> GetOrderForPaymentAsync(int orderId)
        {
            return await _context.OrderSet
                .Include(o => o.CustomerSet)
                .Include(o => o.OrderItems)
                    .ThenInclude(oi => oi.ProductSet)
                .Include(o => o.PaymentSet)
                .FirstOrDefaultAsync(o => o.OrderId == orderId);
        }

        [HttpPost]
        public async Task<bool> ConfirmPaymentAsync(int orderId)
        {
            var order = await _context.OrderSet
                .Include(o => o.OrderItems)
                .ThenInclude(oi => oi.ProductSet)
                .FirstOrDefaultAsync(o => o.OrderId == orderId);

            if (order == null)
            {
                return false;

            }
            foreach (var item in order.OrderItems)
            {
                if (item.ProductSet == null)
                {
                    return false;

                }
                if (item.ProductSet.StockQuantity < item.Quantity)
                {
                    return false;
                }
            }
            foreach (var item in order.OrderItems)
            {
                item.ProductSet.StockQuantity -= item.Quantity;
                _context.ProductSet.Update(item.ProductSet);
            }

            if (order.PaymentSet == null)
            {
                var payment = new Payment
                {
                    OrderId = order.OrderId,
                    PaymentDate = DateTime.Now,
                    AmountPaid = order.TotalAmount,
                    PaymentMethod = "Online",
                    TransactionReference = Guid.NewGuid().ToString("N").Substring(0, 12).ToUpper()
                };
                _context.PaymentSet.Add(payment);
            }
            order.Status = "Paid";
            _context.OrderSet.Update(order);
            await _context.SaveChangesAsync();

            return true;
        }
        public async Task<Purchase?> GetPurchaseForPaymentAsync(int purchaseId)
        {
            return await _context.PurchaseSet
                .Include(p => p.SupplierSet) 
                .Include(p => p.PurchaseItems)
                    .ThenInclude(pi => pi.ProductSet)
                .Include(p => p.PaymentSet)
                .FirstOrDefaultAsync(p => p.PurchaseId == purchaseId);
        }


        [HttpPost]
        public async Task<bool> ConfirmPurchasePaymentAsync(int purchaseId)
        {
            var purchase = await _context.PurchaseSet
                .Include(p => p.PurchaseItems)
                .ThenInclude(pi => pi.ProductSet)
                .FirstOrDefaultAsync(p => p.PurchaseId == purchaseId);

            if (purchase == null)
            {
                return false;
            }
            foreach (var item in purchase.PurchaseItems)
            {
                if (item.ProductSet == null)
                    return false;

                item.ProductSet.StockQuantity += item.Quantity;
                _context.ProductSet.Update(item.ProductSet);
            }
            if (purchase.PaymentSet == null)
            {
                var payment = new Payment
                {
                    PurchaseId = purchase.PurchaseId,
                    PaymentDate = DateTime.Now,
                    AmountPaid = purchase.TotalAmount,
                    PaymentMethod = "Bank Transfer", // or "Online"
                    TransactionReference = Guid.NewGuid().ToString("N").Substring(0, 12).ToUpper()
                };
                _context.PaymentSet.Add(payment);
            }
            purchase.Status = "Paid";
            _context.PurchaseSet.Update(purchase);

            await _context.SaveChangesAsync();
            return true;
        }
    }
}
