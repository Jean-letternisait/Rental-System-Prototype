using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace Restaurant_FinalProject.Data
{
    // Represents an employee in the restaurant system
    public class Employee : Person

    {
        [Key]
        public int EmployeeID { get; set; }
        [Required]
        public DateTime DateHired { get; set; }
        public DateTime DateFired { get; set; }
        [Required]
        [MaxLength(50)]
        public string Position { get; set; }

        [Required]
        [Range(0, double.MaxValue, ErrorMessage = "Hourly rate must be a positive value.")]
        public decimal HourlyRate { get; set; }

        [EmailAddress]
        [MaxLength(100)]
        public string Email { get; set; }

        // Calculates if the employee is currently active
        public bool IsActive()
        {
        return DateFired == null || DateFired > DateTime.Now;
        }
        public virtual ICollection<Timesheet> Timesheets { get; set; } = new List<Timesheet>();
    }

}
