using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace Restaurant_FinalProject.Data
{
    public abstract class Person
    {
        //Base class for all people in the system

        [Key]
        public int PersonID { get; set; }   
        [Required]
        [MaxLength(50)]
        public string FirstName { get; set; }
        [Required]
        [MaxLength(50)]
        public string LastName { get; set; }
        [Required]
        [MaxLength(15)]
        public string PhoneNumber { get; set; }
        public string GetFullName()
        {
            return $"{FirstName} {LastName}";
        }


    }
}
