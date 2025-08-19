using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using cpsy200FinalProject.Data;

namespace cpsy200FinalProject.Interfaces
{
    public interface IRentalService
    {
        List<RentalItem> GetAllRentals();
        RentalItem? GetRentalById(int id);
        void ProcessRental(RentalItem rental, Customer customer);
        void CompleteRental(int rentalId, Equipment equipment);

        // Queries
        List<RentalItem> GetActiveRentals();

        // Calculations
        decimal CalculateRentalCost(int equipmentId, DateTime rentalDate, DateTime returnDate, Equipment equipment);

        // Validation
        bool IsEquipmentAvailable(int equipmentId, DateTime rentalDate, DateTime returnDate);
    }
}
}
