using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NToastNotify;
using OrderManagementSystem.Entity.Models;
using OrderManagementSystem.Services.Repository;

namespace OrderManagementSystem.Controllers
{
    [Authorize(Roles ="Admin,Vendor")]
    public class PurchaseController : Controller
    {
        private readonly IPurchaseRepository _purchaseRepository;
        private readonly IToastNotification _nToastNotify;

        public PurchaseController(IPurchaseRepository purchaseRepository,IToastNotification nToastNotify)
        {
            _purchaseRepository = purchaseRepository;
            _nToastNotify = nToastNotify;
        }
        // GET: PurchaseController
        public async Task<ActionResult> Index(int purchaseId, int supplierId, DateTime purchaseDate)
        {
            var purchase=await _purchaseRepository.GetAllPurchaseAsync(purchaseId, supplierId, purchaseDate);
            return View(purchase);
        }

        // GET: PurchaseController/Details/5
        public async Task<ActionResult> Detail(int id)
        {
            var purchase = await _purchaseRepository.GetPurchaseByIdAsync(id);
            if (purchase == null)
            {
                _nToastNotify.AddErrorToastMessage("Purchase not found!");
                return RedirectToAction(nameof(Index));
            }
            return View(purchase);
        }

        // GET: PurchaseController/Create
        public async Task<ActionResult> Create()
        {
            Purchase purchase = new Purchase();
            return View(purchase);
        }

        // POST: PurchaseController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        [ActionName("Create")]
        public async Task<ActionResult> Create(Purchase purchase)
        {
            if (ModelState.IsValid)
            {
                await _purchaseRepository.AddPurchaseAsync(purchase);
                _nToastNotify.AddSuccessToastMessage("Purchase created successfully!");
                return RedirectToAction(nameof(Index));
            }
            _nToastNotify.AddErrorToastMessage("Failed to create purchase. Please check input values.");
            return View(purchase);
        }

        // GET: PurchaseController/Edit/5
        public async Task<ActionResult> Edit(int id)
        {
            var purchase =await  _purchaseRepository.GetPurchaseByIdAsync(id);
            if (purchase == null)
            {
                _nToastNotify.AddErrorToastMessage("Purchase not found!");
                return RedirectToAction(nameof(Index));
            }
            return View(purchase);
        }

        // POST: PurchaseController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        [ActionName("Edit")]
        public async Task<ActionResult> ConfirmEdit(Purchase purchase)
        {
            if (ModelState.IsValid)
            {
                await _purchaseRepository.UpdatePurchaseAsync(purchase);
                _nToastNotify.AddSuccessToastMessage("Purchase updated successfully!");
                return RedirectToAction(nameof(Index));
            }
            _nToastNotify.AddErrorToastMessage("Failed to update purchase. Please check input values.");
            return View(purchase);
        }

        // GET: PurchaseController/Delete/5
        public async Task<ActionResult> Delete(int id)
        {
            var purchase = await _purchaseRepository.GetPurchaseByIdAsync(id);
            if (purchase == null)
            {
                _nToastNotify.AddErrorToastMessage("Purchase not found!");
                return RedirectToAction(nameof(Index));
            }
            return View(purchase);
        }

        // POST: PurchaseController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        [ActionName("Delete")]
        public async Task<ActionResult> ConfirmDelete(int id)
        {
            try
            {
                await _purchaseRepository.DeletePurchaseAsync(id);
                _nToastNotify.AddSuccessToastMessage("Purchase deleted successfully!");
                return RedirectToAction(nameof(Index));
            }
            catch(Exception e) 
            {
                throw new Exception(e.Message);
            }
        }
    }
}
