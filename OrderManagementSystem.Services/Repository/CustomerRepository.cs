using Microsoft.EntityFrameworkCore;
using OrderManagementSystem.Entity.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderManagementSystem.Services.Repository
{
    public class CustomerRepository : ICustomerRepository
    {
        private readonly AppDbContext _context;

        public CustomerRepository(AppDbContext context)
        {
            _context = context;
        }
        public async Task AddCustomerAsync(Customer customer)
        {
            _context.CustomerSet.Add(customer);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteCustomerAsync(int id)
        {
            var customer= await _context.CustomerSet.FindAsync(id);
            if (customer != null)
            {
                _context.CustomerSet.Remove(customer);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<List<Customer>> GetAllCustomersAsync(String firstName, String lastName, String phone, String address, String email)
        {
            var query = from customer in _context.CustomerSet select customer;
            if (!string.IsNullOrWhiteSpace(firstName))
            {
                query = query.Where(x => x.FirstName.Contains(firstName));
            }
            if (!string.IsNullOrWhiteSpace(lastName))
            {
                query = query.Where(x => x.LastName.Contains(lastName));
            }
            if (!string.IsNullOrWhiteSpace(phone))
            {
                query = query.Where(x => x.Phone.Contains(phone));
            }
            if (!string.IsNullOrWhiteSpace(address))
            {
                query = query.Where(x => x.Address.Contains(address));
            }
            if (!string.IsNullOrWhiteSpace(email))
            {
                query = query.Where(x => x.Email.Contains(email));
            }
            var customerList = await query.ToListAsync();
            return customerList;
        }

        public async Task<Customer> GetCustomerByIdAsync(int id)
        {
            var customer = await _context.CustomerSet.FindAsync(id);
            return customer?? throw new NotImplementedException();
        }

        public async Task UpdateCustomerAsync(Customer customer)
        {
            _context.CustomerSet.Update(customer);
            await _context.SaveChangesAsync();
        }
    }
}
