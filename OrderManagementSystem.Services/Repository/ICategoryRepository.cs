using OrderManagementSystem.Entity.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderManagementSystem.Services.Repository
{
    public interface ICategoryRepository
    {
        public Task<List<Category>> GetAllCategoriesAsync(String categoryName, String decription);
        public Task<List<Category>> GetAllCategoriesAsync();
        public Task<Category> GetCategoryByIdAsync(int id);
        public Task AddCategoryAsync(Category category);
        public Task UpdateCategoryAsync(Category category);
        public Task DeleteCategoryAsync(int id);
    }
}
