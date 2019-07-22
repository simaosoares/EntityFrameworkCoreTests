using System.Collections.Generic;
using EntityFrameworkCoreTests.Data.Entities;

namespace EntityFrameworkCoreTests.Services
{
    public interface IEquipmentService
    {
        ICollection<Equipment> FindAll();

        Equipment FindById(int id);

        Equipment Create(Equipment equipment);

        void Update(Equipment equipment);

        void Delete(int id);
    }
}
