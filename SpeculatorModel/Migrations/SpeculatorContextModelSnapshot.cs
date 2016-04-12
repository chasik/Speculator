using System;
using Microsoft.Data.Entity;
using Microsoft.Data.Entity.Infrastructure;
using Microsoft.Data.Entity.Metadata;
using Microsoft.Data.Entity.Migrations;
using SpeculatorModel;

namespace SpeculatorModel.Migrations
{
    [DbContext(typeof(SpeculatorContext))]
    partial class SpeculatorContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.0-rc1-16348")
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("SpeculatorModel.SmartComBidAskValue", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<bool>("IsBid");

                    b.Property<double>("Price");

                    b.Property<byte>("RowId");

                    b.Property<int>("SmartComSymbolId");

                    b.Property<double>("Volume");

                    b.HasKey("Id");

                    b.HasAnnotation("Relational:TableName", "SmartComBidAskValues");
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

            modelBuilder.Entity("SpeculatorModel.SmartComBidAskValue", b =>
                {
                    b.HasOne("SpeculatorModel.SmartComSymbol")
                        .WithMany()
                        .HasForeignKey("SmartComSymbolId");
                });
        }
    }
}
