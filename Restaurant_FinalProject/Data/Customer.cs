using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace Restaurant_FinalProject.Data
{
    public class Customer : Person
    {
        [Key]
        public int CustomerID { get; set; }

        // Navigation properties for relationships
        public virtual ICollection<Reservation> Reservations { get; set; }
        public virtual ICollection<Order> Orders { get; set; }

        // Creates a new reservation for the customer
        public Reservation MakeReservation()
        {
            return new Reservation { CustomerID = this.CustomerID };
        }

        // Creates a new order for the customer
        public Order PlaceOrder()
        {
            return new Order { CustomerID = this.CustomerID };
        }
    }
}
