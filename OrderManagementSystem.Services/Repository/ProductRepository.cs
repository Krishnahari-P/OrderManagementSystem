using Microsoft.EntityFrameworkCore;
using OrderManagementSystem.Entity.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderManagementSystem.Services.Repository
{
    public class ProductRepository:IProductRepository
    {
        private readonly AppDbContext _context;

        public ProductRepository(AppDbContext context)
        {
            _context = context;
        }
        public async Task AddProductAsync(Product product)
        {
            _context.ProductSet.Add(product);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteProductAsync(int id)
        {
            var product = await _context.ProductSet.FindAsync(id);
            if (product != null)
            {
                _context.ProductSet.Remove(product);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<List<Product>> GetAllProductsAsync(string productName, int? categoryId, decimal? minPrice, decimal? maxPrice)
        {
            var query = from product in _context.ProductSet
                        select product;

            if (!string.IsNullOrWhiteSpace(productName))
            {
                query = query.Where(x => x.ProductName.Contains(productName));
            }

            if (categoryId.HasValue)
            {
                query = query.Where(x => x.CategoryId == categoryId.Value);
            }

            if (minPrice.HasValue)
            {
                query = query.Where(x => x.UnitPrice >= minPrice.Value);
            }

            if (maxPrice.HasValue)
            {
                query = query.Where(x => x.UnitPrice <= maxPrice.Value);
            }

            return await query.ToListAsync();
        }

        public async Task<Product> GetProductByIdAsync(int id)
        {
            var product = await _context.ProductSet.FindAsync(id);
            return product ?? throw new Exception($"Product with ID {id} not found");
        }

        public async Task UpdateProductAsync(Product product)
        {
            _context.ProductSet.Update(product);
            await _context.SaveChangesAsync();
        }
    }
}
