using Microsoft.EntityFrameworkCore;
using OrderManagementSystem.Entity.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderManagementSystem.Services.Repository
{
    public class CategoryRepository : ICategoryRepository
    {
        private readonly AppDbContext _context;

        public CategoryRepository(AppDbContext context)
        {
            _context = context;
        }
        public async Task AddCategoryAsync(Category category)
        {
            _context.CategorySet.Add(category);
            await _context.SaveChangesAsync();
        }
        public async Task DeleteCategoryAsync(int id)
        {
            var category = await _context.CategorySet.FindAsync(id);
            if (category != null)
            {
                _context.CategorySet.Remove(category);
                await _context.SaveChangesAsync();
            }
        }
        public async Task<List<Category>> GetAllCategoriesAsync(String categoryName, String description)
        {
            var query = from category in _context.CategorySet select category;
            if (!string.IsNullOrWhiteSpace(categoryName))
            {
                query = query.Where(x => x.CategoryName.Contains(categoryName));
            }
            if (!string.IsNullOrWhiteSpace(description))
            {
                query = query.Where(x => x.Description.Contains(description));
            }
            var categoryList = await query.ToListAsync();
            return categoryList;
        }

        public async Task<Category> GetCategoryByIdAsync(int id)
        {
            var category = await _context.CategorySet.FindAsync(id);
            return category ?? throw new NotImplementedException();
        }
        public async Task UpdateCategoryAsync(Category category)
        {
            _context.CategorySet.Update(category);
            await _context.SaveChangesAsync();
        }

        
    }
}


