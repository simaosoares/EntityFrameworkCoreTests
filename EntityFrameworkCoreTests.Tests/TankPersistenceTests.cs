using System.Threading.Tasks;
using EntityFrameworkCoreTests.Data;
using EntityFrameworkCoreTests.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace EntityFrameworkCoreTests.Tests
{
    /// <summary>
    /// Persistence tests for <see cref="T:EntityFrameworkCoreTests.Data.Entities.Tank"/> 
    /// </summary>
    public class TankPersistenceTests
    {
        public class SqlLite
        {
            [Theory]
            [InlineData("TKN 1500", 10000)]
            public async Task WhenPassingCorrectData_CreateSuccessfully(string name, int volume)
            {
                using (var factory = new SqlLiteDbContextFactory())
                {
                    // Get a context
                    using (var context = factory.CreateContext())
                    {

                        context.Tanks.Add(new Tank()
                        {
                            Volume = volume,
                            Equipment = new Equipment
                            {
                                Name = name
                            }
                        });
                    
                        await context.SaveChangesAsync();
                    }

                    // Get another context using the same connection
                    using (var context = factory.CreateContext())
                    {
                        AssertPersistedTank(context, name, volume);
                    }
                }
            }
        }
        
        public class InMemoryDatabase
        {

            [Theory]
            [InlineData("Tank 300L", 10000)]
            public async Task WhenPassingCorrectData_CreateSuccessfully(string name, int volume)
            {
                var options = new DbContextOptionsBuilder<DataContext>()
                    .UseInMemoryDatabase("Tank_XUnitTests_InMemoryDatabase")
                    .Options;

                using (var context = new DataContext(options))
                {
                    context.Tanks.Add(new Tank()
                    {
                        Volume = volume,
                        Equipment = new Equipment
                        {
                            Name = name
                        }
                    });
                    
                    await context.SaveChangesAsync();
                }
                
                using (var context = new DataContext(options))
                {
                    AssertPersistedTank(context, name, volume);
                }
            }
        }

        /// <summary>
        /// Helper method to assert a persisted <see cref="T:EntityFrameworkCoreTests.Data.Entities.Tank"/>
        /// </summary>
        /// <param name="context">Database context</param>
        /// <param name="name">Tank name</param>
        /// <param name="volume">Tank volume</param>
        private static async void AssertPersistedTank(DataContext context, string name, int volume)
        {
            Assert.Equal(1, await context.Tanks.CountAsync());
            Assert.Equal(1, await context.Equipments.CountAsync());
            
            var tank = await context.Tanks
                .Include(t => t.Equipment)
                .SingleAsync();
            
            Assert.True(tank.Id > 0);
            Assert.True(tank.Id == tank.Equipment.Id);
            Assert.Equal(volume, tank.Volume);
            Assert.Equal(name, tank.Equipment.Name);
        }
    }
}