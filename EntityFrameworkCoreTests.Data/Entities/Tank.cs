using System.ComponentModel.DataAnnotations.Schema;

namespace EntityFrameworkCoreTests.Data.Entities
{
    public class Tank
    {
        // PK and FK pointing to Equipment.
        // Simulates a Table-Per-Type (TPT) inheritance which is not yet available on EFCore 
        // TODO: remove annotation below and use the fluent api
        [ForeignKey(nameof(Equipment))]
        public int Id { get; set; }
        
        public Equipment Equipment { get; set; }
        
        public int Volume { get; set; }
    }
}