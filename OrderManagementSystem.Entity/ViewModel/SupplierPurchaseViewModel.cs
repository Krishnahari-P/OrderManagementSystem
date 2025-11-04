using OrderManagementSystem.Entity.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderManagementSystem.Entity.ViewModel
{
    public class SupplierPurchaseViewModel
    {
        public Purchase? Purchase { get; set; }
        //public IEnumerable<PurchaseItem> PurchaseItems { get; set; }
        public IEnumerable<Product> Products { get; set; }
    }
}
