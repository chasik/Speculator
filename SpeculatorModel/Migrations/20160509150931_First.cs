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
                name: "MoexClaimActions",
                columns: table => new
                {
                    Id = table.Column<byte>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MoexClaimAction", x => x.Id);
                });
            migrationBuilder.CreateTable(
                name: "MoexClaimTypes",
                columns: table => new
                {
                    Id = table.Column<byte>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MoexClaimType", x => x.Id);
                });
            migrationBuilder.CreateTable(
                name: "MoexSymbols",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MoexSymbol", x => x.Id);
                });
            migrationBuilder.CreateTable(
                name: "MoexSystems",
                columns: table => new
                {
                    Id = table.Column<byte>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MoexSystem", x => x.Id);
                });
            migrationBuilder.CreateTable(
                name: "MoexTradeDiractions",
                columns: table => new
                {
                    Id = table.Column<byte>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MoexTradeDiraction", x => x.Id);
                });
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
                name: "TransaqCandleKinds",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false),
                    Period = table.Column<int>(nullable: false),
                    PeriodName = table.Column<string>(type: "nvarchar(20)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CandleKind", x => x.Id);
                });
            migrationBuilder.CreateTable(
                name: "TransaqClientNegDeals",
                columns: table => new
                {
                    TransactionId = table.Column<long>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ClientNegDeal", x => x.TransactionId);
                });
            migrationBuilder.CreateTable(
                name: "TransaqClientOrders",
                columns: table => new
                {
                    TransactionId = table.Column<long>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ClientOrder", x => x.TransactionId);
                });
            migrationBuilder.CreateTable(
                name: "TransaqClientStopOrders",
                columns: table => new
                {
                    TransactionId = table.Column<long>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ClientStopOrder", x => x.TransactionId);
                });
            migrationBuilder.CreateTable(
                name: "TransaqClientTrades",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ClientTrade", x => x.Id);
                });
            migrationBuilder.CreateTable(
                name: "TransaqMarkets",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false),
                    Name = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Market", x => x.Id);
                });
            migrationBuilder.CreateTable(
                name: "TransaqQuotes",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Quote", x => x.Id);
                });
            migrationBuilder.CreateTable(
                name: "TransaqTicks",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Tick", x => x.Id);
                });
            migrationBuilder.CreateTable(
                name: "TransaqTrades",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Trade", x => x.Id);
                });
            migrationBuilder.CreateTable(
                name: "MoexClaims",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    MoexSystemId = table.Column<byte>(nullable: false),
                    Moment = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MoexClaim", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MoexClaim_MoexSystem_MoexSystemId",
                        column: x => x.MoexSystemId,
                        principalTable: "MoexSystems",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });
            migrationBuilder.CreateTable(
                name: "MoexTrades",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    MoexSystemId = table.Column<byte>(nullable: false),
                    Moment = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MoexTrade", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MoexTrade_MoexSystem_MoexSystemId",
                        column: x => x.MoexSystemId,
                        principalTable: "MoexSystems",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
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
            migrationBuilder.CreateTable(
                name: "TransaqBoards",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    InnerId = table.Column<string>(nullable: true),
                    MarketId = table.Column<int>(nullable: true),
                    Name = table.Column<string>(nullable: true),
                    TypeId = table.Column<byte>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Board", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Board_Market_MarketId",
                        column: x => x.MarketId,
                        principalTable: "TransaqMarkets",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });
            migrationBuilder.CreateTable(
                name: "TransaqSecurities",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false),
                    Active = table.Column<bool>(nullable: false),
                    BoardId = table.Column<int>(nullable: false),
                    Decimals = table.Column<byte>(nullable: false),
                    LotSize = table.Column<int>(nullable: false),
                    MarketId = table.Column<int>(nullable: false),
                    MinStep = table.Column<double>(nullable: false),
                    PointCost = table.Column<double>(nullable: false),
                    SecCode = table.Column<string>(nullable: true),
                    ShortName = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Security", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Security_Board_BoardId",
                        column: x => x.BoardId,
                        principalTable: "TransaqBoards",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Security_Market_MarketId",
                        column: x => x.MarketId,
                        principalTable: "TransaqMarkets",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });
            migrationBuilder.CreateIndex(
                name: "IX_Board_InnerId",
                table: "TransaqBoards",
                column: "InnerId",
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable("MoexClaims");
            migrationBuilder.DropTable("MoexClaimActions");
            migrationBuilder.DropTable("MoexClaimTypes");
            migrationBuilder.DropTable("MoexSymbols");
            migrationBuilder.DropTable("MoexTrades");
            migrationBuilder.DropTable("MoexTradeDiractions");
            migrationBuilder.DropTable("SmartComBidAskValues");
            migrationBuilder.DropTable("SmartComQuotes");
            migrationBuilder.DropTable("SmartComTicks");
            migrationBuilder.DropTable("TransaqCandleKinds");
            migrationBuilder.DropTable("TransaqClientNegDeals");
            migrationBuilder.DropTable("TransaqClientOrders");
            migrationBuilder.DropTable("TransaqClientStopOrders");
            migrationBuilder.DropTable("TransaqClientTrades");
            migrationBuilder.DropTable("TransaqQuotes");
            migrationBuilder.DropTable("TransaqSecurities");
            migrationBuilder.DropTable("TransaqTicks");
            migrationBuilder.DropTable("TransaqTrades");
            migrationBuilder.DropTable("MoexSystems");
            migrationBuilder.DropTable("SmartComSymbols");
            migrationBuilder.DropTable("TransaqBoards");
            migrationBuilder.DropTable("TransaqMarkets");
        }
    }
}
