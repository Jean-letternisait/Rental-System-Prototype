using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using cpsy200FinalProject.Data;
using cpsy200FinalProject.Interfaces;

namespace cpsy200FinalProject.Services
{
    public class CustomerService : ICustomerService
    {
        private readonly List<Customer> _customers;
        public CustomerService()
        {
            // Initialize with sample data
            _customers = new List<Customer>
            {
                new Customer(1001, "Doe", "John", "(555) 555-1212", "jd@sample.net", isBanned: false, 0),
                new Customer(1002, "Smith", "Jane", "(555) 555-3434", "js@live.com", isBanned: true, 0),
                new Customer(1003, "Lee", "Michael", "(555) 555-5656", "ml@sample.net",isBanned: false, 0)
            };
        }

        public List<Customer> GetAllCustomers() => _customers;
       
        public Customer? GetCustomerById(int customerId) =>
            _customers.FirstOrDefault(c => c.CustomerId == customerId);
        

        public  void AddCustomer(Customer customer)
        {
            // Validate email format
            if (!customer.EmailAddress.Contains("@"))
                throw new ArgumentException("Invalid email format");

            // Auto-generate ID if not provided
            if (customer.CustomerId == 0)
            {
                customer.CustomerId = _customers.Any() ?
                    _customers.Max(c => c.CustomerId) + 1 : 1000;
            }

            _customers.Add(customer);
        }

        public void UpdateCustomer(Customer customer)
        {
            var existing = GetCustomerById(customer.CustomerId);
            if (existing == null)
                throw new KeyNotFoundException("Customer not found");

            existing.FirstName = customer.FirstName;
            existing.LastName = customer.LastName;
            existing.ContactPhone = customer.ContactPhone;
            existing.EmailAddress = customer.EmailAddress;
            existing.IsBanned = customer.IsBanned;
        }

        public void DeleteCustomer(int id)
        {
            var customer = GetCustomerById(id);
            if (customer != null)
            {
                _customers.Remove(customer);
            }
        }

        public bool IsCustomerBanned(int customerId)
        {
            var customer = GetCustomerById(customerId);
            return customer?.IsBanned ?? true; // Default to true if customer not found
        }
    }
}
