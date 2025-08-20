using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Restaurant_FinalProject.Data
{
    public class Supplier
    {
        /// Represents a supplier for inventory items
        /// 
            [Key]
            public int SupplierID { get; set; }

            [Required]
            [MaxLength(100)]
            public string CompanyName { get; set; }

            [Required]
            [MaxLength(200)]
            public string ContactInfo { get; set; }

            // Navigation property
            public virtual ICollection<InventoryItem> SuppliedItems { get; set; }

            
            /// Places an order for a specific inventory item
           
            public bool PlaceOrder(int itemID, double quantity)
            {
                // Create a purchase order
                Console.WriteLine($"Order placed with {CompanyName} for {quantity} of item #{itemID}");
                return true;
            }
    
    }
}
