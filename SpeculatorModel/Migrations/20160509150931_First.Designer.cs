using System;
using Microsoft.Data.Entity;
using Microsoft.Data.Entity.Infrastructure;
using Microsoft.Data.Entity.Metadata;
using Microsoft.Data.Entity.Migrations;
using SpeculatorModel;

namespace SpeculatorModel.Migrations
{
    [DbContext(typeof(SpeculatorContext))]
    [Migration("20160509150931_First")]
    partial class First
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.0-rc1-16348")
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("SpeculatorModel.MoexHistory.MoexClaim", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<byte>("MoexSystemId");

                    b.Property<DateTime>("Moment");

                    b.HasKey("Id");

                    b.HasAnnotation("Relational:TableName", "MoexClaims");
                });

            modelBuilder.Entity("SpeculatorModel.MoexHistory.MoexClaimAction", b =>
                {
                    b.Property<byte>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Name")
                        .HasAnnotation("MaxLength", 20);

                    b.HasKey("Id");

                    b.HasAnnotation("Relational:TableName", "MoexClaimActions");
                });

            modelBuilder.Entity("SpeculatorModel.MoexHistory.MoexClaimType", b =>
                {
                    b.Property<byte>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Name")
                        .HasAnnotation("MaxLength", 20);

                    b.HasKey("Id");

                    b.HasAnnotation("Relational:TableName", "MoexClaimTypes");
                });

            modelBuilder.Entity("SpeculatorModel.MoexHistory.MoexSymbol", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Name")
                        .HasAnnotation("MaxLength", 50);

                    b.HasKey("Id");

                    b.HasAnnotation("Relational:TableName", "MoexSymbols");
                });

            modelBuilder.Entity("SpeculatorModel.MoexHistory.MoexSystem", b =>
                {
                    b.Property<byte>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Name")
                        .HasAnnotation("MaxLength", 50);

                    b.HasKey("Id");

                    b.HasAnnotation("Relational:TableName", "MoexSystems");
                });

            modelBuilder.Entity("SpeculatorModel.MoexHistory.MoexTrade", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<byte>("MoexSystemId");

                    b.Property<DateTime>("Moment");

                    b.HasKey("Id");

                    b.HasAnnotation("Relational:TableName", "MoexTrades");
                });

            modelBuilder.Entity("SpeculatorModel.MoexHistory.MoexTradeDiraction", b =>
                {
                    b.Property<byte>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Name")
                        .HasAnnotation("MaxLength", 20);

                    b.HasKey("Id");

                    b.HasAnnotation("Relational:TableName", "MoexTradeDiractions");
                });

            modelBuilder.Entity("SpeculatorModel.SmartComBidAskValue", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<DateTime>("Added")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("Relational:ColumnType", "datetime2")
                        .HasAnnotation("Relational:GeneratedValueSql", "getdate()");

                    b.Property<bool>("IsBid");

                    b.Property<double>("Price");

                    b.Property<byte>("RowId");

                    b.Property<int>("SmartComSymbolId");

                    b.Property<int>("Volume");

                    b.HasKey("Id");

                    b.HasAnnotation("Relational:TableName", "SmartComBidAskValues");
                });

            modelBuilder.Entity("SpeculatorModel.SmartComQuote", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<double>("Ask");

                    b.Property<int>("AskSize");

                    b.Property<double>("Bid");

                    b.Property<int>("BidSize");

                    b.Property<DateTime>("LastTradeDateTime")
                        .HasAnnotation("Relational:ColumnType", "datetime2");

                    b.Property<double>("LastTradePrice");

                    b.Property<int>("LastTradeVolume");

                    b.Property<int>("OpenInterest");

                    b.Property<DateTime>("QuoteAdded")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("Relational:ColumnType", "datetime2")
                        .HasAnnotation("Relational:GeneratedValueSql", "getdate()");

                    b.Property<int>("SmartComSymbolId");

                    b.Property<double>("Volatility");

                    b.HasKey("Id");

                    b.HasAnnotation("Relational:TableName", "SmartComQuotes");
                });

            modelBuilder.Entity("SpeculatorModel.SmartComSymbol", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("Decimals");

                    b.Property<DateTime>("ExpiryDate");

                    b.Property<string>("LongName");

                    b.Property<int>("LotSize");

                    b.Property<double?>("Punkt");

                    b.Property<string>("SecExchName");

                    b.Property<string>("SecExtId");

                    b.Property<string>("ShortName");

                    b.Property<double?>("Step");

                    b.Property<double?>("Strike");

                    b.Property<string>("Symbol");

                    b.Property<string>("Type");

                    b.HasKey("Id");

                    b.HasAnnotation("Relational:TableName", "SmartComSymbols");
                });

            modelBuilder.Entity("SpeculatorModel.SmartComTick", b =>
                {
                    b.Property<long>("TradeNo");

                    b.Property<byte>("OrderAction");

                    b.Property<double>("Price");

                    b.Property<int>("SmartComSymbolId");

                    b.Property<DateTime>("TradeAdded")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("Relational:ColumnType", "datetime2")
                        .HasAnnotation("Relational:GeneratedValueSql", "getdate()");

                    b.Property<DateTime>("TradeDateTime")
                        .HasAnnotation("Relational:ColumnType", "datetime2");

                    b.Property<int>("Volume");

                    b.HasKey("TradeNo");

                    b.HasAnnotation("Relational:TableName", "SmartComTicks");
                });

            modelBuilder.Entity("SpeculatorModel.Transaq.Board", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("InnerId")
                        .HasAnnotation("MaxLength", 200);

                    b.Property<int?>("MarketId");

                    b.Property<string>("Name");

                    b.Property<byte>("TypeId");

                    b.HasKey("Id");

                    b.HasIndex("InnerId")
                        .IsUnique();

                    b.HasAnnotation("Relational:TableName", "TransaqBoards");
                });

            modelBuilder.Entity("SpeculatorModel.Transaq.CandleKind", b =>
                {
                    b.Property<int>("Id");

                    b.Property<int>("Period");

                    b.Property<string>("PeriodName")
                        .HasAnnotation("Relational:ColumnType", "nvarchar(20)");

                    b.HasKey("Id");

                    b.HasAnnotation("Relational:TableName", "TransaqCandleKinds");
                });

            modelBuilder.Entity("SpeculatorModel.Transaq.ClientNegDeal", b =>
                {
                    b.Property<long>("TransactionId");

                    b.HasKey("TransactionId");

                    b.HasAnnotation("Relational:TableName", "TransaqClientNegDeals");
                });

            modelBuilder.Entity("SpeculatorModel.Transaq.ClientOrder", b =>
                {
                    b.Property<long>("TransactionId");

                    b.HasKey("TransactionId");

                    b.HasAnnotation("Relational:TableName", "TransaqClientOrders");
                });

            modelBuilder.Entity("SpeculatorModel.Transaq.ClientStopOrder", b =>
                {
                    b.Property<long>("TransactionId");

                    b.HasKey("TransactionId");

                    b.HasAnnotation("Relational:TableName", "TransaqClientStopOrders");
                });

            modelBuilder.Entity("SpeculatorModel.Transaq.ClientTrade", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.HasKey("Id");

                    b.HasAnnotation("Relational:TableName", "TransaqClientTrades");
                });

            modelBuilder.Entity("SpeculatorModel.Transaq.Market", b =>
                {
                    b.Property<int>("Id");

                    b.Property<string>("Name")
                        .HasAnnotation("MaxLength", 30);

                    b.HasKey("Id");

                    b.HasAnnotation("Relational:TableName", "TransaqMarkets");
                });

            modelBuilder.Entity("SpeculatorModel.Transaq.Quote", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.HasKey("Id");

                    b.HasAnnotation("Relational:TableName", "TransaqQuotes");
                });

            modelBuilder.Entity("SpeculatorModel.Transaq.Security", b =>
                {
                    b.Property<int>("Id");

                    b.Property<bool>("Active");

                    b.Property<int>("BoardId");

                    b.Property<byte>("Decimals");

                    b.Property<int>("LotSize");

                    b.Property<int>("MarketId");

                    b.Property<double>("MinStep");

                    b.Property<double>("PointCost");

                    b.Property<string>("SecCode");

                    b.Property<string>("ShortName");

                    b.HasKey("Id");

                    b.HasAnnotation("Relational:TableName", "TransaqSecurities");
                });

            modelBuilder.Entity("SpeculatorModel.Transaq.Tick", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.HasKey("Id");

                    b.HasAnnotation("Relational:TableName", "TransaqTicks");
                });

            modelBuilder.Entity("SpeculatorModel.Transaq.Trade", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.HasKey("Id");

                    b.HasAnnotation("Relational:TableName", "TransaqTrades");
                });

            modelBuilder.Entity("SpeculatorModel.MoexHistory.MoexClaim", b =>
                {
                    b.HasOne("SpeculatorModel.MoexHistory.MoexSystem")
                        .WithMany()
                        .HasForeignKey("MoexSystemId");
                });

            modelBuilder.Entity("SpeculatorModel.MoexHistory.MoexTrade", b =>
                {
                    b.HasOne("SpeculatorModel.MoexHistory.MoexSystem")
                        .WithMany()
                        .HasForeignKey("MoexSystemId");
                });

            modelBuilder.Entity("SpeculatorModel.SmartComBidAskValue", b =>
                {
                    b.HasOne("SpeculatorModel.SmartComSymbol")
                        .WithMany()
                        .HasForeignKey("SmartComSymbolId");
                });

            modelBuilder.Entity("SpeculatorModel.SmartComQuote", b =>
                {
                    b.HasOne("SpeculatorModel.SmartComSymbol")
                        .WithMany()
                        .HasForeignKey("SmartComSymbolId");
                });

            modelBuilder.Entity("SpeculatorModel.SmartComTick", b =>
                {
                    b.HasOne("SpeculatorModel.SmartComSymbol")
                        .WithMany()
                        .HasForeignKey("SmartComSymbolId");
                });

            modelBuilder.Entity("SpeculatorModel.Transaq.Board", b =>
                {
                    b.HasOne("SpeculatorModel.Transaq.Market")
                        .WithMany()
                        .HasForeignKey("MarketId");
                });

            modelBuilder.Entity("SpeculatorModel.Transaq.Security", b =>
                {
                    b.HasOne("SpeculatorModel.Transaq.Board")
                        .WithMany()
                        .HasForeignKey("BoardId");

                    b.HasOne("SpeculatorModel.Transaq.Market")
                        .WithMany()
                        .HasForeignKey("MarketId");
                });
        }
    }
}
