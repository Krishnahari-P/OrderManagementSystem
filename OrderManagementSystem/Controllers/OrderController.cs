using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using NToastNotify;
using OrderManagementSystem.Entity.Models;
using OrderManagementSystem.Entity.ViewModel;
using OrderManagementSystem.Services.Repository;
using System.Threading.Tasks;

namespace OrderManagementSystem.Controllers
{
    [Authorize(Roles = "Salesman,Admin,Manager")]
    public class OrderController : Controller
    {
        private readonly IOrderRepository _orderRepository;
        private readonly IToastNotification _nToastNotify;
        private readonly ICustomerRepository _customerRepository;
        private readonly IProductRepository _productRepository;
        private readonly AppDbContext _context;

        public OrderController(IOrderRepository orderRepository, IToastNotification nToastNotify,ICustomerRepository customerRepository,IProductRepository productRepository,AppDbContext context)
        {
            _orderRepository = orderRepository;
            _nToastNotify = nToastNotify;
            _customerRepository = customerRepository;
            _productRepository = productRepository;
            _context = context;
        }
        public async Task<IActionResult> Index(int orderId, DateTime orderDate, String status)
        {
            var orders = await _orderRepository.GetAllOrdersAsync(orderId, orderDate , status);
            return View(orders);
        }
        public async Task<IActionResult> Create()
        {
            Order order = new Order();
            order.CustomerList= await GetCustomerList();
            return View(order);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        [ActionName("Create")]
        public async Task<IActionResult> Create(Order order)
        {
            if (ModelState.IsValid)
            {
                await _orderRepository.AddOrderAsync(order);
                _nToastNotify.AddSuccessToastMessage("Order created successfully!");
                return RedirectToAction(nameof(Index));
            }
            _nToastNotify.AddErrorToastMessage("Failed to create order. Please check input values.");
            return View(order);
        }
        public async Task<IActionResult> ListAll()
        {
            var order = await _orderRepository.GetAllOrdersAsync();
            return View(order);
        }
        public async Task<IActionResult> Detail(int id)
        {
            var order = await _orderRepository.GetOrderByIdAsync(id);
            if (order == null)
            {
                _nToastNotify.AddErrorToastMessage("Order not found!");
                return RedirectToAction(nameof(Index));
            }
            return View(order);
        }
        public async Task<IActionResult> Edit(int id)
        {
            var order = await _orderRepository.GetOrderByIdAsync(id);
            var products = await _productRepository.GetAllProductsAsync();
            var viewModel = new PlaceOrderViewModel
            {
                Order = order,
                Products = products
            };
            return View(viewModel);
        }
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //[ActionName("Edit")]
        //public async Task<IActionResult> ConfirmEdit(Order order)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        await _orderRepository.UpdateOrderAsync(order);
        //        _nToastNotify.AddSuccessToastMessage("Order updated successfully!");
        //        return RedirectToAction(nameof(Index));
        //    }
        //    _nToastNotify.AddErrorToastMessage("Failed to update order. Please check input values.");
        //    return View(order);
        //}
        public async Task<IActionResult> Delete(int id)
        {
            var order = await _orderRepository.GetOrderByIdAsync(id);
            if (order == null)
            {
                _nToastNotify.AddErrorToastMessage("Order not found!");
                return RedirectToAction(nameof(Index));
            }
            return View(order);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        [ActionName("Delete")]
        public async Task<IActionResult> ConfirmDelete(int id)
        {
            try
            {
                await _orderRepository.DeleteOrderAsync(id);
                _nToastNotify.AddSuccessToastMessage("Order deleted successfully!");
                return RedirectToAction(nameof(Index));
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }

        }
        [HttpGet]
        public async Task<IActionResult> PlaceOrder(int customerId)
        {
            if (customerId <= 0)
                return BadRequest("Invalid customer ID");

            var order = new Order
            {
                CustomerId = customerId,
                OrderDate = DateTime.Now,
                Status = "Pending",
                TotalAmount = 0
            };

            await _orderRepository.AddOrderAsync(order);

            return RedirectToAction("Index");
        }
        [HttpPost]
        public async Task<IActionResult> AddItem(int orderId, int productId, int quantity)

        {

            var order = await _orderRepository.GetOrderByIdAsync(orderId);

            var product = await _productRepository.GetProductByIdAsync(productId);

            if (order == null || product == null)
            {
                return NotFound();

            }

            var existingItem = await _context.OrderItemSet

                .FirstOrDefaultAsync(oi => oi.OrderId == orderId && oi.ProductId == productId);

            if (existingItem != null)

            { 

                existingItem.Quantity += quantity;

                order.TotalAmount += product.UnitPrice * quantity;

                _nToastNotify.AddInfoToastMessage("Item quantity updated successfully.");

            }

            else

            {

                var newItem = new OrderItem

                {

                    OrderId = orderId,

                    ProductId = productId,

                    Quantity = quantity,

                    UnitPrice = product.UnitPrice

                };

                _context.OrderItemSet.Add(newItem);

                order.TotalAmount += product.UnitPrice * quantity;

                _nToastNotify.AddSuccessToastMessage("Item added successfully.");

            }

            await _context.SaveChangesAsync();

            return RedirectToAction("Edit", new { id = orderId });

        }



        private async Task<List<SelectListItem>> GetCustomerList()
        {
            var customers = await _customerRepository.GetAllCustomersAsync();

            var customerList = new List<SelectListItem>();

            foreach (Customer customer in customers)
            {
                customerList.Add(new SelectListItem
                {
                    Value = customer.CustomerId.ToString(),
                    Text = customer.FirstName + " " + customer.LastName,
                });
            }

            return customerList;

        }
    }
}
