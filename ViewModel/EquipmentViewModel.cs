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
    public class EquipmentViewModel : BaseViewModel

    {
        private readonly IEquipmentService _equipmentService;

        // UI-Bindable Properties
        public ObservableCollection<Equipment> EquipmentList { get; } = new();
        public string SearchTerm { get; set; }

        // Commands (bound to UI buttons)
        public ICommand LoadEquipmentCommand { get; }
        public ICommand AddEquipmentCommand { get; }
        public ICommand DeleteEquipmentCommand { get; }
        public ICommand SearchEquipmentCommand { get; }

        public EquipmentViewModel(IEquipmentService equipmentService)
        {
            _equipmentService = equipmentService;

            // Initialize commands
            LoadEquipmentCommand = new Command(LoadEquipment);
            AddEquipmentCommand = new Command<Equipment>(AddEquipment);
            DeleteEquipmentCommand = new Command<int>(DeleteEquipment);
            SearchEquipmentCommand = new Command(SearchEquipment);

            LoadEquipment(); // Initial data load
        }

        private void LoadEquipment()
        {
            EquipmentList.Clear();
            var allEquipment = _equipmentService.GetAllEquipment();
            foreach (var item in allEquipment)
            {
                EquipmentList.Add(item);
            }
        }

        private void AddEquipment(Equipment equipment)
        {
            _equipmentService.AddEquipment(equipment);
            LoadEquipment(); // Refresh list
        }

        private void DeleteEquipment(int equipmentId)
        {
            _equipmentService.DeleteEquipment(equipmentId);
            LoadEquipment();
        }

        private void SearchEquipment()
        {
            if (string.IsNullOrWhiteSpace(SearchTerm))
            {
                LoadEquipment();
                return;
            }

            EquipmentList.Clear();
            var filtered = _equipmentService.GetAllEquipment()
                .Where(e => e.Name.Contains(SearchTerm, StringComparison.OrdinalIgnoreCase));

            foreach (var item in filtered)
            {
                EquipmentList.Add(item);
            }
        }




    }

    
}
