using EntityFrameworkCoreTests.Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace EntityFrameworkCoreTests.Data
{
    /// <summary>
    /// Database context class. Main class that coordinates Entity Framework functionality for a given data model. 
    /// 
    /// This code creates a DbSet property for each entity set. In Entity Framework terminology, an entity set 
    /// typically corresponds to a database table, and an entity corresponds to a row in the table.
    ///
    /// By default, the Entity Framework interprets a property that's named ID or classnameID as the primary key.
    /// 
    /// </summary>
    public class DataContext : DbContext
    {
        public DbSet<Equipment> Equipments { get; set; }
        public DbSet<Tank> Tanks { get; set; }
      
        public DataContext(DbContextOptions<DataContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Equipment>()
                .Property(equipment => equipment.Name)
                .IsRequired();

            modelBuilder.Entity<Tank>()
                .Property(tank => tank.Volume)
                .IsRequired();

            // TODO: remove `[ForeignKey(nameof(Equipment))]` annotation and use only the fluent api
            // https://github.com/aspnet/EntityFrameworkCore/issues/14158
            // modelBuilder.Entity<Tank>()
            //   .HasForeignKey(....)
        }

    }
}