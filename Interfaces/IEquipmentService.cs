using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using cpsy200FinalProject.Data;

namespace cpsy200FinalProject.Interfaces
{
    public interface IEquipmentService
    {
        List<Equipment> GetAllEquipment();
        Equipment? GetEquipmentById(int equipmentId);
        void AddEquipment(Equipment equipment);
        void UpdateEquipment(Equipment equipment);
        void DeleteEquipment(int equipmentId);
        List<Equipment> GetAvailableEquipment();
    }
}
