using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using SpeculatorModel;

namespace SpeculatorModel.Migrations
{
    [DbContext(typeof(SpeculatorContext))]
    partial class SpeculatorContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
            modelBuilder
                .HasAnnotation("ProductVersion", "1.0.0-rc2-20896")
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("SpeculatorModel.MainData.ClaimAction", b =>
                {
                    b.Property<byte>("Id");

                    b.Property<string>("Name")
                        .HasAnnotation("MaxLength", 20);

                    b.HasKey("Id");

                    b.ToTable("ClaimActions");
                });

            modelBuilder.Entity("SpeculatorModel.MainData.DataSource", b =>
                {
                    b.Property<byte>("Id");

                    b.Property<string>("Name")
                        .HasAnnotation("MaxLength", 100);

                    b.HasKey("Id");

                    b.ToTable("DataSources");
                });

            modelBuilder.Entity("SpeculatorModel.MainData.Diraction", b =>
                {
                    b.Property<byte>("Id");

                    b.Property<string>("Name")
                        .HasAnnotation("MaxLength", 20);

                    b.HasKey("Id");

                    b.ToTable("TradeDiractions");
                });

            modelBuilder.Entity("SpeculatorModel.MoexHistory.MoexClaim", b =>
                {
                    b.Property<long>("Id");

                    b.Property<byte>("ClaimActionId");

                    b.Property<byte>("DiractionId");

                    b.Property<int>("MoexSymbolId");

                    b.Property<byte>("MoexSystemId");

                    b.Property<long?>("MoexTradeId");

                    b.Property<DateTime>("Moment");

                    b.Property<decimal>("Price");

                    b.Property<decimal?>("PriceDeal");

                    b.Property<int>("Volume");

                    b.HasKey("Id");

                    b.HasIndex("ClaimActionId");

                    b.HasIndex("DiractionId");

                    b.HasIndex("MoexSymbolId");

                    b.HasIndex("MoexSystemId");

                    b.HasIndex("MoexTradeId");

                    b.ToTable("MoexClaims");
                });

            modelBuilder.Entity("SpeculatorModel.MoexHistory.MoexClaimType", b =>
                {
                    b.Property<byte>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Name")
                        .HasAnnotation("MaxLength", 20);

                    b.HasKey("Id");

                    b.ToTable("MoexClaimTypes");
                });

            modelBuilder.Entity("SpeculatorModel.MoexHistory.MoexSymbol", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("LongName");

                    b.Property<int?>("LotSize");

                    b.Property<string>("Name");

                    b.Property<double?>("Punkt");

                    b.Property<string>("ShortName");

                    b.Property<double?>("Step");

                    b.HasKey("Id");

                    b.ToTable("MoexSymbols");
                });

            modelBuilder.Entity("SpeculatorModel.MoexHistory.MoexSystem", b =>
                {
                    b.Property<byte>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Name")
                        .HasAnnotation("MaxLength", 50);

                    b.HasKey("Id");

                    b.ToTable("MoexSystems");
                });

            modelBuilder.Entity("SpeculatorModel.MoexHistory.MoexTrade", b =>
                {
                    b.Property<long>("Id");

                    b.Property<byte>("DiractionId");

                    b.Property<int>("MoexSymbolId");

                    b.Property<byte>("MoexSystemId");

                    b.Property<DateTime>("Moment");

                    b.Property<int>("OpenInterest");

                    b.Property<decimal>("Price");

                    b.Property<int>("Volume");

                    b.HasKey("Id");

                    b.HasIndex("DiractionId");

                    b.HasIndex("MoexSymbolId");

                    b.HasIndex("MoexSystemId");

                    b.ToTable("MoexTrades");
                });

            modelBuilder.Entity("SpeculatorModel.SmartCom.SmartComBidAskValue", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<DateTime>("Added")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("datetime2")
                        .HasDefaultValueSql("getdate()");

                    b.Property<bool>("IsBid");

                    b.Property<double>("Price");

                    b.Property<byte>("RowId");

                    b.Property<int>("SmartComSymbolId");

                    b.Property<int>("Volume");

                    b.HasKey("Id");

                    b.HasIndex("SmartComSymbolId");

                    b.ToTable("SmartComBidAskValues");
                });

            modelBuilder.Entity("SpeculatorModel.SmartCom.SmartComQuote", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<double>("Ask");

                    b.Property<int>("AskSize");

                    b.Property<double>("Bid");

                    b.Property<int>("BidSize");

                    b.Property<DateTime>("LastTradeDateTime")
                        .HasColumnType("datetime2");

                    b.Property<double>("LastTradePrice");

                    b.Property<int>("LastTradeVolume");

                    b.Property<int>("OpenInterest");

                    b.Property<DateTime>("QuoteAdded")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("datetime2")
                        .HasDefaultValueSql("getdate()");

                    b.Property<int>("SmartComSymbolId");

                    b.Property<double>("Volatility");

                    b.HasKey("Id");

                    b.HasIndex("SmartComSymbolId");

                    b.ToTable("SmartComQuotes");
                });

            modelBuilder.Entity("SpeculatorModel.SmartCom.SmartComSymbol", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("Decimals");

                    b.Property<DateTime>("ExpiryDate");

                    b.Property<string>("LongName");

                    b.Property<int?>("LotSize");

                    b.Property<string>("Name");

                    b.Property<double?>("Punkt");

                    b.Property<string>("SecExchName");

                    b.Property<string>("SecExtId");

                    b.Property<string>("ShortName");

                    b.Property<double?>("Step");

                    b.Property<double?>("Strike");

                    b.Property<string>("Type");

                    b.HasKey("Id");

                    b.ToTable("SmartComSymbols");
                });

            modelBuilder.Entity("SpeculatorModel.SmartCom.SmartComTrade", b =>
                {
                    b.Property<long>("TradeNo");

                    b.Property<byte>("DiractionId");

                    b.Property<double>("Price");

                    b.Property<int>("SmartComSymbolId");

                    b.Property<DateTime>("TradeAdded")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("datetime2")
                        .HasDefaultValueSql("getdate()");

                    b.Property<DateTime>("TradeDateTime")
                        .HasColumnType("datetime2");

                    b.Property<int>("Volume");

                    b.HasKey("TradeNo");

                    b.HasIndex("DiractionId");

                    b.HasIndex("SmartComSymbolId");

                    b.ToTable("SmartComTrades");
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

                    b.HasIndex("MarketId");

                    b.ToTable("TransaqBoards");
                });

            modelBuilder.Entity("SpeculatorModel.Transaq.CandleKind", b =>
                {
                    b.Property<int>("Id");

                    b.Property<int>("Period");

                    b.Property<string>("PeriodName")
                        .HasColumnType("nvarchar(20)");

                    b.HasKey("Id");

                    b.ToTable("TransaqCandleKinds");
                });

            modelBuilder.Entity("SpeculatorModel.Transaq.ClientNegDeal", b =>
                {
                    b.Property<long>("TransactionId");

                    b.HasKey("TransactionId");

                    b.ToTable("TransaqClientNegDeals");
                });

            modelBuilder.Entity("SpeculatorModel.Transaq.ClientOrder", b =>
                {
                    b.Property<long>("TransactionId");

                    b.HasKey("TransactionId");

                    b.ToTable("TransaqClientOrders");
                });

            modelBuilder.Entity("SpeculatorModel.Transaq.ClientStopOrder", b =>
                {
                    b.Property<long>("TransactionId");

                    b.HasKey("TransactionId");

                    b.ToTable("TransaqClientStopOrders");
                });

            modelBuilder.Entity("SpeculatorModel.Transaq.ClientTrade", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.HasKey("Id");

                    b.ToTable("TransaqClientTrades");
                });

            modelBuilder.Entity("SpeculatorModel.Transaq.Market", b =>
                {
                    b.Property<int>("Id");

                    b.Property<string>("Name")
                        .HasAnnotation("MaxLength", 30);

                    b.HasKey("Id");

                    b.ToTable("TransaqMarkets");
                });

            modelBuilder.Entity("SpeculatorModel.Transaq.Quote", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.HasKey("Id");

                    b.ToTable("TransaqQuotes");
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

                    b.HasIndex("BoardId");

                    b.HasIndex("MarketId");

                    b.ToTable("TransaqSecurities");
                });

            modelBuilder.Entity("SpeculatorModel.Transaq.Tick", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.HasKey("Id");

                    b.ToTable("TransaqTicks");
                });

            modelBuilder.Entity("SpeculatorModel.Transaq.TransaqTrade", b =>
                {
                    b.Property<long>("TradeNo");

                    b.Property<byte>("DiractionId");

                    b.Property<double>("Price");

                    b.Property<int>("Volume");

                    b.HasKey("TradeNo");

                    b.HasIndex("DiractionId");

                    b.ToTable("TransaqTrades");
                });

            modelBuilder.Entity("SpeculatorModel.MoexHistory.MoexClaim", b =>
                {
                    b.HasOne("SpeculatorModel.MainData.ClaimAction")
                        .WithMany()
                        .HasForeignKey("ClaimActionId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("SpeculatorModel.MainData.Diraction")
                        .WithMany()
                        .HasForeignKey("DiractionId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("SpeculatorModel.MoexHistory.MoexSymbol")
                        .WithMany()
                        .HasForeignKey("MoexSymbolId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("SpeculatorModel.MoexHistory.MoexSystem")
                        .WithMany()
                        .HasForeignKey("MoexSystemId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("SpeculatorModel.MoexHistory.MoexTrade")
                        .WithMany()
                        .HasForeignKey("MoexTradeId");
                });

            modelBuilder.Entity("SpeculatorModel.MoexHistory.MoexTrade", b =>
                {
                    b.HasOne("SpeculatorModel.MainData.Diraction")
                        .WithMany()
                        .HasForeignKey("DiractionId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("SpeculatorModel.MoexHistory.MoexSymbol")
                        .WithMany()
                        .HasForeignKey("MoexSymbolId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("SpeculatorModel.MoexHistory.MoexSystem")
                        .WithMany()
                        .HasForeignKey("MoexSystemId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("SpeculatorModel.SmartCom.SmartComBidAskValue", b =>
                {
                    b.HasOne("SpeculatorModel.SmartCom.SmartComSymbol")
                        .WithMany()
                        .HasForeignKey("SmartComSymbolId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("SpeculatorModel.SmartCom.SmartComQuote", b =>
                {
                    b.HasOne("SpeculatorModel.SmartCom.SmartComSymbol")
                        .WithMany()
                        .HasForeignKey("SmartComSymbolId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("SpeculatorModel.SmartCom.SmartComTrade", b =>
                {
                    b.HasOne("SpeculatorModel.MainData.Diraction")
                        .WithMany()
                        .HasForeignKey("DiractionId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("SpeculatorModel.SmartCom.SmartComSymbol")
                        .WithMany()
                        .HasForeignKey("SmartComSymbolId")
                        .OnDelete(DeleteBehavior.Cascade);
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
                        .HasForeignKey("BoardId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("SpeculatorModel.Transaq.Market")
                        .WithMany()
                        .HasForeignKey("MarketId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("SpeculatorModel.Transaq.TransaqTrade", b =>
                {
                    b.HasOne("SpeculatorModel.MainData.Diraction")
                        .WithMany()
                        .HasForeignKey("DiractionId")
                        .OnDelete(DeleteBehavior.Cascade);
                });
        }
    }
}
