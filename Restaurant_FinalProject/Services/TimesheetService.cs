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
    public class TimesheetService
    {
        private readonly RestaurantDbContext _context;

        public TimesheetService(RestaurantDbContext context)
        {
            _context = context;
        }

        public async Task<List<Timesheet>> GetAllTimesheetsAsync()
        {
            return await _context.Timesheets.Include(t => t.Employee).ToListAsync();
        }

        public async Task<Timesheet> GetTimesheetByIdAsync(int id)
        {
            return await _context.Timesheets.Include(t => t.Employee).FirstOrDefaultAsync(t => t.TimesheetID == id);
        }

        public async Task<bool> AddTimesheetAsync(Timesheet timesheet)
        {
            try
            {
                await _context.Timesheets.AddAsync(timesheet);
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error adding timesheet: {ex.Message}");
                return false;
            }
        }

        public async Task<bool> UpdateTimesheetAsync(Timesheet timesheet)
        {
            try
            {
                _context.Timesheets.Update(timesheet);
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error updating timesheet: {ex.Message}");
                return false;
            }
        }

        public async Task<bool> DeleteTimesheetAsync(int id)
        {
            try
            {
                var timesheet = await _context.Timesheets.FindAsync(id);
                if (timesheet != null)
                {
                    _context.Timesheets.Remove(timesheet);
                    await _context.SaveChangesAsync();
                    return true;
                }
                return false;
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error deleting timesheet: {ex.Message}");
                return false;
            }
        }

        public async Task<List<Timesheet>> GetTimesheetsByEmployeeAsync(int employeeId)
        {
            return await _context.Timesheets
                .Where(t => t.EmployeeID == employeeId)
                .Include(t => t.Employee)
                .OrderByDescending(t => t.Date)
                .ToListAsync();
        }

        public async Task<List<Timesheet>> GetTimesheetsByDateRangeAsync(DateTime startDate, DateTime endDate)
        {
            return await _context.Timesheets
                .Where(t => t.Date >= startDate && t.Date <= endDate)
                .Include(t => t.Employee)
                .OrderBy(t => t.Date)
                .ToListAsync();
        }

        public async Task<decimal> GetTotalHoursWorkedAsync(int employeeId, DateTime startDate, DateTime endDate)
        {
            return await _context.Timesheets
                .Where(t => t.EmployeeID == employeeId && t.Date >= startDate && t.Date <= endDate)
                .SumAsync(t => t.HoursWorked);
        }
    }
}
