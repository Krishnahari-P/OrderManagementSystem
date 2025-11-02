using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OrderManagementSystem.Entity.Models;
using OrderManagementSystem.Entity.ViewModel;

namespace OrderManagementSystem.Controllers
{
    [Authorize]
    public class DashboardController : Controller
    {
        private readonly AppDbContext _context;

        public DashboardController(AppDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            var model = new DashboardViewModel
            {
                TotalCustomers = _context.CustomerSet.Count(),
                TotalOrders = _context.OrderSet.Count(),
                TotalProducts = _context.ProductSet.Count(),
                TotalSuppliers = _context.SupplierSet.Count(),
                TotalPayments = _context.PaymentSet.Count(),
                TotalRevenue = _context.PaymentSet.Sum(p => (decimal?)p.AmountPaid) ?? 0
            };
            return View(model);
        }
    }
}
