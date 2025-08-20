using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace Restaurant_FinalProject.Data
{
    public class Reservation
    {

        [Key]
        public int ReservationID { get; set; }

        [Required]
        public DateTime ReservationDate { get; set; }

        [Required]
        [Range(1, 20)]
        public int NumGuests { get; set; }

        [Required]
        [MaxLength(20)]
        public string Status { get; set; } = "Pending"; // Pending, Confirmed, Cancelled, Completed

        // Foreign keys and navigation properties
        public int CustomerID { get; set; }
        public virtual Customer Customer { get; set; }

        public int TableID { get; set; }
        public virtual Table Table { get; set; }

       
        // Checks if the reservation is valid (Date and table is available)
        
        // <returns>True if reservation is valid, false otherwise</returns>
        public bool IsValid()
        {
            return ReservationDate > DateTime.Now &&
                   (Status == "Pending" || Status == "Confirmed");
        }

        
        // Updates the reservation status
        
       
        // <returns>True if status was updated successfully</returns>
        public bool UpdateStatus(string newStatus)
        {
            var validStatuses = new[] { "Pending", "Confirmed", "Cancelled", "Completed" };
            if (validStatuses.Contains(newStatus))
            {
                Status = newStatus;
                return true;
            }
            return false;
        }
    }
}
