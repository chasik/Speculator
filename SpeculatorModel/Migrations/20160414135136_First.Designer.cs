using System;
using Microsoft.Data.Entity;
using Microsoft.Data.Entity.Infrastructure;
using Microsoft.Data.Entity.Metadata;
using Microsoft.Data.Entity.Migrations;
using SpeculatorModel;

namespace SpeculatorModel.Migrations
{
    [DbContext(typeof(SpeculatorContext))]
    [Migration("20160414135136_First")]
    partial class First
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.0-rc1-16348")
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

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
        }
    }
}
