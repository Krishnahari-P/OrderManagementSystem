using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using NToastNotify;
using OrderManagementSystem.Entity.Models;
using OrderManagementSystem.Services.Repository;
using System.Diagnostics;

namespace OrderManagementSystem.Controllers
{
    [Authorize(Roles ="Admin,Vendor")]
    public class ProductController : Controller
    {
        private readonly IProductRepository _productRepository;
        private readonly IToastNotification _nToastNotify;
        private readonly ICategoryRepository _categoryRepository;

        public ProductController(IProductRepository productRepository, IToastNotification nToastNotify,ICategoryRepository categoryRepository)
        {
            _productRepository = productRepository;
            _nToastNotify = nToastNotify;
            _categoryRepository = categoryRepository;
        }
        public async Task<IActionResult> Index(string productName, int? categoryId, decimal? minPrice, decimal? maxPrice)
        {
            var productList = await _productRepository.GetAllProductsAsync(productName, categoryId, minPrice, maxPrice);
            return View(productList);
        }
        public async Task<IActionResult> List(string productName, int? categoryId, decimal? minPrice, decimal? maxPrice)
        {
            var productList = await _productRepository.GetAllProductsAsync(productName, categoryId, minPrice, maxPrice);
            return View(productList);
        }

        public async Task<IActionResult> ListAll()
        {
            var product = await _productRepository.GetAllProductsAsync();
            return View(product);
        }
        public async Task<IActionResult> ListJson(string productName, int? categoryId, decimal? minPrice, decimal? maxPrice)
        {
            var productList = await _productRepository.GetAllProductsAsync(productName, categoryId, minPrice, maxPrice);
            return Json(new { data = productList });
        }
        public async Task<IActionResult> Create()
        {
            Product product = new Product();
            product.CategoryList = await GetCategoryList();
            return View(product);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        [ActionName("Create")]
        public async Task<IActionResult> Create(Product product)
        {
            if (ModelState.IsValid)
            {
                await _productRepository.AddProductAsync(product);
                _nToastNotify.AddSuccessToastMessage("Product saved successfully");
                return RedirectToAction(nameof(Index));
            }
            _nToastNotify.AddErrorToastMessage("Failed to create product. Please check input values.");
            return View(product);
        }
        public async Task<IActionResult> Detail(int id)
        {
            var product = await _productRepository.GetProductByIdAsync(id);
            if (product == null)
            {
                _nToastNotify.AddErrorToastMessage("Product not found!");
                return RedirectToAction(nameof(Index));
            }
            return View(product);
        }
        public async Task<IActionResult> Edit(int id)
        {
            var product = await _productRepository.GetProductByIdAsync(id);
            if (product == null)
            {
                _nToastNotify.AddErrorToastMessage("Product not found!");
                return RedirectToAction(nameof(Index));
            }
            return View(product);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        [ActionName("Edit")]
        public async Task<IActionResult> ConfirmEdit(Product product)
        {
            if (ModelState.IsValid)
            {
                await _productRepository.UpdateProductAsync(product);
                _nToastNotify.AddSuccessToastMessage("Product updated successfully");
                return RedirectToAction(nameof(Index));
            }
            _nToastNotify.AddErrorToastMessage("Failed to update product. Please check input values.");

            return View(product);
        }
        public async Task<IActionResult> Delete(int id)
        {
            var product = await _productRepository.GetProductByIdAsync(id);
            if (product == null)
            {
                _nToastNotify.AddErrorToastMessage("Product not found!");
                return RedirectToAction(nameof(Index));
            }
            return View(product);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        [ActionName("Delete")]
        public async Task<IActionResult> ConfirmDelete(int id)
        {
            try
            {
                await _productRepository.DeleteProductAsync(id);
                _nToastNotify.AddSuccessToastMessage("Product deleted successfully");
                return RedirectToAction(nameof(Index));
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        private async Task<List<SelectListItem>> GetCategoryList()
        {
            var categories = await _categoryRepository.GetAllCategoriesAsync();

            var categoryList = new List<SelectListItem>();

            foreach (Category category in categories)
            {
                categoryList.Add(new SelectListItem
                {
                    Value = category.CategoryId.ToString(),
                    Text = category.CategoryName,
                });
            }

            return categoryList;

        }
    }
}
