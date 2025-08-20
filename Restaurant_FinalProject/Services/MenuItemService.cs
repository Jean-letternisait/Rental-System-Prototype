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
    public class MenuItemService
    {
        
        private readonly RestaurantDbContext _context;

        public MenuItemService(RestaurantDbContext context)
        {
            _context = context;
        }

        public async Task<List<Data.MenuItem>> GetAllMenuItemsAsync()
        {
            return await _context.MenuItems.Include(m => m.InventoryItem).ToListAsync();
        }

        public async Task<Data.MenuItem> GetMenuItemByIdAsync(int id)
        {
            return await _context.MenuItems.Include(m => m.InventoryItem).FirstOrDefaultAsync(m => m.MenuItemID == id);
        }

        public async Task<bool> AddMenuItemAsync(Data.MenuItem menuItem)
        {
            try
            {
                await _context.MenuItems.AddAsync(menuItem);
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error adding menu item: {ex.Message}");
                return false;
            }
        }

        public async Task<bool> UpdateMenuItemAsync(Data.MenuItem menuItem)
        {
            try
            {
                _context.MenuItems.Update(menuItem);
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error updating menu item: {ex.Message}");
                return false;
            }
        }

        public async Task<bool> DeleteMenuItemAsync(int id)
        {
            try
            {
                var menuItem = await _context.MenuItems.FindAsync(id);
                if (menuItem != null)
                {
                    _context.MenuItems.Remove(menuItem);
                    await _context.SaveChangesAsync();
                    return true;
                }
                return false;
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error deleting menu item: {ex.Message}");
                return false;
            }
        }

        public async Task<List<Data.MenuItem>> GetMenuItemsByCategoryAsync(string category)
        {
            return await _context.MenuItems
                .Where(m => m.Category.Equals(category, StringComparison.OrdinalIgnoreCase))
                .ToListAsync();
        }

        public async Task<List<Data.MenuItem>> GetAvailableMenuItemsAsync()
        {
            return await _context.MenuItems
                .Where(m => m.Availability)
                .ToListAsync();
        }
    }
}
  
