using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace cpsy200FinalProject.Data
{
    public class Customer
    {
        private int customerId;
        private string lastName;
        private string firstName;
        private string contactPhone;
        private string emailAddress;
        private bool isBanned;
        private double discountRate;

        public int CustomerId { get => customerId; set => customerId = value; }
        public string LastName { get => lastName; set => lastName = value; }
        public string FirstName { get => firstName; set => firstName = value; }
        public string ContactPhone { get => contactPhone; set => contactPhone = value; }
        public string EmailAddress { get => emailAddress; set => emailAddress = value; }
        public bool IsBanned { get => isBanned; set => isBanned = value; }
        public double DiscountRate { get => discountRate; set => discountRate = value; }

        public Customer()
        {
             
        }

        public Customer(int customerId, string lastName, string firstName, string contactPhone, string emailAddress,
            bool isBanned, double discountRate)
        {
            this.customerId = customerId;
            this.lastName = lastName;
            this.firstName = firstName;
            this.contactPhone = contactPhone;
            this.emailAddress = emailAddress;
            this.isBanned = isBanned;
            this.discountRate = discountRate;
        }

    }
}
