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
    public class InventoryService
    {
        private readonly RestaurantDbContext _context;

        public InventoryService(RestaurantDbContext context)
        {
            _context = context;
        }

        public async Task<List<InventoryItem>> GetAllInventoryItemsAsync()
        {
            return await _context.InventoryItems.Include(i => i.Supplier).ToListAsync();
        }

        public async Task<InventoryItem> GetInventoryItemByIdAsync(int id)
        {
            return await _context.InventoryItems.Include(i => i.Supplier).FirstOrDefaultAsync(i => i.ItemID == id);
        }

        public async Task<bool> AddInventoryItemAsync(InventoryItem item)
        {
            try
            {
                await _context.InventoryItems.AddAsync(item);
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error adding inventory item: {ex.Message}");
                return false;
            }
        }

        public async Task<bool> UpdateInventoryItemAsync(InventoryItem item)
        {
            try
            {
                _context.InventoryItems.Update(item);
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error updating inventory item: {ex.Message}");
                return false;
            }
        }

        public async Task<bool> DeleteInventoryItemAsync(int id)
        {
            try
            {
                var item = await _context.InventoryItems.FindAsync(id);
                if (item != null)
                {
                    _context.InventoryItems.Remove(item);
                    await _context.SaveChangesAsync();
                    return true;
                }
                return false;
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error deleting inventory item: {ex.Message}");
                return false;
            }
        }

        public async Task<List<InventoryItem>> GetLowStockItemsAsync()
        {
            return await _context.InventoryItems
                .Where(i => i.Quantity <= i.Threshold)
                .ToListAsync();
        }

        public async Task<List<InventoryItem>> GetExpiringSoonItemsAsync(int days = 7)
        {
            return await _context.InventoryItems
                .Where(i => i.ExpirationDate.HasValue && i.ExpirationDate <= DateTime.Now.AddDays(days))
                .ToListAsync();
        }

        public async Task<bool> UpdateInventoryQuantityAsync(int itemId, decimal quantityChange)
        {
            try
            {
                var item = await _context.InventoryItems.FindAsync(itemId);
                if (item != null)
                {
                    item.Quantity += quantityChange;
                    await _context.SaveChangesAsync();
                    return true;
                }
                return false;
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error updating inventory quantity: {ex.Message}");
                return false;
            }
        }

    }
}
