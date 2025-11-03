using OrderManagementSystem.Entity.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderManagementSystem.Entity.ViewModel
{
    public class PlaceOrderViewModel
    {
       public Order? Order { get; set; }
       public IEnumerable<Product> Products { get; set; }
    }

}
