using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace Restaurant_FinalProject.Data
{
    public class MenuItem
    {
        [Key]
        public int MenuItemID { get; set; }

        [Required]
        [MaxLength(100)]
        public string Name { get; set; }

        [MaxLength(500)]
        public string Description { get; set; }

        [Required]
        [Range(0, double.MaxValue)]
        public decimal Price { get; set; }

        [Required]
        [MaxLength(50)]
        public string Category { get; set; }

        [Required]
        public bool Availability { get; set; }

        public string ImageURL { get; set; }

        public string PrepTime { get; set; }

        // Foreign key and navigation property for inventory item
        public int? ItemID { get; set; }
        public virtual InventoryItem InventoryItem { get; set; }

        // Navigation property for orders
        public virtual ICollection<OrderItem> OrderItems { get; set; }

        
        // Updates the availability status of the menu item
        
        public void UpdateAvailability(bool status)
        {
            this.Availability = status;
        }

        
        // Validates if the menu item can be prepared based on inventory
        
        // <returns>True if sufficient inventory exists, false otherwise</returns>
        public bool ValidateInventory()
        {
            // Checks if the menu item is available
            return Availability && (InventoryItem == null || InventoryItem.Quantity > 0);
        }
    }
}
