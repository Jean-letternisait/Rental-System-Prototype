using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using cpsy200FinalProject.Data;
using cpsy200FinalProject.Interfaces;

namespace cpsy200FinalProject.Services
{
    public class RentalService : IRentalService
    {
    private readonly List<RentalItem> _rentals = new();
    private readonly IEquipmentService _equipmentService;
    private readonly ICustomerService _customerService;

    public RentalService(IEquipmentService equipmentService, ICustomerService customerService)
    {
        _equipmentService = equipmentService;
        _customerService = customerService;
        _rentals = new List<RentalItem>
        {
            new RentalItem(1000, DateTime.Parse("2024-02-15"), 149.97, 1001, DateTime.Parse("2024-02-20"), DateTime.Parse("2024-02-23"), 149.97),
            new RentalItem(1001, DateTime.Parse("2024-02-16"), 43.96, 1002, DateTime.Parse("2024-02-21"), DateTime.Parse("2024-02-25"), 43.96)
        };
    }

    public List<RentalItem> GetAllRentals() => _rentals;

    public RentalItem? GetRentalById(int id) => _rentals.FirstOrDefault(r => r.RentalId == id);


    public void ProcessRental(RentalItem rental, Customer customers)
    {
        // Validate
        var customer = _customerService.GetCustomerById(customers.CustomerId);
        if (customer == null)
            throw new KeyNotFoundException("Customer not found");

        if (_customerService.IsCustomerBanned(customers.CustomerId))
            throw new InvalidOperationException("Customer is banned from rentals");

        var equipment = _equipmentService.GetEquipmentById(rental.Equipment);
        if (equipment == null)
            throw new KeyNotFoundException("Equipment not found");

        if (!equipment.Status)
            throw new InvalidOperationException("Equipment already rented");

        // Calculate and set cost
        
            var equipmentForCost = _equipmentService.GetEquipmentById(rental.Equipment);
            rental.TotalCost = (double)CalculateRentalCost(rental.Equipment, rental.RentalDate, rental.ReturnDate, equipmentForCost);
        // Auto-generate ID if needed
        if (rental.RentalId == 0)
        {
            rental.RentalId = _rentals.Any() ?
                _rentals.Max(r => r.RentalId) + 1 : 1000;
        }

        // Update equipment status
        equipment.Status = false;

        // Add to rentals
        _rentals.Add(rental);
    }

    public void CompleteRental(int rentalId, Equipment equipments)
    {
        var rental = GetRentalById(rentalId);
        if (rental == null) return;

        var equipment = _equipmentService.GetEquipmentById(rental.Equipment);
        if (equipment != null)
        {
            equipment.Status = true;
        }

        equipments.Status = true;
    }

    public decimal CalculateRentalCost(int equipmentId, DateTime rentalDate, DateTime returnDate, Equipment equipments)
    {
        var equipment = _equipmentService.GetEquipmentById(equipmentId);
        if (equipment == null) return 0;

        var days = (returnDate - rentalDate).Days;
        return days * (decimal)equipments.DailyRentalCost;
    }

    public List<RentalItem> GetActiveRentals() =>
        _rentals.Where(r => !r.IsReturned).ToList();

        // Replace r.EquipmentId with r.Equipment in the IsEquipmentAvailable method

        public bool IsEquipmentAvailable(int equipmentId, DateTime RentalDate, DateTime ReturnDate)
        {
            var equipment = _equipmentService.GetEquipmentById(equipmentId);
            if (equipment == null || !equipment.Status) return false;

            // Check for overlapping rentals
            return !_rentals.Any(r =>
                r.Equipment == equipmentId &&
                !r.IsReturned &&
                r.RentalDate <= ReturnDate &&
                r.ReturnDate >= RentalDate);
        }
        
    
    }

}
