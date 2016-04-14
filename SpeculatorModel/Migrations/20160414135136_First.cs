using System;
using System.Collections.Generic;
using Microsoft.Data.Entity.Migrations;
using Microsoft.Data.Entity.Metadata;

namespace SpeculatorModel.Migrations
{
    public partial class First : Migration
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
                    Added = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "getdate()"),
                    IsBid = table.Column<bool>(nullable: false),
                    Price = table.Column<double>(nullable: false),
                    RowId = table.Column<byte>(nullable: false),
                    SmartComSymbolId = table.Column<int>(nullable: false),
                    Volume = table.Column<int>(nullable: false)
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
            migrationBuilder.CreateTable(
                name: "SmartComQuotes",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Ask = table.Column<double>(nullable: false),
                    AskSize = table.Column<int>(nullable: false),
                    Bid = table.Column<double>(nullable: false),
                    BidSize = table.Column<int>(nullable: false),
                    LastTradeDateTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastTradePrice = table.Column<double>(nullable: false),
                    LastTradeVolume = table.Column<int>(nullable: false),
                    OpenInterest = table.Column<int>(nullable: false),
                    QuoteAdded = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "getdate()"),
                    SmartComSymbolId = table.Column<int>(nullable: false),
                    Volatility = table.Column<double>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SmartComQuote", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SmartComQuote_SmartComSymbol_SmartComSymbolId",
                        column: x => x.SmartComSymbolId,
                        principalTable: "SmartComSymbols",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });
            migrationBuilder.CreateTable(
                name: "SmartComTicks",
                columns: table => new
                {
                    TradeNo = table.Column<long>(nullable: false),
                    OrderAction = table.Column<byte>(nullable: false),
                    Price = table.Column<double>(nullable: false),
                    SmartComSymbolId = table.Column<int>(nullable: false),
                    TradeAdded = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "getdate()"),
                    TradeDateTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Volume = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SmartComTick", x => x.TradeNo);
                    table.ForeignKey(
                        name: "FK_SmartComTick_SmartComSymbol_SmartComSymbolId",
                        column: x => x.SmartComSymbolId,
                        principalTable: "SmartComSymbols",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable("SmartComBidAskValues");
            migrationBuilder.DropTable("SmartComQuotes");
            migrationBuilder.DropTable("SmartComTicks");
            migrationBuilder.DropTable("SmartComSymbols");
        }
    }
}
