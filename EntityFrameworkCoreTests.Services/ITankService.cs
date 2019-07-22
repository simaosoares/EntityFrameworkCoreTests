using System.Collections.Generic;
using EntityFrameworkCoreTests.Data.Entities;

namespace EntityFrameworkCoreTests.Services
{
    public interface ITankService
    {
        ICollection<Tank> FindAll();

        Tank FindById(int id);

        Tank Create(Tank tank);

        void Update(Tank tank);

        void Delete(int id);

    }
}