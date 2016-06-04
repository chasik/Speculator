using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Metadata;

namespace SpeculatorModel.Migrations
{
    public partial class First : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "DataSources",
                columns: table => new
                {
                    Id = table.Column<byte>(nullable: false),
                    Name = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DataSources", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "MoexClaimActions",
                columns: table => new
                {
                    Id = table.Column<byte>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {table.PrimaryKey("PK_MoexClaimActions", x => x.Id);
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
                    table.PrimaryKey("PK_MoexClaimTypes", x => x.Id);
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
                    table.PrimaryKey("PK_MoexSymbols", x => x.Id);
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
                    table.PrimaryKey("PK_MoexSystems", x => x.Id);
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
                    table.PrimaryKey("PK_MoexTradeDiractions", x => x.Id);
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
                    table.PrimaryKey("PK_SmartComSymbols", x => x.Id);
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
                    table.PrimaryKey("PK_TransaqCandleKinds", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "TransaqClientNegDeals",
                columns: table => new
                {
                    TransactionId = table.Column<long>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TransaqClientNegDeals", x => x.TransactionId);
                });

            migrationBuilder.CreateTable(
                name: "TransaqClientOrders",
                columns: table => new
                {
                    TransactionId = table.Column<long>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TransaqClientOrders", x => x.TransactionId);
                });

            migrationBuilder.CreateTable(
                name: "TransaqClientStopOrders",
                columns: table => new
                {
                    TransactionId = table.Column<long>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TransaqClientStopOrders", x => x.TransactionId);
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
                    table.PrimaryKey("PK_TransaqClientTrades", x => x.Id);
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
                    table.PrimaryKey("PK_TransaqMarkets", x => x.Id);
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
                    table.PrimaryKey("PK_TransaqQuotes", x => x.Id);
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
                    table.PrimaryKey("PK_TransaqTicks", x => x.Id);
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
                    table.PrimaryKey("PK_TransaqTrades", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "MoexClaims",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    MoexSymbolId = table.Column<byte>(nullable: false),
                    MoexSymbolId1 = table.Column<int>(nullable: true),
                    MoexSystemId = table.Column<byte>(nullable: false),
                    Moment = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MoexClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MoexClaims_MoexSymbols_MoexSymbolId1",
                        column: x => x.MoexSymbolId1,
                        principalTable: "MoexSymbols",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_MoexClaims_MoexSystems_MoexSystemId",
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
                    MoexSymbolId = table.Column<byte>(nullable: false),
                    MoexSymbolId1 = table.Column<int>(nullable: true),
                    MoexSystemId = table.Column<byte>(nullable: false),
                    Moment = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MoexTrades", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MoexTrades_MoexSymbols_MoexSymbolId1",
                        column: x => x.MoexSymbolId1,
                        principalTable: "MoexSymbols",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_MoexTrades_MoexSystems_MoexSystemId",
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
                    table.PrimaryKey("PK_SmartComBidAskValues", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SmartComBidAskValues_SmartComSymbols_SmartComSymbolId",
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
                    table.PrimaryKey("PK_SmartComQuotes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SmartComQuotes_SmartComSymbols_SmartComSymbolId",
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
                    table.PrimaryKey("PK_SmartComTicks", x => x.TradeNo);
                    table.ForeignKey(
                        name: "FK_SmartComTicks_SmartComSymbols_SmartComSymbolId",
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
                    table.PrimaryKey("PK_TransaqBoards", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TransaqBoards_TransaqMarkets_MarketId",
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
                    table.PrimaryKey("PK_TransaqSecurities", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TransaqSecurities_TransaqBoards_BoardId",
                        column: x => x.BoardId,
                        principalTable: "TransaqBoards",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_TransaqSecurities_TransaqMarkets_MarketId",
                        column: x => x.MarketId,
                        principalTable: "TransaqMarkets",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_MoexClaims_MoexSymbolId1",
                table: "MoexClaims",
                column: "MoexSymbolId1");

            migrationBuilder.CreateIndex(
                name: "IX_MoexClaims_MoexSystemId",
                table: "MoexClaims",
                column: "MoexSystemId");

            migrationBuilder.CreateIndex(
                name: "IX_MoexTrades_MoexSymbolId1",
                table: "MoexTrades",
                column: "MoexSymbolId1");

            migrationBuilder.CreateIndex(
                name: "IX_MoexTrades_MoexSystemId",
                table: "MoexTrades",
                column: "MoexSystemId");

            migrationBuilder.CreateIndex(
                name: "IX_SmartComBidAskValues_SmartComSymbolId",
                table: "SmartComBidAskValues",
                column: "SmartComSymbolId");

            migrationBuilder.CreateIndex(
                name: "IX_SmartComQuotes_SmartComSymbolId",
                table: "SmartComQuotes",
                column: "SmartComSymbolId");

            migrationBuilder.CreateIndex(
                name: "IX_SmartComTicks_SmartComSymbolId",
                table: "SmartComTicks",
                column: "SmartComSymbolId");

            migrationBuilder.CreateIndex(
                name: "IX_TransaqBoards_InnerId",
                table: "TransaqBoards",
                column: "InnerId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_TransaqBoards_MarketId",
                table: "TransaqBoards",
                column: "MarketId");

            migrationBuilder.CreateIndex(
                name: "IX_TransaqSecurities_BoardId",
                table: "TransaqSecurities",
                column: "BoardId");

            migrationBuilder.CreateIndex(
                name: "IX_TransaqSecurities_MarketId",
                table: "TransaqSecurities",
                column: "MarketId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "DataSources");

            migrationBuilder.DropTable(
                name: "MoexClaims");

            migrationBuilder.DropTable(
                name: "MoexClaimActions");

            migrationBuilder.DropTable(
                name: "MoexClaimTypes");

            migrationBuilder.DropTable(
                name: "MoexTrades");

            migrationBuilder.DropTable(
                name: "MoexTradeDiractions");

            migrationBuilder.DropTable(
                name: "SmartComBidAskValues");

            migrationBuilder.DropTable(
                name: "SmartComQuotes");

            migrationBuilder.DropTable(
                name: "SmartComTicks");

            migrationBuilder.DropTable(
                name: "TransaqCandleKinds");

            migrationBuilder.DropTable(
                name: "TransaqClientNegDeals");

            migrationBuilder.DropTable(
                name: "TransaqClientOrders");

            migrationBuilder.DropTable(
                name: "TransaqClientStopOrders");

            migrationBuilder.DropTable(
                name: "TransaqClientTrades");

            migrationBuilder.DropTable(
                name: "TransaqQuotes");

            migrationBuilder.DropTable(
                name: "TransaqSecurities");

            migrationBuilder.DropTable(
                name: "TransaqTicks");

            migrationBuilder.DropTable(
                name: "TransaqTrades");

            migrationBuilder.DropTable(
                name: "MoexSymbols");

            migrationBuilder.DropTable(
                name: "MoexSystems");

            migrationBuilder.DropTable(
                name: "SmartComSymbols");

            migrationBuilder.DropTable(
                name: "TransaqBoards");

            migrationBuilder.DropTable(
                name: "TransaqMarkets");
        }
    }
}
