using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NToastNotify;
using OrderManagementSystem.Entity.Models;
using OrderManagementSystem.Services.Repository;

namespace OrderManagementSystem.Controllers
{
    [Authorize(Roles ="Admin,Manager,Vendor")]
    public class PurchaseItemController : Controller
    {
        private readonly IPurchaseItemRepository _purchaseItemRepository;
        private readonly IToastNotification _nToastNotify;

        public PurchaseItemController(IPurchaseItemRepository purchaseItemRepository,IToastNotification nToastNotify)
        {
            _purchaseItemRepository = purchaseItemRepository;
            _nToastNotify = nToastNotify;
        }
        // GET: PurchaseItemController
        public async Task<ActionResult> Index(int purchaseItemId, int purchaseId, int productId)
        {
            var purchaseItem= await _purchaseItemRepository.GetAllPurchaseItemAsync(purchaseItemId, purchaseId, productId);
            return View(purchaseItem);
        }

        // GET: PurchaseItemController/Details/5
        public async Task<ActionResult> Detail(int id)
        {
            var purchaseItem = await _purchaseItemRepository.GetPurchaseItemByIdAsync(id);
            if (purchaseItem == null)
            {
                _nToastNotify.AddErrorToastMessage("Purchase item not found!");
                return RedirectToAction(nameof(Index));
            }
            return View(purchaseItem);
        }

        // GET: PurchaseItemController/Create
        public async Task<ActionResult> Create()
        {
            PurchaseItem purchaseItem=new PurchaseItem();
            return View(purchaseItem);
        }

        // POST: PurchaseItemController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        [ActionName("Create")]
        public async Task<ActionResult> Create(PurchaseItem purchaseItem)
        {
            if (ModelState.IsValid)
            {
                await _purchaseItemRepository.AddPurchaseItemAsync(purchaseItem);
                _nToastNotify.AddSuccessToastMessage("Purchase item created successfully!");
                return RedirectToAction(nameof(Index));
            }
            _nToastNotify.AddErrorToastMessage("Failed to create purchase item. Please check input values.");
            return View(purchaseItem);
        }

        // GET: PurchaseItemController/Edit/5
        public async Task<ActionResult> Edit(int id)
        {
            var purchaseItem = await _purchaseItemRepository.GetPurchaseItemByIdAsync(id);
            if (purchaseItem == null)
            {
                _nToastNotify.AddErrorToastMessage("Purchase item not found!");
                return RedirectToAction(nameof(Index));
            }
            return View(purchaseItem);
        }

        // POST: PurchaseItemController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        [ActionName("Edit")]
        public async Task<ActionResult> ConfirmEdit(PurchaseItem purchaseItem)
        {
            if (ModelState.IsValid)
            {
                await _purchaseItemRepository.UpdatePurchaseItemAsync(purchaseItem);
                _nToastNotify.AddSuccessToastMessage("Purchase item updated successfully!");
                return RedirectToAction(nameof(Index));
            }
            _nToastNotify.AddErrorToastMessage("Failed to update purchase item. Please check input values.");
            return View(purchaseItem);
        }

        // GET: PurchaseItemController/Delete/5
        public async Task<ActionResult> Delete(int id)
        {
            var purchaseItem = await _purchaseItemRepository.GetPurchaseItemByIdAsync(id);
            if (purchaseItem == null)
            {
                _nToastNotify.AddErrorToastMessage("Purchase item not found!");
                return RedirectToAction(nameof(Index));
            }
            return View(purchaseItem);
        }

        // POST: PurchaseItemController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        [ActionName("Delete")]
        public async Task<ActionResult> ConfirmDelete(int id, IFormCollection collection)
        {
            try
            {
                await _purchaseItemRepository.DeletePurchaseItemAsync(id);
                _nToastNotify.AddSuccessToastMessage("Purchase item deleted successfully!");
                return RedirectToAction(nameof(Index));
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }
    }
}
