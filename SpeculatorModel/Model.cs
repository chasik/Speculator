﻿using Microsoft.EntityFrameworkCore;
using SpeculatorModel.MainData;
using SpeculatorModel.MoexHistory;
using SpeculatorModel.SmartCom;
using SpeculatorModel.Transaq;

namespace SpeculatorModel
{
    public class SpeculatorContext : DbContext
    {
        public DbSet<DataSource> DataSources { get; set; }

        public DbSet<SmartComSymbol> SmartComSymbols { get; set; }
        public DbSet<SmartComBidAskValue> SmartComBidAskValues { get; set; }
        public DbSet<SmartComTrade> SmartComTicks { get; set; }
        public DbSet<SmartComQuote> SmartComQuotes { get; set; }

        public DbSet<Market> Markets { get; set; }
        public DbSet<Board> Boards { get; set; }
        public DbSet<Security> Securities { get; set; }
        public DbSet<CandleKind> CandleKinds { get; set; }
        public DbSet<ClientOrder> ClientOrders { get; set; }
        public DbSet<ClientStopOrder> ClientSotpOrders { get; set; }
        public DbSet<ClientNegDeal> ClientNegDeals { get; set; }
        public DbSet<ClientTrade> ClientTrades { get; set; }
        public DbSet<TransaqTrade> TransaqTrades { get; set; }
        public DbSet<Tick> Ticks { get; set; }
        public DbSet<Quote> Quotes { get; set; }

        public DbSet<MoexSystem> MoexSystems { get; set; }
        public DbSet<MoexSymbol> MoexSymbols { get; set; }
        public DbSet<MoexClaimType> MoexClaimTypes { get; set; }
        public DbSet<ClaimAction> MoexClaimActions { get; set; }
        public DbSet<Diraction> MoexTradeDiractions { get; set; }
        public DbSet<MoexTrade> MoexTrades { get; set; }
        public DbSet<MoexClaim> MoexClaims { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(@"Data Source=localhost;Initial Catalog=speculator;Integrated Security=True;");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<SmartComTrade>()
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
