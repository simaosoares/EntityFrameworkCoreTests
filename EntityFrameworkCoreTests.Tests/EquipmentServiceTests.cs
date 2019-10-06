using System;
using System.Linq;
using EntityFrameworkCoreTests.Data;
using EntityFrameworkCoreTests.Data.Entities;
using EntityFrameworkCoreTests.Services;
using Xunit;
using Microsoft.EntityFrameworkCore;

namespace EntityFrameworkCoreTests.Tests
{
    /// <summary>
    /// Unit tests for <see cref="T:EntityFrameworkCoreTests.Services.EquipmentService"/>
    /// </summary>
    public class EquipmentServiceTests
    {
        public class FindAll
        {
            [Fact]
            public void WhenEquipmentExists_ReturnAll()
            {
                using (var factory = new SqlLiteDbContextFactory())
                {
                    using (var context = factory.CreateContext())
                    {
                        SetupTestData(context);
                        
                        var service = new EquipmentService(context);
                        var equipments = service.FindAll();
                        
                        Assert.Equal(2, equipments.Count);
                    }
                }
            }
            
            [Fact]
            public void WhenEquipmentDoesNotExists_ReturnNone()
            {
                using (var factory = new SqlLiteDbContextFactory())
                {
                    using (var context = factory.CreateContext())
                    {
                        var service = new EquipmentService(context);
                        var equipments = service.FindAll();
                        
                        Assert.Empty(equipments);
                    }
                }
            }
            
        }
        
        public class FindById
        {
            [Theory]
            [InlineData(1)]
            [InlineData(2)]
            public void WhenEquipmentExists_ReturnEquipment(int id)
            {
                using (var factory = new SqlLiteDbContextFactory())
                {
                    using (var context = factory.CreateContext())
                    {
                        SetupTestData(context);

                        var service = new EquipmentService(context);
                        var equipment = service.FindById(id);
                        Assert.NotNull(equipment);                        
                    }
                }
            }
            
            [Theory]
            [InlineData(11)]
            [InlineData(22)]
            public void WhenEquipmentDoesNotExist_ReturnNull(int id)
            {
                using (var factory = new SqlLiteDbContextFactory())
                {
                    using (var context = factory.CreateContext())
                    {
                        SetupTestData(context);

                        var service = new EquipmentService(context);
                        var equipment = service.FindById(id);
                        Assert.Null(equipment);
                    }
                }
            }
        }
        
        public class Create
        {
            public class UseInMemoryDatabase
            {
                [Theory]
                [InlineData("Tank 200L")]
                public void WhenPassingCorrectData_CreateSuccessfully(string name)
                {

                    var options = new DbContextOptionsBuilder<DataContext>()
                        .UseInMemoryDatabase(databaseName: "XUnitTests")
                        .Options;

                    // Run the test against one instance of the context
                    using (var context = new DataContext(options))
                    {
                        var service = new EquipmentService(context);
                        service.Create(new Equipment
                        {
                            Name = name
                        });
                    }

                    // Use a separate instance of the context to verify correct data was saved to database
                    using (var context = new DataContext(options))
                    {
                        Assert.Equal(1, context.Equipments.Count());
                        Assert.Equal(name, context.Equipments.Single().Name);
                    }
                }    
            }
            
            public class UseSqlite
            {
                [Theory]
                [InlineData("Tank 200L")]
                public void WhenPassingCorrectData_CreateSuccessfully(string name)
                {
                    using (var factory = new SqlLiteDbContextFactory())
                    {
                        // Get a context
                        using (var context = factory.CreateContext())
                        {
                            var service = new EquipmentService(context);
                            service.Create(new Equipment
                            {
                                Name = name
                            });
                        }

                        // Get another context using the same connection
                        using (var context = factory.CreateContext())
                        {
                            Assert.Equal(1, context.Equipments.Count());
                            Assert.Equal(name, (context.Equipments.Single()).Name);
                        }
                    }
                }


                [Theory]
                [InlineData(null)]
                public void WhenPassingIncorrectData_ThrowsException(string name)
                {
                    using (var factory = new SqlLiteDbContextFactory())
                    {

                        using (var context = factory.CreateContext())
                        {
                            var service = new EquipmentService(context);

                            Assert.Throws<DbUpdateException>(() => service.Create(new Equipment() 
                            {
                                Name = name
                            }));

                        }
                    }
                }
                
            }
            
        }

        public class Update
        {
            [Theory]
            [InlineData(1, "Scania R500")]
            [InlineData(2, "Volvo FH17")]
            public void WhenEquipmentExists_ShouldUpdateSuccessfully(int id, string name)
            {
                using (var factory = new SqlLiteDbContextFactory())
                {
                    using (var context = factory.CreateContext())
                    {
                        SetupTestData(context);
                        
                        var service = new EquipmentService(context);

                        var equipment = new Equipment()
                        {
                            Id = id,
                            Name = name
                        };
                        service.Update(equipment);

                        var updatedEquipment = service.FindById(id);
                        
                        Assert.NotNull(updatedEquipment);
                        Assert.Equal(updatedEquipment.Id, id);
                        Assert.Equal(updatedEquipment.Name, name);
                    }
                }
            }
            
            [Theory]
            [InlineData(11, "Scania R500")]
            [InlineData(22, "Volvo FH17")]
            public void WhenEquipmentDoesNotExist_ShouldThrowException(int id, string name)
            {
                using (var factory = new SqlLiteDbContextFactory())
                {
                    using (var context = factory.CreateContext())
                    {
                        SetupTestData(context);
                        
                        var service = new EquipmentService(context);

                        var equipment = new Equipment()
                        {
                            Id = id,
                            Name = name
                        };

                        Assert.Throws<Exception>(() => service.Update(equipment));
                    }
                }
            }
        }
        
        public class Delete
        {
            [Theory]
            [InlineData(1)]
            [InlineData(2)]
            public void WhenEquipmentExists_ShouldDeleteEquipment(int id)
            {
                using (var factory = new SqlLiteDbContextFactory() )
                {
                    using (var context = factory.CreateContext())
                    {
                        SetupTestData(context);

                        var service = new EquipmentService(context);
                        service.Delete(id);

                        var deletedEquipment = service.FindById(id);
                        Assert.Null(deletedEquipment);
                    }
                }
            }
            
            [Theory]
            [InlineData(11)]
            [InlineData(22)]
            public void WhenEquipmentNotExists_ShouldThrowException(int id)
            {
                using (var factory = new SqlLiteDbContextFactory() )
                {
                    using (var context = factory.CreateContext())
                    {
                        var service = new EquipmentService(context);
                        Assert.Throws<Exception>(() => service.Delete(id));
                    }
                }
            }
        }
        
        /// <summary>
        /// Setup some testing data for <see cref="T:EntityFrameworkCoreTests.Services.EquipmentService"/> unit tests  
        /// </summary>
        /// <param name="context">database context</param>
        private static void SetupTestData(DataContext context)
        {
            var equipmentS = new[]
            {
                new Equipment() { Id = 1, Name = "Scania R730" }, 
                new Equipment() { Id = 2, Name = "Volvo FH16" }
            };

            context.Equipments.AddRange(equipmentS);
            context.SaveChanges();
        }
    }
}
