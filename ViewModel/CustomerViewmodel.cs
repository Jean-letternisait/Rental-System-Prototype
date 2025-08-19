using cpsy200FinalProject.Data;
using cpsy200FinalProject.Interfaces;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace cpsy200FinalProject.ViewModel
{
    public class CustomerViewModel : BaseViewModel
    {
        private readonly ICustomerService _customerService;

        public ObservableCollection<Customer> Customers { get; } = new();
        public Customer SelectedCustomer { get; set; }

        public ICommand LoadCustomersCommand { get; }
        public ICommand AddCustomerCommand { get; }
        public ICommand ToggleBanCommand { get; }

        public CustomerViewModel(ICustomerService customerService)
        {
            _customerService = customerService;

            LoadCustomersCommand = new Command(LoadCustomers);
            AddCustomerCommand = new Command<Customer>(AddCustomer);
            ToggleBanCommand = new Command<int>(ToggleBanStatus);

            LoadCustomers();
        }

        private void LoadCustomers()
        {
            Customers.Clear();
            foreach (var customer in _customerService.GetAllCustomers())
            {
                Customers.Add(customer);
            }
        }

        private void AddCustomer(Customer customer)
        {
            _customerService.AddCustomer(customer);
            Customers.Add(customer);
        }

        private void ToggleBanStatus(int customerId)
        {
            var customer = _customerService.GetCustomerById(customerId);
            if (customer != null)
            {
                customer.IsBanned = !customer.IsBanned;
                _customerService.UpdateCustomer(customer);
            }
        }

    }
}
