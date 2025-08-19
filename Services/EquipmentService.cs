using cpsy200FinalProject.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using cpsy200FinalProject.Data;

namespace cpsy200FinalProject.Services
{
    public class EquipmentService : IEquipmentService
    {
        private readonly List<Equipment> _equipment;
        public EquipmentService()
        {
            _equipment = new List<Equipment>

            {
                new Equipment(101, 10, "Hammer drill", "Powerful drill for concrete", 25.99),
                new Equipment(201, 20, "Chainsaw", "Gas-powered chainsaw", 49.99),
                new Equipment(202, 20, "Lawn mower", "Self-propelled mower", 19.99),
                new Equipment(301, 30, "Small Compressor", "5 Gallon Portable", 14.99),
                new Equipment(501, 50, "Brad Nailer", "3/4 to 1 1/2 Brad Nails", 10.99)

            };
        }

        public List<Equipment> GetAllEquipment() => _equipment;

        public Equipment GetEquipmentById(int equipmentId) => _equipment.FirstOrDefault(e => e.EquipmentId == equipmentId);

        public void AddEquipment(Equipment equipment)
        {
            // Validate daily rate
            if (equipment.DailyRentalCost <= 0)
                throw new ArgumentException("Daily rate must be positive");

            // Auto-generate ID if not provided
            if (equipment.EquipmentId == 0)
            {
                equipment.EquipmentId = _equipment.Any() ?
                    _equipment.Max(e => e.EquipmentId) + 1 : 100;
            }

            equipment.Status = true;
            _equipment.Add(equipment);
        }

        public void UpdateEquipment(Equipment equipment)
        {
            var existing = GetEquipmentById(equipment.EquipmentId);
            if (existing == null)
                throw new KeyNotFoundException("Equipment not found");

            existing.Name = equipment.Name;
            existing.Description = equipment.Description;
            existing.DailyRentalCost = equipment.DailyRentalCost;
            existing.CategoryId = equipment.CategoryId;
            // Note: IsAvailable should be modified via rentals, not direct update
        }

        public void DeleteEquipment(int equipmentId)
        {
            var equipment = GetEquipmentById(equipmentId);
            if (equipment == null) return;

            // Prevent deletion if equipment is currently rented
            if (!equipment.Status)
                throw new InvalidOperationException("Cannot delete rented equipment");

            _equipment.Remove(equipment);
        }
        public List<Equipment> GetAvailableEquipment()
        {
            return _equipment.Where(e => e.Status ).ToList();
        }
    }
}
