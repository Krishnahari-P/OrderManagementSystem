using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using NToastNotify;
using OrderManagementSystem.Entity.Models;
using OrderManagementSystem.Entity.ViewModel;
using OrderManagementSystem.Services.Repository;
using System.Diagnostics;

namespace OrderManagementSystem.Controllers
{
    [Authorize(Roles ="Admin,Vendor")]
    public class PurchaseController : Controller
    {
        private readonly IPurchaseRepository _purchaseRepository;
        private readonly IToastNotification _nToastNotify;
        private readonly ISupplierRepository _supplierRepository;
        private readonly IProductRepository _productRepository;
        private readonly AppDbContext _context;
        private readonly IPurchaseItemRepository _purchaseItemRepository;

        public PurchaseController(IPurchaseRepository purchaseRepository,IToastNotification nToastNotify, ISupplierRepository supplierRepository,IProductRepository productRepository,AppDbContext context,IPurchaseItemRepository purchaseItemRepository)
        {
            _purchaseRepository = purchaseRepository;
            _nToastNotify = nToastNotify;
            _supplierRepository = supplierRepository;
            _productRepository = productRepository;
            _context = context;
            _purchaseItemRepository = purchaseItemRepository;
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
            purchase.SupplierList = await GetSupplierList();
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
            var purchase = await _purchaseRepository.GetPurchaseByIdAsync(id);
            var products = await _productRepository.GetAllProductsAsync(); 
            //var purchaseItems = await _purchaseItemRepository.GetPurchaseItemsByPurchaseIdAsync(id);
            var viewModel = new SupplierPurchaseViewModel
            {
                Purchase= purchase,
                Products= products,
            };
            return View(viewModel);
        }

        // POST: PurchaseController/Edit/5
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //[ActionName("Edit")]
        //public async Task<ActionResult> ConfirmEdit(Purchase purchase)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        await _purchaseRepository.UpdatePurchaseAsync(purchase);
        //        _nToastNotify.AddSuccessToastMessage("Purchase updated successfully!");
        //        return RedirectToAction(nameof(Index));
        //    }
        //    _nToastNotify.AddErrorToastMessage("Failed to update purchase. Please check input values.");
        //    return View(purchase);
        //}

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

        [HttpGet]
        public async Task<IActionResult> PlaceOrder(int supplierId)
        {
            if (supplierId <= 0)
                return BadRequest("Invalid supplier ID");

            var purchase = new Purchase
            {
                SupplierId = supplierId,
                PurchaseDate = DateTime.Now,
                Status = "Pending",
                TotalAmount = 0
            };

            await _purchaseRepository.AddPurchaseAsync(purchase);

            return RedirectToAction("Index");
        }

        [HttpPost]
        public async Task<IActionResult> AddPurchaseItem(int purchaseId, int productId, int quantity, decimal unitCost)
        {
            var purchase=await _purchaseRepository.GetPurchaseByIdAsync(purchaseId);

            var product = await _context.ProductSet.FirstOrDefaultAsync(p => p.ProductId == productId);

            if (purchase == null || product == null)
            {
                return NotFound();
            }
            var existingItem = await _context.PurchaseItemSet
                .FirstOrDefaultAsync(pi => pi.PurchaseId == purchaseId && pi.ProductId == productId);

            if (existingItem != null)
            {
                existingItem.Quantity += quantity;
                existingItem.UnitCost = unitCost;
                purchase.TotalAmount += unitCost * quantity;
                //product.StockQuantity += quantity;

                _nToastNotify.AddInfoToastMessage("Item quantity updated successfully.");
            }
            else
            {
                var newItem = new PurchaseItem
                {
                    PurchaseId = purchaseId,
                    ProductId = productId,
                    Quantity = quantity,
                    UnitCost = unitCost
                };
                _context.PurchaseItemSet.Add(newItem);
                purchase.TotalAmount += unitCost * quantity;
                //product.StockQuantity += quantity;

                _nToastNotify.AddSuccessToastMessage("Item added successfully.");
            }
            //decimal profitMargin = 0.10m;
            //product.UnitPrice = unitCost + (unitCost * profitMargin);
            //_context.ProductSet.Update(product);

            await _context.SaveChangesAsync();
            return RedirectToAction("Edit", new { id = purchaseId });
        }

        private async Task<List<SelectListItem>> GetSupplierList()
        {
            var suppliers = await _supplierRepository.GetAllSupplierAsync();

            var supplierList = new List<SelectListItem>();

            foreach(Supplier supplier in suppliers)
            {
                supplierList.Add(new SelectListItem
                {
                    Value = supplier.SupplierId.ToString(),
                    Text = supplier.SupplierName,
                });
            }

            return supplierList;

        }
    }
}
