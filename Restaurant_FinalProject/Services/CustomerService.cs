using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Restaurant_FinalProject.Database;
using Restaurant_FinalProject.Data;
using System.Data;
using System.Diagnostics;

namespace Restaurant_FinalProject.Services
{
   public class CustomerService
    {
        private readonly RestaurantDbContext _context;

        public CustomerService(RestaurantDbContext context)
        {
            _context = context;
        }

        // RAW SQL EXAMPLE: Get all customers
        public async Task<List<Customer>> GetAllCustomersRawSqlAsync()
        {
            try
            {
                return await _context.Customers
                    .FromSqlRaw("SELECT * FROM Customers")
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error executing raw SQL: {ex.Message}");
                throw;
            }
        }

        // RAW SQL EXAMPLE: Get customer by ID with parameters
        public async Task<Customer> GetCustomerByIdRawSqlAsync(int id)
        {
            try
            {
                return await _context.Customers
                    .FromSqlRaw("SELECT * FROM Customers WHERE CustomerID = {0}", id)
                    .FirstOrDefaultAsync();
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error executing raw SQL: {ex.Message}");
                throw;
            }
        }

        // RAW SQL EXAMPLE: Execute stored procedure or custom command
        public async Task<int> ExecuteRawSqlCommandAsync()
        {
            try
            {
                return await _context.Database.ExecuteSqlRawAsync(
                    "UPDATE Customers SET PhoneNumber = '555-0000' WHERE CustomerID = 1");
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error executing raw SQL command: {ex.Message}");
                throw;
            }
        }

        // RAW SQL EXAMPLE: Complex join with manual mapping
        public async Task<List<CustomerOrderSummary>> GetCustomerOrderSummaryAsync()
        {
            try
            {
                var results = new List<CustomerOrderSummary>();

                using (var command = _context.Database.GetDbConnection().CreateCommand())
                {
                    command.CommandText = @"
                        SELECT 
                            c.CustomerID,
                            c.FirstName,
                            c.LastName,
                            COUNT(o.OrderID) as OrderCount,
                            SUM(o.TotalAmount) as TotalSpent
                        FROM Customers c
                        LEFT JOIN Orders o ON c.CustomerID = o.CustomerID
                        GROUP BY c.CustomerID, c.FirstName, c.LastName
                        ORDER BY TotalSpent DESC";

                    await _context.Database.OpenConnectionAsync();

                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            results.Add(new CustomerOrderSummary
                            {
                                CustomerID = reader.GetInt32(0),
                                FirstName = reader.GetString(1),
                                LastName = reader.GetString(2),
                                OrderCount = reader.GetInt32(3),
                                TotalSpent = reader.GetDecimal(4)
                            });
                        }
                    }
                }

                return results;
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error executing complex raw SQL: {ex.Message}");
                throw;
            }
        }
    }

    // Helper class for complex SQL results
    public class CustomerOrderSummary
    {
        public int CustomerID { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public int OrderCount { get; set; }
        public decimal TotalSpent
        {
            get; set;
        }
    }
}
