using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NToastNotify;
using OrderManagementSystem.Entity.Models;
using OrderManagementSystem.Services.Repository;
using System.Diagnostics.Contracts;

namespace OrderManagementSystem.Controllers
{
    [Authorize(Roles = "Salesman,Admin")]
    public class CustomerController : Controller
    {
        private readonly ICustomerRepository _customerRepository;
        private readonly IToastNotification _nToastNotify;

        public CustomerController(ICustomerRepository customerRepository, IToastNotification nToastNotify)
        {
            _customerRepository = customerRepository;
            _nToastNotify = nToastNotify;
        }
        public async Task<IActionResult> Index(String firstName,String lastName, String phone, String address, String email)
        {
            List<Customer> customerList = new();
            customerList = await _customerRepository.GetAllCustomersAsync(firstName,lastName,phone, address, email);
            return View(customerList);
        }
        public async Task<IActionResult> List(String firstName,String lastName, String phone, String address, String email)
        {
            List<Customer> customerList = new();
            customerList = await _customerRepository.GetAllCustomersAsync(firstName,lastName,phone, address, email);
            return View(customerList);
        }

        public async Task<IActionResult> ListJson(String firstName, String lastName, String phone, String address, String email)
        {
            List<Customer> customerList = new();
            customerList = await _customerRepository.GetAllCustomersAsync(firstName, lastName,phone, address, email);
            return Json(new { data = customerList });
        }
        public async Task<IActionResult> Create()
        {
            Customer customer = new Customer();
            return View(customer); 
        }
        public async Task<IActionResult> Detail(int id)
        {
            var customer = await _customerRepository.GetCustomerByIdAsync(id);
            if (customer == null)
            {
                _nToastNotify.AddErrorToastMessage("Customer not found!");
                return RedirectToAction(nameof(Index));
            }
            _nToastNotify.AddErrorToastMessage("Failed to create customer. Please check input values.");
            return View(customer);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        [ActionName("Create")]
        public async Task<IActionResult> Create(Customer customer) 
        {
            if (ModelState.IsValid)
            {              
                await _customerRepository.AddCustomerAsync(customer);
                _nToastNotify.AddSuccessToastMessage("Saved Successfully");
                return RedirectToAction(nameof(Index));
            }
            return View();
        }
        public async Task<IActionResult> Edit(int id)
        {
            var customer = await _customerRepository.GetCustomerByIdAsync(id);
            if (customer == null)
            {
                _nToastNotify.AddErrorToastMessage("Customer not found!");
                return RedirectToAction(nameof(Index));
            }
            return View(customer);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        [ActionName("Edit")]
        public async Task<IActionResult> ConfirmEdit(Customer customer)
        {
            if (ModelState.IsValid)
            {
                await _customerRepository.UpdateCustomerAsync(customer);
                _nToastNotify.AddSuccessToastMessage("Edited Successfully");
                return RedirectToAction(nameof(Index));
            }
            _nToastNotify.AddErrorToastMessage("Failed to update customer. Please check input values.");

            return View(customer);
        }
        public async Task<IActionResult> Delete(int id)
        {
            var customer = await _customerRepository.GetCustomerByIdAsync(id);
            if (customer == null)
            {
                _nToastNotify.AddErrorToastMessage("Customer not found!");
                return RedirectToAction(nameof(Index));
            }
            return View(customer);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        [ActionName("Delete")]
        public async Task<IActionResult> ConfirmDelete(int id)
        {
            //var customer = await _customerRepository.GetCustomerByIdAsync(id);
            try
            {
                await _customerRepository.DeleteCustomerAsync(id);
                _nToastNotify.AddSuccessToastMessage("Record deleted successfully");
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

        }

    }
}
