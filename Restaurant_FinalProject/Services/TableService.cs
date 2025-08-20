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
    public class TableService
    {
        private readonly RestaurantDbContext _context;

        public TableService(RestaurantDbContext context)
        {
            _context = context;
        }

        public async Task<List<Table>> GetAllTablesAsync()
        {
            return await _context.Tables.ToListAsync();
        }

        public async Task<Table> GetTableByIdAsync(int id)
        {
            return await _context.Tables.FindAsync(id);
        }

        public async Task<bool> AddTableAsync(Table table)
        {
            try
            {
                await _context.Tables.AddAsync(table);
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error adding table: {ex.Message}");
                return false;
            }
        }

        public async Task<bool> UpdateTableAsync(Table table)
        {
            try
            {
                _context.Tables.Update(table);
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error updating table: {ex.Message}");
                return false;
            }
        }

        public async Task<bool> DeleteTableAsync(int id)
        {
            try
            {
                var table = await _context.Tables.FindAsync(id);
                if (table != null)
                {
                    _context.Tables.Remove(table);
                    await _context.SaveChangesAsync();
                    return true;
                }
                return false;
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error deleting table: {ex.Message}");
                return false;
            }
        }

        public async Task<List<Table>> GetAvailableTablesAsync()
        {
            return await _context.Tables
                .Where(t => t.IsAvailable)
                .ToListAsync();
        }

        public async Task<List<Table>> GetTablesByCapacityAsync(int minCapacity, int maxCapacity = int.MaxValue)
        {
            return await _context.Tables
                .Where(t => t.Capacity >= minCapacity && t.Capacity <= maxCapacity)
                .ToListAsync();
        }

        public async Task<bool> UpdateTableAvailabilityAsync(int tableId, bool isAvailable)
        {
            try
            {
                var table = await _context.Tables.FindAsync(tableId);
                if (table != null)
                {
                    table.IsAvailable = isAvailable;
                    await _context.SaveChangesAsync();
                    return true;
                }
                return false;
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error updating table availability: {ex.Message}");
                return false;
            }
        }

    }
}
