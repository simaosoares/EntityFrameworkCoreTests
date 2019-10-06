using System.Threading.Tasks;
using EntityFrameworkCoreTests.Data;
using EntityFrameworkCoreTests.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace EntityFrameworkCoreTests.Tests
{
    /// <summary>
    /// Persistence tests for <see cref="T:EntityFrameworkCoreTests.Data.Entities.Equipment"/> 
    /// </summary>
    public class EquipmentPersistenceTests
    {

        public class SqlLite
        {
            [Theory]
            [InlineData("Tank 200L")]
            public async Task WhenPassingCorrectData_CreateSuccessfully(string name)
            {
                using (var factory = new SqlLiteDbContextFactory())
                {
                    // Get a context
                    using (var context = factory.CreateContext())
                    {

                        context.Equipments.Add(new Equipment
                        {
                            Name = name
                        });
                    
                        await context.SaveChangesAsync();
                    }

                    // Get another context using the same connection
                    using (var context = factory.CreateContext())
                    {
                        AssertPersistedEquipment(context, name);
                    }
                }
            }
        }
        
        public class InMemoryDatabase
        {
            [Theory]
            [InlineData("Tank 300L")]
            public async Task WhenPassingCorrectData_CreateSuccessfully(string name)
            {
                var options = new DbContextOptionsBuilder<DataContext>()
                    .UseInMemoryDatabase("Equipment_XUnitTests_InMemoryDatabase")
                    .Options;

                using (var context = new DataContext(options))
                {
                    context.Equipments.Add(new Equipment
                    {
                        Name = name
                    });
                    
                    await context.SaveChangesAsync();
                }
                
                using (var context = new DataContext(options))
                {
                    AssertPersistedEquipment(context, name);
                }
            }
        }

        /// <summary>
        /// Helper method to assert a persisted <see cref="T:EntityFrameworkCoreTests.Data.Entities.Equipment"/>
        /// </summary>
        /// <param name="context">Database context</param>
        /// <param name="name">Equipment name</param>
        private static async void AssertPersistedEquipment(DataContext context, string name)
        {
            Assert.Equal(1, await context.Equipments.CountAsync());
            var equipment = await context.Equipments.SingleAsync();
            Assert.NotNull(equipment);
            Assert.True(equipment.Id > 0);
            Assert.Equal(name, equipment.Name);
        }
    }
}