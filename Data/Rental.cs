using cpsy200FinalProject.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace cpsy200FinalProject.Data
{
    public class Rental 
    {
        private int rentalId;
        private DateTime currentDate;
        private double totalCost;
       

public int RentalId { get => rentalId; set => rentalId = value; }
        public DateTime CurrentDate { get => currentDate; set => currentDate = value; }
        public double TotalCost { get => totalCost; set => totalCost = value; }
        

        public Rental()
        {

        }
        public Rental(int rentalId, DateTime currentDate, double totalCost)
        {
            this.rentalId = rentalId;
            this.currentDate = currentDate;
            this.totalCost = totalCost;
           
        }

        

    }
}
