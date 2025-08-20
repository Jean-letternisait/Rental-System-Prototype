using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Restaurant_FinalProject.Data;
using Restaurant_FinalProject.Database;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;


namespace Restaurant_FinalProject.Services
{
    // Service class for handling employee-related operations
    // Contains business logic for employee management
   public class EmployeeService
    {
        private readonly RestaurantDbContext _context;

        public EmployeeService(RestaurantDbContext context)
        {
            _context = context;
        }

        
        // Gets all employees from the database
        
        // <returns>List of all employees</returns>
        public async Task<List<Employee>> GetAllEmployeesAsync()
        {
            try
            {
                return await _context.Employees.ToListAsync();
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error getting all employees: {ex.Message}");
                throw new ApplicationException("Failed to retrieve employees", ex);
            }
        }

        
        // Gets an employee by their ID
        
        
        // <returns>Employee object or null if not found</returns>
        public async Task<Employee> GetEmployeeByIdAsync(int id)
        {
            try
            {
                return await _context.Employees.FindAsync(id);
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error getting employee by ID: {ex.Message}");
                throw new ApplicationException($"Failed to retrieve employee with ID {id}", ex);
            }
        }

        
        // Adds a new employee to the database
        
        // <returns>True if successful, false otherwise</returns>
        public async Task<bool> AddEmployeeAsync(Employee employee)
        {
            try
            {
                // Basic validation
                if (employee == null)
                    throw new ArgumentNullException(nameof(employee));

                if (string.IsNullOrEmpty(employee.FirstName) || string.IsNullOrEmpty(employee.LastName))
                    throw new ArgumentException("First name and last name are required");

                if (employee.HourlyRate <= 0)
                    throw new ArgumentException("Hourly rate must be greater than 0");

                // Set default values if not provided
                if (employee.DateHired == default)
                    employee.DateHired = DateTime.Now;

                await _context.Employees.AddAsync(employee);
                await _context.SaveChangesAsync();
                return true;
            }
            catch (DbUpdateException ex)
            {
                Debug.WriteLine($"Database error adding employee: {ex.Message}");
                throw new ApplicationException("Failed to add employee to database", ex);
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error adding employee: {ex.Message}");
                throw new ApplicationException("Failed to add employee", ex);
            }
        }

        
        // Updates an existing employee
        
        
        // <returns>True if successful, false otherwise</returns>
        public async Task<bool> UpdateEmployeeAsync(Employee employee)
        {
            try
            {
                // Basic validation
                if (employee == null)
                    throw new ArgumentNullException(nameof(employee));

                var existingEmployee = await _context.Employees.FindAsync(employee.EmployeeID);
                if (existingEmployee == null)
                    throw new KeyNotFoundException($"Employee with ID {employee.EmployeeID} not found");

                // Update properties
                existingEmployee.FirstName = employee.FirstName;
                existingEmployee.LastName = employee.LastName;
                existingEmployee.PhoneNumber = employee.PhoneNumber;
                existingEmployee.Position = employee.Position;
                existingEmployee.HourlyRate = employee.HourlyRate;
                existingEmployee.Email = employee.Email;
                existingEmployee.DateFired = employee.DateFired;

                _context.Employees.Update(existingEmployee);
                await _context.SaveChangesAsync();
                return true;
            }
            catch (DbUpdateException ex)
            {
                Debug.WriteLine($"Database error updating employee: {ex.Message}");
                throw new ApplicationException("Failed to update employee in database", ex);
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error updating employee: {ex.Message}");
                throw new ApplicationException("Failed to update employee", ex);
            }
        }

        // Deletes an employee from the database
        
        
        // <returns>True if successful, false otherwise</returns>
        public async Task<bool> DeleteEmployeeAsync(int id)
        {
            try
            {
                var employee = await _context.Employees.FindAsync(id);
                if (employee == null)
                    throw new KeyNotFoundException($"Employee with ID {id} not found");

                _context.Employees.Remove(employee);
                await _context.SaveChangesAsync();
                return true;
            }
            catch (DbUpdateException ex)
            {
                Debug.WriteLine($"Database error deleting employee: {ex.Message}");
                throw new ApplicationException("Failed to delete employee from database", ex);
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error deleting employee: {ex.Message}");
                throw new ApplicationException("Failed to delete employee", ex);
            }
        }

        
        // Gets all active employees (not fired)
        
        // <returns>List of active employees</returns>
        public async Task<List<Employee>> GetActiveEmployeesAsync()
        {
            try
            {
                return await _context.Employees
                    .Where(e => e.DateFired == null || e.DateFired > DateTime.Now)
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error getting active employees: {ex.Message}");
                throw new ApplicationException("Failed to retrieve active employees", ex);
            }
        }

        
        // Gets employees by position
       
        // <returns>List of employees with the specified position</returns>
        public async Task<List<Employee>> GetEmployeesByPositionAsync(string position)
        {
            try
            {
                return await _context.Employees
                    .Where(e => e.Position.Equals(position, StringComparison.OrdinalIgnoreCase))
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error getting employees by position: {ex.Message}");
                throw new ApplicationException($"Failed to retrieve employees with position {position}", ex);
            }
        }

        
        // Calculates the total monthly labor cost for all employees
       
        // <returns>Total labor cost</returns>
        public async Task<decimal> CalculateTotalMonthlyLaborCostAsync()
        {
            try
            {
                // This is a simplified calculation - in a real app, you'd use timesheet data
                var activeEmployees = await GetActiveEmployeesAsync();
                return activeEmployees.Sum(e => e.HourlyRate * 160); // Assuming 160 hours/month
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error calculating labor cost: {ex.Message}");
                throw new ApplicationException("Failed to calculate labor cost", ex);
            }
        }

        
        // Terminates an employee (sets their termination date)
         
        // <returns>True if successful</returns>
        public async Task<bool> TerminateEmployeeAsync(int employeeId, DateTime? terminationDate = null)
        {
            try
            {
                var employee = await _context.Employees.FindAsync(employeeId);
                if (employee == null)
                    throw new KeyNotFoundException($"Employee with ID {employeeId} not found");

                employee.DateFired = terminationDate ?? DateTime.Now;
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error terminating employee: {ex.Message}");
                throw new ApplicationException("Failed to terminate employee", ex);
            }
        }
    }
}
