using Microsoft.Data.Entity;

namespace SpeculatorModel
{
    public class SpeculatorContext : DbContext
    {
        public DbSet<SmartComSymbol> SmartComSymbols { get; set; }
        public DbSet<SmartComBidAskValue> SmartComBidAskValues { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(@"Data Source=localhost;Initial Catalog=speculator;Integrated Security=True;");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

        }
    }
}
