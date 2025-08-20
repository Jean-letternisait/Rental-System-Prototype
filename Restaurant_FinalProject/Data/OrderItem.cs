using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace Restaurant_FinalProject.Data
{
    public class OrderItem
    {
        [Key]
        public int OrderItemID { get; set; }

        [Required]
        [Range(1, int.MaxValue)]
        public int Quantity { get; set; }

        [Required]
        [Range(0, double.MaxValue)]
        public decimal Price { get; set; }

        // Foreign keys and navigation properties
        public int OrderID { get; set; }
        public virtual Order Order { get; set; }

        public int MenuItemID { get; set; }
        public virtual MenuItem MenuItem { get; set; }

        
        // Calculates the total price for this order item
        
        // <returns>Total price (quantity × price)</returns>
        public decimal GetTotalPrice()
        {
            return Quantity * Price;
        }
    }
}
