using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using cpsy200FinalProject.Data;
using cpsy200FinalProject.Interfaces;

namespace cpsy200FinalProject.ViewModel
{
    public class RentalViewModel : BaseViewModel
    {
        private readonly IRentalService _rentalService;
        private readonly IEquipmentService _equipmentService;
        private readonly ICustomerService _customerService;

        // Bindable Properties
        public ObservableCollection<Customer> Customers { get; } = new();
        public ObservableCollection<Equipment> AvailableEquipment { get; } = new();
        public Customer SelectedCustomer { get; set; }
        public Equipment SelectedEquipment { get; set; }
        public DateTime StartDate { get; set; } = DateTime.Today;
        public DateTime ReturnDate { get; set; } = DateTime.Today.AddDays(1);
        public decimal EstimatedCost { get; private set; }

        // Commands
        public ICommand LoadDataCommand { get; }
        public ICommand CalculateCostCommand { get; }
        public ICommand ProcessRentalCommand { get; }

        public RentalViewModel(
            IRentalService rentalService,
            IEquipmentService equipmentService,
            ICustomerService customerService)
        {
            _rentalService = rentalService;
            _equipmentService = equipmentService;
            _customerService = customerService;

            LoadDataCommand = new Command(LoadData);
            CalculateCostCommand = new Command(CalculateCost);
            ProcessRentalCommand = new Command(ProcessRental);

            LoadData();
        }

        private void LoadData()
        {
            // Load customers
            Customers.Clear();
            foreach (var customer in _customerService.GetAllCustomers().Where(c => !c.IsBanned))
            {
                Customers.Add(customer);
            }

            // Load available equipment
            AvailableEquipment.Clear();
            foreach (var equipment in _equipmentService.GetAvailableEquipment())
            {
                AvailableEquipment.Add(equipment);
            }
        }

        
        private void CalculateCost()
        {
            if (SelectedEquipment == null) return;

            EstimatedCost = _rentalService.CalculateRentalCost(
                SelectedEquipment.EquipmentId,
                    StartDate,
                    ReturnDate,
                    SelectedEquipment);

                OnPropertyChanged(nameof(EstimatedCost));
            }

            private void ProcessRental()
            {
                if (SelectedCustomer == null || SelectedEquipment == null) return;

                var rental = new RentalItem
                {
                    Equipment = SelectedEquipment.EquipmentId,
                    RentalDate = StartDate,
                    ReturnDate = ReturnDate,
                    Cost = (double)EstimatedCost
            };

            _rentalService.ProcessRental(rental, SelectedCustomer);
            LoadData(); // Refresh available equipment
        }
    }
}
