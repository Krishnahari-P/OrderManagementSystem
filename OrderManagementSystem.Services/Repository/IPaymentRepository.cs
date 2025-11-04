using OrderManagementSystem.Entity.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderManagementSystem.Services.Repository
{
    public interface IPaymentRepository
    {
        public Task<List<Payment>> GetAllPaymentsAsync(int paymentId, DateTime paymentDate, decimal amountPaid);
        public Task<Payment> GetPaymentByIdAsync(int id);
        public Task AddPaymentAsync(Payment payment);
        public Task UpdatePaymentAsync(Payment payment);
        public Task DeletePaymentAsync(int id);
        public Task<Order?> GetOrderForPaymentAsync(int orderId);
        public Task<bool> ConfirmPaymentAsync(int orderId);
        public Task<Purchase?> GetPurchaseForPaymentAsync(int purchaseId);
        public Task<bool> ConfirmPurchasePaymentAsync(int purchaseId);
    }
}
