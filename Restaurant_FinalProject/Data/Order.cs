using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace Restaurant_FinalProject.Data
{
    public class Order
    {
        [Key]
        public int OrderID { get; set; }

        [Required]
        public DateTime OrderDate { get; set; } = DateTime.Now;

        [Required]
        [MaxLength(20)]
        public string Status { get; set; } = "Pending"; // Pending, In Progress, Completed, Cancelled

        [Range(0, double.MaxValue)]
        public decimal TotalAmount { get; set; }

        // Foreign key and navigation property
        public int CustomerID { get; set; }
        public virtual Customer Customer { get; set; }

        public int? TableID { get; set; }
        public virtual Table Table { get; set; }

        // Navigation property for order items
        public virtual ICollection<OrderItem> OrderItems { get; set; }

        
        // Calculates the total amount of the order based on order items
        
        public void CalculateTotal()
        {
            TotalAmount = OrderItems?.Sum(oi => oi.Price * oi.Quantity) ?? 0;
        }

        
        // Updates the order status
        
        
        // <returns>True if status was updated successfully</returns>
        public bool UpdateStatus(string newStatus)
        {
            var validStatuses = new[] { "Pending", "In Progress", "Completed", "Cancelled" };
            if (validStatuses.Contains(newStatus))
            {
                Status = newStatus;
                return true;
            }
            return false;
        }
    }
}
