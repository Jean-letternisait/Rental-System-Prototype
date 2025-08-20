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
    public class ReportService
    {
        private readonly RestaurantDbContext _context;

        public ReportService(RestaurantDbContext context)
        {
            _context = context;
        }

        public async Task<List<Report>> GetAllReportsAsync()
        {
            return await _context.Reports.ToListAsync();
        }

        public async Task<Report> GetReportByIdAsync(int id)
        {
            return await _context.Reports.FindAsync(id);
        }

        public async Task<bool> AddReportAsync(Report report)
        {
            try
            {
                await _context.Reports.AddAsync(report);
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error adding report: {ex.Message}");
                return false;
            }
        }

        public async Task<bool> GenerateMonthlyReportAsync(DateTime month)
        {
            try
            {
                // Calculate report data
                var startDate = new DateTime(month.Year, month.Month, 1);
                var endDate = startDate.AddMonths(1).AddDays(-1);

                var orders = await _context.Orders
                    .Where(o => o.OrderDate >= startDate && o.OrderDate <= endDate)
                    .ToListAsync();

                var timesheets = await _context.Timesheets
                    .Where(t => t.Date >= startDate && t.Date <= endDate)
                    .ToListAsync();

                var inventorySpending = await _context.InventoryItems
                    .Where(i => i.DeliveredDate >= startDate && i.DeliveredDate <= endDate)
                    .SumAsync(i => i.Quantity * i.CostPerUnit);

                var totalRevenue = orders.Where(o => o.Status == "Completed").Sum(o => o.TotalAmount);
                var laborCost = timesheets.Sum(t => t.HoursWorked * t.Rate);

                // Create and save report
                var report = new Report
                {
                    Month = startDate,
                    MonthlySpent = inventorySpending + laborCost + 2000, // 2000 for rent
                    LabourSpending = laborCost,
                    InventorySpending = inventorySpending,
                    Rent = 2000, // Fixed rent
                    MonthlyReceived = totalRevenue,
                    DateGenerated = DateTime.Now
                };

                await _context.Reports.AddAsync(report);
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error generating report: {ex.Message}");
                return false;
            }
        }

        public async Task<decimal> GetProfitForMonthAsync(DateTime month)
        {
            var report = await _context.Reports
                .FirstOrDefaultAsync(r => r.Month.Year == month.Year && r.Month.Month == month.Month);

            return report?.CalculateProfit() ?? 0;
        }

        public async Task<List<Report>> GetReportsByDateRangeAsync(DateTime startDate, DateTime endDate)
        {
            return await _context.Reports
                .Where(r => r.Month >= startDate && r.Month <= endDate)
                .OrderBy(r => r.Month)
                .ToListAsync();
        }
    }
}
