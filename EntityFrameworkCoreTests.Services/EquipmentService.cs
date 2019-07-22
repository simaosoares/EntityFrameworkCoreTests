using System;
using System.Collections.Generic;
using System.Linq;
using EntityFrameworkCoreTests.Data;
using EntityFrameworkCoreTests.Data.Entities;

namespace EntityFrameworkCoreTests.Services
{
    public class EquipmentService : IEquipmentService
    {
        private DataContext _dataContext;

        public EquipmentService(DataContext dataContext)
        {
            _dataContext = dataContext;
        }

        public ICollection<Equipment> FindAll()
        {
            return _dataContext.Equipments.ToList();
        }
        
        public Equipment FindById(int id)
        {
            return _dataContext.Equipments.Find(id);
        }

        public Equipment Create(Equipment equipment)
        {
            _dataContext.Equipments.Add(equipment);
            _dataContext.SaveChanges();
            return equipment;
        }
        
        public void Update(Equipment equipment)
        {
            var existingEquipment = _dataContext.Equipments.Find(equipment.Id);
            if (existingEquipment == null)
            {
                throw new Exception($"Record id {equipment.Id} not found.");
            }

            existingEquipment.Name = equipment.Name;
            
            _dataContext.Equipments.Update(existingEquipment);
            _dataContext.SaveChanges();
        }

        public void Delete(int id)
        {
            var equipment = _dataContext.Equipments.Find(id);
            if (equipment == null)
            {
                throw new Exception($"Record id {id} not found.");
            }

            _dataContext.Equipments.Remove(equipment);
            _dataContext.SaveChanges();
        }
        
    }
}
