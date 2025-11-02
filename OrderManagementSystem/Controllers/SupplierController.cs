using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NToastNotify;
using OrderManagementSystem.Entity.Models;
using OrderManagementSystem.Services.Repository;

namespace OrderManagementSystem.Controllers
{
    public class SupplierController : Controller
    {
        private readonly ISupplierRepository _supplierRepository;
        private readonly IToastNotification _nToastNotify;

        public SupplierController(ISupplierRepository supplierRepository, IToastNotification nToastNotify)
        {
            _supplierRepository = supplierRepository;
            _nToastNotify = nToastNotify;
        }
        // GET: SupplierController
        public async Task<ActionResult> Index(int supplierId, String supplierName)
        {
            var supplier = await _supplierRepository.GetAllSupplierAsync(supplierId,supplierName);
            return View(supplier);
        }

        // GET: SupplierController/Details/5
        public async Task<ActionResult> Detail(int id)
        {
            var supplier = await _supplierRepository.GetSupplierByIdAsync(id);
            if (supplier == null)
            {
                _nToastNotify.AddErrorToastMessage("Supplier not found!");
                return RedirectToAction(nameof(Index));
            }
            return View(supplier);;
        }

        // GET: SupplierController/Create
        public async Task<ActionResult> Create()
        {
            Supplier supplier = new Supplier();
            return View(supplier);
        }

        // POST: SupplierController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        [ActionName("Create")]
        public async Task<ActionResult> Create(Supplier supplier)
        {
            if (ModelState.IsValid)
            {
                await _supplierRepository.AddSupplierAsync(supplier);
                _nToastNotify.AddSuccessToastMessage("Supplier added successfully!");
                return RedirectToAction(nameof(Index));
            }
            _nToastNotify.AddErrorToastMessage("Failed to add supplier. Please check input values.");
            return View(supplier);
        }

        // GET: SupplierController/Edit/5
        public async Task<IActionResult> Edit(int id)
        {
            var supplier = await _supplierRepository.GetSupplierByIdAsync(id);
            if (supplier == null)
            {
                _nToastNotify.AddErrorToastMessage("Supplier not found!");
                return RedirectToAction(nameof(Index));
            }
            return View(supplier);
        }

        // POST: SupplierController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        [ActionName("Edit")]
        public async Task<IActionResult> ConfirmEdit(Supplier supplier)
        {
            if (ModelState.IsValid)
            {
                await _supplierRepository.UpdateSupplierAsync(supplier);
                _nToastNotify.AddSuccessToastMessage("Supplier updated successfully!");
                return RedirectToAction(nameof(Index));
            }
            _nToastNotify.AddErrorToastMessage("Failed to update supplier. Please check input values.");
            return View(supplier);
        }

        // GET: SupplierController/Delete/5
        public async Task<IActionResult> Delete(int id)
        {
            var supplier = await _supplierRepository.GetSupplierByIdAsync(id);
            if (supplier == null)
            {
                _nToastNotify.AddErrorToastMessage("Supplier not found!");
                return RedirectToAction(nameof(Index));
            }
            return View(supplier);
        }

        // POST: SupplierController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        [ActionName("Delete")]
        public async Task<IActionResult> ConfirmDelete(int id)
        {
            try
            {
                await _supplierRepository.DeleteSupplierAsync(id);
                _nToastNotify.AddSuccessToastMessage("Supplier deleted successfully!");
                return RedirectToAction(nameof(Index));
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }

        }
    }
}
