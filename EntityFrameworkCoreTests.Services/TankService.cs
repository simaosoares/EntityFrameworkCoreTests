using System;
using System.Collections.Generic;
using System.Linq;
using EntityFrameworkCoreTests.Data;
using EntityFrameworkCoreTests.Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace EntityFrameworkCoreTests.Services
{
    public class TankService : ITankService
    {
        private DataContext _context;

        public TankService(DataContext context)
        {
            this._context = context;
        }

        public ICollection<Tank> FindAll()
        {
            return this._context.Tanks.ToList();
        }

        public Tank FindById(int id)
        {
            return _context.Tanks.Find(id);
        }

        public Tank Create(Tank tank)
        {
            this._context.Tanks.Add(tank);
            this._context.SaveChanges();
            return tank;
        }

        public void Update(Tank tank)
        {
            var tankToBeUpdated = _context.Tanks.Find(tank.Id);
            
            if (tankToBeUpdated == null) 
            {
                throw new Exception($"Record id {tank.Id} not found.");
            }

            tankToBeUpdated.Volume = tank.Volume;
            tankToBeUpdated.Equipment.Name = tank.Equipment.Name;
            _context.SaveChanges();
        }

        public void Delete(int id)
        {
            var tank = _context.Tanks.Find(id);
            
            if (tank == null) 
            {
                throw new Exception($"Record id {id} not found.");
            }

            _context.Tanks.Remove(tank);
            _context.SaveChanges();
        }
    }
}