using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NToastNotify;
using OrderManagementSystem.Entity.Models;
using OrderManagementSystem.Services.Repository;

namespace OrderManagementSystem.Controllers
{
    [Authorize(Roles ="Admin,Manager")]
    public class PaymentController : Controller
    {
        private readonly IPaymentRepository _paymentRepository;
        private readonly IToastNotification _nToastNotify;
        private readonly IPaymentRepository _paymentRepository1;

        public PaymentController(IPaymentRepository paymentRepository,IToastNotification nToastNotify,IPaymentRepository paymentRepository1)
        {
            _paymentRepository = paymentRepository;
            _nToastNotify = nToastNotify;
            _paymentRepository1 = paymentRepository1;
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
        [HttpGet]
        public async Task<IActionResult> CheckPayment(int orderId)
        {
            if (orderId <= 0)
                return BadRequest("Invalid order ID.");

            var order = await _paymentRepository.GetOrderForPaymentAsync(orderId);
            if (order == null)
                return NotFound("Order not found.");

            return View(order);
        }
        [HttpPost]
        public async Task<IActionResult> ConfirmPayment(int orderId)
        {
            if (orderId <= 0)
                return BadRequest("Invalid order ID.");

            bool success = await _paymentRepository.ConfirmPaymentAsync(orderId);

            if (!success)
            {
                TempData["ErrorMessage"] = "Payment confirmation failed.";
                return RedirectToAction("CheckPayment", new { orderId });
            }

            TempData["SuccessMessage"] = "Payment confirmed successfully!";
            return RedirectToAction("PaymentSuccess", new { orderId });
        }
        [HttpGet]
        public async Task<IActionResult> CheckPurchasePayment(int purchaseId)
        {
            if (purchaseId <= 0)
                return BadRequest("Invalid purchase ID.");

            var purchase = await _paymentRepository.GetPurchaseForPaymentAsync(purchaseId);
            if (purchase == null)
                return NotFound("Purchase not found.");

            return View(purchase);
        }
        [HttpPost]
        public async Task<IActionResult> ConfirmPurchasePayment(int purchaseId)
        {
            if (purchaseId <= 0)
                return BadRequest("Invalid purchase ID.");

            bool success = await _paymentRepository.ConfirmPurchasePaymentAsync(purchaseId);

            if (!success)
            {
                return RedirectToAction("CheckPurchasePayment", new { purchaseId });
            }
            return RedirectToAction("PurchasePaymentSuccess", new { purchaseId });
        }
        [HttpGet]
        public async Task<IActionResult> PaymentSuccess(int orderId)
        {
            var order = await _paymentRepository.GetOrderForPaymentAsync(orderId);
            return View(order);
        }
        public async Task<IActionResult> PurchasePaymentSuccess(int purchaseId)
        {
            var purchase = await _paymentRepository.GetPurchaseForPaymentAsync(purchaseId);
            if (purchase == null)
            {
                return NotFound("Purchase not found.");
            }

            return View(purchase);
        }
 
    }
}
