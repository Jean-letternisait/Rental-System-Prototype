using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace cpsy200FinalProject.Data
{
    public class RentalItem : Rental
    {
        private int equipment;
        private DateTime rentalDate;
        private DateTime returnDate;
        private double cost;
        private bool isReturned;

        public int Equipment { get => equipment; set => equipment = value; }
        public DateTime RentalDate { get => rentalDate; set => rentalDate = value; }
        public DateTime ReturnDate { get => returnDate; set => returnDate = value; }
        public double Cost { get => cost; set => cost = value; }    
        public bool IsReturned { get => isReturned; set => isReturned = value; }

        public void caluclateItemCost() 
        {
            throw new NotImplementedException("This method is not implemented yet.");
        }
        public RentalItem()
        {
             
        }
        //public RentalItem(int rentalId, DateTime currentDate, double totalCost, int equipment, DateTime rentalDate, DateTime returnDate, double cost)
        //    : base(rentalId, currentDate, totalCost)
        //{
        //    this.Equipment = equipment;
        //    this.RentalDate = rentalDate;
        //    this.ReturnDate = returnDate;
        //    this.Cost = cost;
        //}

        public RentalItem(int rentalId, DateTime currentDate, double totalCost, int equipment, DateTime rentalDate, DateTime returnDate, double cost)
            : base(rentalId, currentDate, totalCost)
        {
            this.Equipment = equipment;
            this.RentalDate = rentalDate;
            this.ReturnDate = returnDate;
            this.Cost = cost;
        }


    }
}
