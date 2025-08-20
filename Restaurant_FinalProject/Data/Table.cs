using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace Restaurant_FinalProject.Data
{
    public class Table
    {
        [Key]
        public int TableID { get; set; }

        [Required]
        [Range(1, 20)]
        public int Capacity { get; set; }

        [Required]
        public bool IsAvailable { get; set; }

        // Navigation property for reservations
        public virtual ICollection<Reservation> Reservations { get; set; }

        
        // Checks if the table can accommodate a party of given size
        
        
        // <returns>True if table can accommodate, false otherwise</returns>
        public bool CanAccommodate(int partySize)
        {
            return IsAvailable && partySize <= Capacity;
        }
        public virtual ICollection<Order> Orders { get; set; } = new List<Order>();

    }
}
