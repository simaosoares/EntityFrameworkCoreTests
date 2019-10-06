using System;
using System.Linq;
using EntityFrameworkCoreTests.Data;
using EntityFrameworkCoreTests.Data.Entities;
using EntityFrameworkCoreTests.Services;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace EntityFrameworkCoreTests.Tests
{
    /// <summary>
    /// Unit tests for <see cref="T:EntityFrameworkCoreTests.Services.TankService"/>
    /// </summary>
    public class TankServiceTests
    {
        
        public class FindAll
        {
            [Fact]
            public void WhenTankExists_ReturnAll()
            {
                using (var factory = new SqlLiteDbContextFactory())
                {
                    using (var context = factory.CreateContext())
                    {
                        SetupTestData(context);
                        
                        var service = new TankService(context);
                        var tanks = service.FindAll();
                        
                        Assert.Equal(2, tanks.Count);
                    }
                }
            }
            
            [Fact]
            public void WhenTankDoesNotExists_ReturnNone()
            {
                using (var factory = new SqlLiteDbContextFactory())
                {
                    using (var context = factory.CreateContext())
                    {
                        var service = new TankService(context);
                        var tanks = service.FindAll();
                        
                        Assert.Empty(tanks);
                    }
                }
            }
        }
        
        public class FindById
        {
            [Theory]
            [InlineData(1, "TKN 10000", 10000)]
            [InlineData(2, "TKN 20000", 20000)]
            public void WhenTankExists_ReturnTank(int id, string name, int volume)
            {
                using (var factory = new SqlLiteDbContextFactory())
                {
                    using (var context = factory.CreateContext())
                    {
                        SetupTestData(context);

                        var service = new TankService(context);
                        var tank = service.FindById(id);

                        AssertTank(tank, id, name, volume);
                    }
                }
            }

            [Theory]
            [InlineData(11)]
            [InlineData(22)]
            public void WhenTankDoesNotExist_ReturnNull(int id)
            {
                using (var factory = new SqlLiteDbContextFactory())
                {
                    using (var context = factory.CreateContext())
                    {
                        SetupTestData(context);

                        var service = new TankService(context);
                        var tank = service.FindById(id);
                        Assert.Null(tank);
                    }
                }
            }
        }
        
        public class Create
        {

            [Theory]
            [InlineData("TKN 1500", 10000)]
            public void WhenPassingCorrectData_CreateSuccessfully(string name, int volume)
            {
                using (var dbContextFactory = new SqlLiteDbContextFactory())
                {
                    // Run the test against the instance of the context
                    using (var context = dbContextFactory.CreateContext())
                    {
                        var service = new TankService(context);
                        var tank = InstantiateTank(name, volume);
                        service.Create(tank);
                    }

                    // Get another context using the same connection
                    using (var context = dbContextFactory.CreateContext())
                    {
                        var service = new TankService(context);
                        var tanks = service.FindAll();
                        Assert.Equal(1, tanks.Count);
                        var tank = tanks.First();
                        Assert.NotNull(tank);
                        // TODO: uncomment below after fix and investigate why the navigation property 'Equipment' is not eager loaded
                        // AssertTank(tank, null, name, volume);
                    }    
                }
            }
        }
        
        public class Update
        {
            [Theory]
            [InlineData(1, "TKN 10100", 10100)]
            [InlineData(2, "TKN 20200", 20200)]
            public void WhenTankExists_ShouldUpdateSuccessfully(int id, string name, int volume)
            {
                using (var factory = new SqlLiteDbContextFactory())
                {
                    using (var context = factory.CreateContext())
                    {
                        SetupTestData(context);
                        
                        var service = new TankService(context);

                        var tank = InstantiateTank(id, name, volume);
                        service.Update(tank);

                        var updatedTank = service.FindById(id);
                        
                        AssertTank(updatedTank, id, name, volume);
                    }
                }
            }

            [Theory]
            [InlineData(11, "TKN 10100", 10100)]
            [InlineData(22, "TKN 20200", 20200)]
            public void WhenTankDoesNotExist_ShouldThrowException(int id, string name, int volume)
            {
                using (var factory = new SqlLiteDbContextFactory())
                {    
                    using (var context = factory.CreateContext())
                    {
                        SetupTestData(context);
                        
                        var service = new TankService(context);
                        var tank = InstantiateTank(id, name, volume);

                        Assert.Throws<Exception>(() => service.Update(tank));
                    }
                }
            }
        }
        
        public class Delete
        {
            [Theory]
            [InlineData(1)]
            [InlineData(2)]
            public void WhenTankExists_ShouldDeleteTank(int id)
            {
                using (var factory = new SqlLiteDbContextFactory() )
                {
                    using (var context = factory.CreateContext())
                    {
                        SetupTestData(context);

                        var service = new TankService(context);
                        service.Delete(id);

                        var tank = service.FindById(id);
                        Assert.Null(tank);
                    }
                }
            }
            
            [Theory]
            [InlineData(11)]
            [InlineData(22)]
            public void WhenTankNotExists_ShouldThrowException(int id)
            {
                using (var factory = new SqlLiteDbContextFactory() )
                {
                    using (var context = factory.CreateContext())
                    {
                        var service = new TankService(context);
                        Assert.Throws<Exception>(() => service.Delete(id));
                    }
                }
            }
        }
        
        
        private static Tank InstantiateTank(int id, string name, int volume)
        {
            var tank = new Tank()
            {
                Id = id,
                Volume = volume,
                Equipment = new Equipment()
                {
                    Name = name
                },
            };
            return tank;
        }
        
        private static Tank InstantiateTank(string name, int volume)
        {
            var tank = new Tank()
            {
                Volume = volume,
                Equipment = new Equipment()
                {
                    Name = name
                },
            };
            return tank;
        }

        /// <summary>
        /// Persists sample data for unit testing
        /// </summary>
        /// <param name="context">Database context</param>
        private static void SetupTestData(DataContext context)
        {
            var tanks = new[]
            {
                new Tank() { 
                    Id = 1, 
                    Volume = 10000, 
                    Equipment = new Equipment() {
                        Id  = 1, 
                        Name = "TKN 10000"
                    }}, 
                
                new Tank() { 
                    Id = 2, 
                    Volume = 20000, 
                    Equipment = new Equipment() {
                        Id  = 2, 
                        Name = "TKN 20000"
                    }}
            };

            context.Tanks.AddRange(tanks);
            context.SaveChanges();
        }
        
        private static void AssertTank(Tank tank, int? id, string name, int volume)
        {
            Assert.NotNull(tank);
            Assert.NotNull(tank.Equipment);

            if (id == null)
            {
                Assert.True(tank.Id > 0);
                Assert.Equal(tank.Id, tank.Equipment.Id);                    
            }
            else
            {
                Assert.Equal(id, tank.Id);
                Assert.Equal(id, tank.Equipment.Id);
            }
                

            Assert.Equal(name, tank.Equipment.Name);
            Assert.Equal(volume, tank.Volume);
        }
        
    }
}