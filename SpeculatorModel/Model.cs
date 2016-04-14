using System;
using Microsoft.Data.Entity;

namespace SpeculatorModel
{
    public class SpeculatorContext : DbContext
    {
        public DbSet<SmartComSymbol> SmartComSymbols { get; set; }
        public DbSet<SmartComBidAskValue> SmartComBidAskValues { get; set; }
        public DbSet<SmartComTick> SmartComTicks { get; set; }
        public DbSet<SmartComQuote> SmartComQuotes { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(@"Data Source=localhost;Initial Catalog=speculator;Integrated Security=True;");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<SmartComTick>()
                .Property(tick => tick.TradeAdded)
                .HasDefaultValueSql("getdate()");

            modelBuilder.Entity<SmartComBidAskValue>()
                .Property(val => val.Added)
                .HasDefaultValueSql("getdate()");

            modelBuilder.Entity<SmartComQuote>()
                .Property(quote => quote.QuoteAdded)
                .HasDefaultValueSql("getdate()");

            //modelBuilder.Properties<DateTime>()
            //    .Configure(c =>
            //    {
            //        c.HasColumnType("datetime2");
            //        c.HasPrecision(2);
            //    });
        }
    }
}
