using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace cpsy200FinalProject.Data
{
    public class Equipment
    {
        private int equipmentId;
        private int categoryId; // Add this field
        private string name;
        private string description;
        private double dailyRentalCost;
        private bool status;

        public int EquipmentId { get => equipmentId; set => equipmentId = value; }
        public int CategoryId { get => categoryId; set => categoryId = value; } // Add this property
        public string Name { get => name; set => name = value; }
        public string Description { get => description; set => description = value; }
        public double DailyRentalCost { get => dailyRentalCost; set => dailyRentalCost = value; }
        public bool Status { get => status; set => status = value; }

        public Equipment()
        {

        }
        //public Equipment(int equipmentId, string name, string description, double dailyRentalCost, bool status)
        //{
        //    this.EquipmentId = equipmentId;
        //    this.Name = name;
        //    this.Description = description;
        //    this.DailyRentalCost = dailyRentalCost;
        //    this.Status = status;
        // }
        public Equipment(int equipmentId, int categoryId, string name, string description, double dailyRentalCost)
        {
            this.EquipmentId = equipmentId;
            this.Name = name;
            this.Description = description;
            this.DailyRentalCost = dailyRentalCost;
            this.CategoryId = categoryId;
        }
        public override string ToString()
        {
            return $"{Name} - {Description} - ${DailyRentalCost}/day - Status: {(Status ? "Available" : "Not Available")}";
        }
    }
}
