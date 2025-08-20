using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace Restaurant_FinalProject.Data
{
    // Represents an employee's timesheet 
    public class Timesheet
    {
        [Key]
        public int TimesheetID { get; set; }

        [Required]
        public DateTime Date { get; set; }

        [Required]
        [Range(0, 24)]
        public decimal HoursWorked { get; set; }

        [Required]
        [Range(0, double.MaxValue)]
        public decimal Rate { get; set; }

        [Required]
        [Range(0, 31)]
        public int DaysWorked { get; set; }

        public TimeSpan ShiftStart { get; set; }

        public TimeSpan ShiftEnd { get; set; }

        // Foreign key and navigation property
        public int EmployeeID { get; set; }
        public virtual Employee Employee { get; set; }

       
        // Calculates the total earnings for this timesheet entry
       
        // <returns>Total earnings (hours worked × rate)</returns>
        public decimal CalculateEarnings()
        {
            return HoursWorked * Rate;
        }

        
        // Checks if the shift times are valid (end time after start time)
        
        // <returns>True if shift times are valid, false otherwise</returns>
        public bool IsValidShift()
        {
            return ShiftEnd > ShiftStart;
        }
    }
}
