using System;
using Microsoft.Data.Entity;
using SpeculatorModel.Transaq;

namespace SpeculatorModel
{
    public class SpeculatorContext : DbContext
    {
        public DbSet<SmartComSymbol> SmartComSymbols { get; set; }
        public DbSet<SmartComBidAskValue> SmartComBidAskValues { get; set; }
        public DbSet<SmartComTick> SmartComTicks { get; set; }
        public DbSet<SmartComQuote> SmartComQuotes { get; set; }

        public DbSet<Market> Markets { get; set; }
        public DbSet<Board> Boards { get; set; }
        public DbSet<Security> Securities { get; set; }
        public DbSet<CandleKind> CandleKinds { get; set; }
        public DbSet<ClientOrder> ClientOrders { get; set; }
        public DbSet<ClientStopOrder> ClientSotpOrders { get; set; }
        public DbSet<ClientNegDeal> ClientNegDeals { get; set; }
        public DbSet<ClientTrade> ClientTrades { get; set; }
        public DbSet<Trade> Trades { get; set; }
        public DbSet<Tick> Ticks { get; set; }
        public DbSet<Quote> Quotes { get; set; }

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

            #region Индексы

            modelBuilder.Entity<Board>()
                .HasIndex(b => b.InnerId)
                .IsUnique();

            #endregion

            //modelBuilder.Properties<DateTime>()
            //    .Configure(c =>
            //    {
            //        c.HasColumnType("datetime2");
            //        c.HasPrecision(2);
            //    });
        }
    }
}
