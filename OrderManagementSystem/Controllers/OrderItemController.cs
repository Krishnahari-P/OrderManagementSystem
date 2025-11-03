using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using NToastNotify;
using OrderManagementSystem.Entity.Models;
using OrderManagementSystem.Services.Repository;

namespace OrderManagementSystem.Controllers
{
    [Authorize(Roles = "Salesman,Admin,Manager")]
    public class OrderItemController : Controller
    {
        private readonly IOrderItemRepository _orderItemRepository;
        private readonly IToastNotification _nToastNotify;
        private readonly IOrderRepository _orderRepository;

        public OrderItemController(IOrderItemRepository orderItemRepository, IToastNotification nToastNotify,IOrderRepository orderRepository)
        {
            _orderItemRepository = orderItemRepository;
            _nToastNotify = nToastNotify;
            _orderRepository = orderRepository;
        }
        public async Task<IActionResult> Index(int orderItemId, int orderId, int productId)
        {
            var orders = await _orderItemRepository.GetAllOrderItemsAsync(orderItemId,orderId , productId);
            return View(orders);
        }
        public IActionResult Create()
        {
            OrderItem orderItem = new OrderItem();
            return View(orderItem);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        [ActionName("Create")]
        public async Task<IActionResult> Create(OrderItem orderItem)
        {
            if (ModelState.IsValid)
            {
                await _orderItemRepository.AddOrderItemAsync(orderItem);
                _nToastNotify.AddSuccessToastMessage("Order item created successfully!");
                return RedirectToAction(nameof(Index));
            }
            _nToastNotify.AddErrorToastMessage("Failed to create order item. Please check input values.");
            return View(orderItem);
        }
        public async Task<IActionResult> Detail(int id)
        {
            var orderItem = await _orderItemRepository.GetOrderItemByIdAsync(id);
            if (orderItem == null)
            {
                _nToastNotify.AddErrorToastMessage("Order item not found!");
                return RedirectToAction(nameof(Index));
            }
            return View(orderItem);
        }
        public async Task<IActionResult> Edit(int id)
        {
            var orderItem = await _orderItemRepository.GetOrderItemByIdAsync(id);
            if (orderItem == null)
            {
                _nToastNotify.AddErrorToastMessage("Order item not found!");
                return RedirectToAction(nameof(Index));
            }
            return View(orderItem);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        [ActionName("Edit")]
        public async Task<IActionResult> ConfirmEdit(OrderItem orderItem)
        {
            if (ModelState.IsValid)
            {
                await _orderItemRepository.UpdateOrderItemAsync(orderItem);
                _nToastNotify.AddSuccessToastMessage("Order item updated successfully!");
                return RedirectToAction(nameof(Index));
            }
            _nToastNotify.AddErrorToastMessage("Failed to update order item. Please check input values.");
            return View(orderItem);
        }
        public async Task<IActionResult> Delete(int id)
        {
            var orderItem = await _orderItemRepository.GetOrderItemByIdAsync(id);
            if (orderItem == null)
            {
                _nToastNotify.AddErrorToastMessage("Order item not found!");
                return RedirectToAction(nameof(Index));
            }
            return View(orderItem);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        [ActionName("Delete")]
        public async Task<IActionResult> ConfirmDelete(int id)
        {
            try
            {
                await _orderItemRepository.DeleteOrderItemAsync(id);
                _nToastNotify.AddSuccessToastMessage("Order item deleted successfully!");
                return RedirectToAction(nameof(Index));
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }

        }
    }
}
