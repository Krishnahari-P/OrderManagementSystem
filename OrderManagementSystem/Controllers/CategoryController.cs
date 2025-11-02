using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NToastNotify;
using OrderManagementSystem.Entity.Models;
using OrderManagementSystem.Services.Repository;

namespace OrderManagementSystem.Controllers
{
    [Authorize(Roles = "Admin,Vendor")]
    public class CategoryController : Controller
    {
        private readonly ICategoryRepository _categoryRepository;
        private readonly IToastNotification _nToastNotify;

        public CategoryController(ICategoryRepository categoryRepository, IToastNotification nToastNotify)
        {
            _categoryRepository = categoryRepository;
            _nToastNotify = nToastNotify;
        }
        public async Task<IActionResult> Index(String categoryName, String description)
        {
            var categoryList = await _categoryRepository.GetAllCategoriesAsync(categoryName,description);
            return View(categoryList);
        }

        public IActionResult Create()
        {
            Category category = new Category();
            return View(category);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [ActionName("Create")]
        public async Task<IActionResult> Create(Category category)
        {
            if (ModelState.IsValid)
            {
                await _categoryRepository.AddCategoryAsync(category);
                _nToastNotify.AddSuccessToastMessage("Category added successfully!");
                return RedirectToAction(nameof(Index));
            }
            _nToastNotify.AddErrorToastMessage("Failed to create category. Please check input values.");
            return View(category);
        }
        public async Task<IActionResult> Detail(int id)
        {
            var category = await _categoryRepository.GetCategoryByIdAsync(id);
            if (category == null)
            {
                _nToastNotify.AddErrorToastMessage("Category not found!");
                return RedirectToAction(nameof(Index));
            }
            return View(category);
        }
        public async Task<IActionResult> Edit(int id)
        {
            var category = await _categoryRepository.GetCategoryByIdAsync(id);
            if (category == null)
            {
                _nToastNotify.AddErrorToastMessage("Category not found!");
                return RedirectToAction(nameof(Index));
            }
            return View(category);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [ActionName("Edit")]
        public async Task<IActionResult> Edit(Category category)
        {
            if (ModelState.IsValid)
            {
                await _categoryRepository.UpdateCategoryAsync(category);
                _nToastNotify.AddSuccessToastMessage("Category updated successfully!");
                return RedirectToAction(nameof(Index));
            }
            _nToastNotify.AddErrorToastMessage("Failed to update category. Please check input values.");

            return View(category);
        }
        public async Task<IActionResult> Delete(int id)
        {
            var category = await _categoryRepository.GetCategoryByIdAsync(id);
            if (category == null)
            {
                _nToastNotify.AddErrorToastMessage("Category not found!");
                return RedirectToAction(nameof(Index));
            }
            return View(category);
        }
        [HttpPost]
        [ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ConfirmDelete(int id)
        {
            try
            {
                await _categoryRepository.DeleteCategoryAsync(id);
                _nToastNotify.AddSuccessToastMessage("Category deleted successfully!");
            }
            catch (Exception ex)
            {
                _nToastNotify.AddErrorToastMessage($"Error deleting category: {ex.Message}");
            }
            return RedirectToAction(nameof(Index));
        }
        
    }
}
