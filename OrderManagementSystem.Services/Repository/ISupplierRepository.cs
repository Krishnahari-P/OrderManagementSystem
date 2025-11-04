using OrderManagementSystem.Entity.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderManagementSystem.Services.Repository
{
    public interface ISupplierRepository
    {
        public Task<List<Supplier>> GetAllSupplierAsync(int supplierId ,String supplierName);
        public Task<List<Supplier>> GetAllSupplierAsync();
        public Task<Supplier> GetSupplierByIdAsync(int id);
        public Task AddSupplierAsync(Supplier supplier);
        public Task UpdateSupplierAsync(Supplier supplier);
        public Task DeleteSupplierAsync(int id);
    }
}
