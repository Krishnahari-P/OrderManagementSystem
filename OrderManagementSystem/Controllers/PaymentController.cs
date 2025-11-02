using Microsoft.AspNetCore.Mvc;
using NToastNotify;
using OrderManagementSystem.Entity.Models;
using OrderManagementSystem.Services.Repository;

namespace OrderManagementSystem.Controllers
{
    public class PaymentController : Controller
    {
        private readonly IPaymentRepository _paymentRepository;
        private readonly IToastNotification _nToastNotify;

        public PaymentController(IPaymentRepository paymentRepository,IToastNotification nToastNotify)
        {
            _paymentRepository = paymentRepository;
            _nToastNotify = nToastNotify;
        }
        public async Task<IActionResult> Index(int paymentId, DateTime paymentDate, decimal amountPaid)
        {
            var payment = await _paymentRepository.GetAllPaymentsAsync(paymentId, paymentDate, amountPaid);
            return View(payment);
        }
        public IActionResult Create()
        {
            Payment payment = new Payment();
            return View(payment);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        [ActionName("Create")]
        public async Task<IActionResult> Create(Payment payment)
        {
            if (ModelState.IsValid)
            {
                await _paymentRepository.AddPaymentAsync(payment);
                _nToastNotify.AddSuccessToastMessage("Payment created successfully!");
                return RedirectToAction(nameof(Index));
            }
            _nToastNotify.AddErrorToastMessage("Failed to create payment");
            return View(payment);
        }
        public async Task<IActionResult> Detail(int id)
        {
            var payment = await _paymentRepository.GetPaymentByIdAsync(id);
            if (payment == null)
            {
                _nToastNotify.AddErrorToastMessage("Payment not found!");
                return RedirectToAction(nameof(Index));
            }
            return View(payment);
        }
        public async Task<IActionResult> Edit(int id)
        {
            var payment = await _paymentRepository.GetPaymentByIdAsync(id);
            if (payment == null)
            {
                _nToastNotify.AddErrorToastMessage("Payment not found!");
                return RedirectToAction(nameof(Index));
            }
            return View(payment);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        [ActionName("Edit")]
        public async Task<IActionResult> ConfirmEdit(Payment payment)
        {
            if (ModelState.IsValid)
            {
                await _paymentRepository.UpdatePaymentAsync(payment);
                _nToastNotify.AddSuccessToastMessage("Payment record updated successfully!");
                return RedirectToAction(nameof(Index));
            }
            _nToastNotify.AddErrorToastMessage("Failed to update payment");
            return View(payment);
        }
        public async Task<IActionResult> Delete(int id)
        {
            var payment = await _paymentRepository.GetPaymentByIdAsync(id);
            if (payment == null)
            {
                _nToastNotify.AddErrorToastMessage("Payment not found!");
                return RedirectToAction(nameof(Index));
            }
            return View(payment);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        [ActionName("Delete")]
        public async Task<IActionResult> ConfirmDelete(int id)
        {
            try
            {
                await _paymentRepository.DeletePaymentAsync(id);
                _nToastNotify.AddSuccessToastMessage("Payment record deleted successfully!");
                return RedirectToAction(nameof(Index));
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }
    }
}
