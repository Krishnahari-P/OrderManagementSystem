using Microsoft.EntityFrameworkCore;
using OrderManagementSystem.Entity.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderManagementSystem.Services.Repository
{
    public class SupplierRepository:ISupplierRepository
    {
        private readonly AppDbContext _context;

        public SupplierRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task AddSupplierAsync(Supplier supplier)
        {
            _context.SupplierSet.Add(supplier);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteSupplierAsync(int id)
        {
            var supplier = await _context.SupplierSet.FindAsync(id);
            if (supplier != null)
            {
                _context.SupplierSet.Remove(supplier);
                await _context.SaveChangesAsync();
            }  
        }

        public async Task<List<Supplier>> GetAllSupplierAsync(int supplierId, string supplierName)
        {
            var query = from supplier in _context.SupplierSet select supplier;
            if(supplierId > 0)
            {
                query=query.Where(x=>x.SupplierId == supplierId);
            }
            if (!string.IsNullOrWhiteSpace(supplierName))
            {
                query = query.Where(x => x.SupplierName.Contains(supplierName));
            }
            return await query.ToListAsync();
        }

        public async Task<Supplier> GetSupplierByIdAsync(int id)
        {
            var supplier = await _context.SupplierSet.FindAsync(id);
            return supplier ?? throw new NotImplementedException();
        }

        public async Task UpdateSupplierAsync(Supplier supplier)
        {
            _context.SupplierSet.Update(supplier);
            await _context.SaveChangesAsync();
        }
    }
}
