using Microsoft.AspNetCore.Mvc;
using NToastNotify;
using OrderManagementSystem.Entity.Models;
using OrderManagementSystem.Services.Repository;

namespace OrderManagementSystem.Controllers
{
    public class OrderController : Controller
    {
        private readonly IOrderRepository _orderRepository;
        private readonly IToastNotification _nToastNotify;

        public OrderController(IOrderRepository orderRepository, IToastNotification nToastNotify)
        {
            _orderRepository = orderRepository;
            _nToastNotify = nToastNotify;
        }
        public async Task<IActionResult> Index(int orderId, DateTime orderDate, String status)
        {
            var orders = await _orderRepository.GetAllOrdersAsync(orderId, orderDate , status);
            return View(orders);
        }
        public IActionResult Create()
        {
            Order order = new Order();
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
            if (order == null)
            {
                _nToastNotify.AddErrorToastMessage("Order not found!");
                return RedirectToAction(nameof(Index));
            }
            return View(order);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        [ActionName("Edit")]
        public async Task<IActionResult> ConfirmEdit(Order order)
        {
            if (ModelState.IsValid)
            {
                await _orderRepository.UpdateOrderAsync(order);
                _nToastNotify.AddSuccessToastMessage("Order updated successfully!");
                return RedirectToAction(nameof(Index));
            }
            _nToastNotify.AddErrorToastMessage("Failed to update order. Please check input values.");
            return View(order);
        }
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
    }
}
