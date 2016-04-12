using System;
using System.Collections.Generic;
using Microsoft.Data.Entity.Migrations;
using Microsoft.Data.Entity.Metadata;

namespace SpeculatorModel.Migrations
{
    public partial class FirstMigration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "SmartComSymbols",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Decimals = table.Column<int>(nullable: false),
                    ExpiryDate = table.Column<DateTime>(nullable: false),
                    LongName = table.Column<string>(nullable: true),
                    LotSize = table.Column<int>(nullable: false),
                    Punkt = table.Column<double>(nullable: true),
                    SecExchName = table.Column<string>(nullable: true),
                    SecExtId = table.Column<string>(nullable: true),
                    ShortName = table.Column<string>(nullable: true),
                    Step = table.Column<double>(nullable: true),
                    Strike = table.Column<double>(nullable: true),
                    Symbol = table.Column<string>(nullable: true),
                    Type = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SmartComSymbol", x => x.Id);
                });
            migrationBuilder.CreateTable(
                name: "SmartComBidAskValues",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    IsBid = table.Column<bool>(nullable: false),
                    Price = table.Column<double>(nullable: false),
                    RowId = table.Column<byte>(nullable: false),
                    SmartComSymbolId = table.Column<int>(nullable: false),
                    Volume = table.Column<double>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SmartComBidAskValue", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SmartComBidAskValue_SmartComSymbol_SmartComSymbolId",
                        column: x => x.SmartComSymbolId,
                        principalTable: "SmartComSymbols",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable("SmartComBidAskValues");
            migrationBuilder.DropTable("SmartComSymbols");
        }
    }
}
