using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace Restaurant_FinalProject.Data
{
    public class Report
    {
        [Key]
        public int ReportID { get; set; }

        [Required]
        public DateTime Month { get; set; }

        [Required]
        [Range(0, double.MaxValue)]
        public decimal MonthlySpent { get; set; }

        [Required]
        [Range(0, double.MaxValue)]
        public decimal LabourSpending { get; set; }

        [Required]
        [Range(0, double.MaxValue)]
        public decimal InventorySpending { get; set; }

        [Required]
        [Range(0, double.MaxValue)]
        public decimal Rent { get; set; }

        [Required]
        [Range(0, double.MaxValue)]
        public decimal MonthlyReceived { get; set; }

        public DateTime DateGenerated { get; set; } = DateTime.Now;

        
        // Calculates the profit for the month
        
        // <returns>Profit (revenue - expenses)</returns>
        public decimal CalculateProfit()
        {
            decimal totalExpenses = MonthlySpent + LabourSpending + InventorySpending + Rent;
            return MonthlyReceived - totalExpenses;
        }

        
        // Generates a summary report string
       
        // <returns>Formatted report summary</returns>
        public string GenerateReportSummary()
        {
            return $"Report for {Month:MMMM yyyy}\n" +
                   $"Revenue: {MonthlyReceived:C}\n" +
                   $"Expenses: {CalculateTotalExpenses():C}\n" +
                   $"Profit: {CalculateProfit():C}";
        }

        
        // Calculates total expenses
        
        // <returns>Sum of all expenses</returns>
        public decimal CalculateTotalExpenses()
        {
            return MonthlySpent + LabourSpending + InventorySpending + Rent;
        }
    }
}
