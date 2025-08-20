using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace Restaurant_FinalProject.Data
{
    public class InventoryItem
    {
        [Key]
        public int ItemID { get; set; }

        [Required]
        [MaxLength(100)]
        public string ItemName { get; set; }

        [Required]
        [Range(0, double.MaxValue)]
        public decimal Quantity { get; set; }

        [Required]
        [MaxLength(20)]
        public string Unit { get; set; }

        [Range(0, double.MaxValue)]
        public decimal Threshold { get; set; }

        public DateTime? ExpirationDate { get; set; }

        public DateTime DeliveredDate { get; set; }

        [Range(0, double.MaxValue)]
        public decimal CostPerUnit { get; set; }

        // Foreign key and navigation property
        public int? SupplierID { get; set; }
        public virtual Supplier Supplier { get; set; }

        // Navigation property for menu items that use this inventory item
        public virtual ICollection<MenuItem> MenuItems { get; set; }

       
        // Checks if the item quantity is below the threshold
        
        // <returns>True if stock is low, false otherwise</returns>
        public bool IsStockLow()
        {
            return Quantity <= Threshold;
        }

        
        // Checks if the item is expired
       
        // <returns>True if expired, false otherwise</returns>
        public bool IsExpired()
        {
            return ExpirationDate.HasValue && ExpirationDate < DateTime.Now;
        }
    }

}
