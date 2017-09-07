using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace MoneyMoat.Migrations
{
    public partial class first : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Financals",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Symbol = table.Column<string>(type: "longtext", nullable: true),
                    UpdateTime = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    Value = table.Column<float>(type: "float", nullable: false),
                    endMonth = table.Column<long>(type: "bigint", nullable: false),
                    fYear = table.Column<long>(type: "bigint", nullable: false),
                    periodType = table.Column<string>(type: "longtext", nullable: true),
                    type = table.Column<string>(type: "longtext", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Financals", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "FinStatements",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    EndDate = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    FiscalPeriod = table.Column<string>(type: "varchar(127)", nullable: true),
                    FiscalYear = table.Column<int>(type: "int", nullable: false),
                    Symbol = table.Column<string>(type: "varchar(127)", nullable: true),
                    UpdateTime = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    Value = table.Column<float>(type: "float", nullable: false),
                    coaCode = table.Column<string>(type: "varchar(127)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FinStatements", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "FinSummarys",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    EPS = table.Column<float>(type: "float", nullable: false),
                    FreeCashFlow = table.Column<float>(type: "float", nullable: false),
                    FreeCashFlowYoY = table.Column<float>(type: "float", nullable: false),
                    OTLO = table.Column<float>(type: "float", nullable: false),
                    OTLOYoY = table.Column<float>(type: "float", nullable: false),
                    PB = table.Column<float>(type: "float", nullable: false),
                    PE = table.Column<float>(type: "float", nullable: false),
                    PEG = table.Column<float>(type: "float", nullable: false),
                    PEYoY = table.Column<float>(type: "float", nullable: false),
                    PS = table.Column<float>(type: "float", nullable: false),
                    Price = table.Column<float>(type: "float", nullable: false),
                    PriceYoY = table.Column<float>(type: "float", nullable: false),
                    SCEX = table.Column<float>(type: "float", nullable: false),
                    SCSI = table.Column<float>(type: "float", nullable: false),
                    SNCC = table.Column<float>(type: "float", nullable: false),
                    Symbol = table.Column<string>(type: "varchar(127)", nullable: true),
                    TotalRevenue = table.Column<float>(type: "float", nullable: false),
                    UpdateTime = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    asofDate = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    period = table.Column<string>(type: "varchar(127)", nullable: true),
                    reportType = table.Column<string>(type: "varchar(127)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FinSummarys", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "FYEstimates",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    High = table.Column<float>(type: "float", nullable: false),
                    Low = table.Column<float>(type: "float", nullable: false),
                    Mean = table.Column<float>(type: "float", nullable: false),
                    Median = table.Column<float>(type: "float", nullable: false),
                    NumOfEst = table.Column<int>(type: "int", nullable: false),
                    StdDev = table.Column<float>(type: "float", nullable: false),
                    Symbol = table.Column<string>(type: "varchar(127)", nullable: true),
                    UpdateTime = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    endMonth = table.Column<long>(type: "bigint", nullable: false),
                    fYear = table.Column<long>(type: "bigint", nullable: false),
                    periodType = table.Column<string>(type: "longtext", nullable: true),
                    type = table.Column<string>(type: "longtext", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FYEstimates", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "NPEstimates",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    High = table.Column<float>(type: "float", nullable: false),
                    Low = table.Column<float>(type: "float", nullable: false),
                    Mean = table.Column<float>(type: "float", nullable: false),
                    Median = table.Column<float>(type: "float", nullable: false),
                    NumOfEst = table.Column<int>(type: "int", nullable: false),
                    StdDev = table.Column<float>(type: "float", nullable: false),
                    Symbol = table.Column<string>(type: "varchar(127)", nullable: true),
                    UpdateTime = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    type = table.Column<string>(type: "longtext", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_NPEstimates", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Stocks",
                columns: table => new
                {
                    Symbol = table.Column<string>(type: "varchar(127)", nullable: false),
                    Category = table.Column<string>(type: "longtext", nullable: true),
                    CommonShareholders = table.Column<int>(type: "int", nullable: false),
                    ConId = table.Column<int>(type: "int", nullable: false),
                    Currency = table.Column<string>(type: "longtext", nullable: true),
                    EarliestDate = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    Employees = table.Column<int>(type: "int", nullable: false),
                    Exchange = table.Column<string>(type: "longtext", nullable: true),
                    MarketCap = table.Column<float>(type: "float", nullable: false),
                    Name = table.Column<string>(type: "longtext", nullable: true),
                    SharesOut = table.Column<long>(type: "bigint", nullable: false),
                    UpdateTime = table.Column<DateTime>(type: "datetime(6)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Stocks", x => x.Symbol);
                });

            migrationBuilder.CreateTable(
                name: "XueQiuDatas",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Symbol = table.Column<string>(type: "varchar(127)", nullable: true),
                    UpdateTime = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    chg = table.Column<float>(type: "float", nullable: false),
                    close = table.Column<float>(type: "float", nullable: false),
                    dea = table.Column<float>(type: "float", nullable: false),
                    dif = table.Column<float>(type: "float", nullable: false),
                    high = table.Column<float>(type: "float", nullable: false),
                    lot_volume = table.Column<int>(type: "int", nullable: false),
                    low = table.Column<float>(type: "float", nullable: false),
                    ma10 = table.Column<float>(type: "float", nullable: false),
                    ma20 = table.Column<float>(type: "float", nullable: false),
                    ma30 = table.Column<float>(type: "float", nullable: false),
                    ma5 = table.Column<float>(type: "float", nullable: false),
                    macd = table.Column<float>(type: "float", nullable: false),
                    open = table.Column<float>(type: "float", nullable: false),
                    percent = table.Column<float>(type: "float", nullable: false),
                    time = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    timestamp = table.Column<long>(type: "bigint", nullable: false),
                    turnrate = table.Column<float>(type: "float", nullable: false),
                    volume = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_XueQiuDatas", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_FinStatements_EndDate",
                table: "FinStatements",
                column: "EndDate");

            migrationBuilder.CreateIndex(
                name: "IX_FinStatements_FiscalPeriod",
                table: "FinStatements",
                column: "FiscalPeriod");

            migrationBuilder.CreateIndex(
                name: "IX_FinStatements_FiscalYear",
                table: "FinStatements",
                column: "FiscalYear");

            migrationBuilder.CreateIndex(
                name: "IX_FinStatements_Symbol",
                table: "FinStatements",
                column: "Symbol");

            migrationBuilder.CreateIndex(
                name: "IX_FinStatements_coaCode",
                table: "FinStatements",
                column: "coaCode");

            migrationBuilder.CreateIndex(
                name: "IX_FinSummarys_Symbol",
                table: "FinSummarys",
                column: "Symbol");

            migrationBuilder.CreateIndex(
                name: "IX_FinSummarys_asofDate",
                table: "FinSummarys",
                column: "asofDate");

            migrationBuilder.CreateIndex(
                name: "IX_FinSummarys_period",
                table: "FinSummarys",
                column: "period");

            migrationBuilder.CreateIndex(
                name: "IX_FinSummarys_reportType",
                table: "FinSummarys",
                column: "reportType");

            migrationBuilder.CreateIndex(
                name: "IX_FYEstimates_Symbol",
                table: "FYEstimates",
                column: "Symbol");

            migrationBuilder.CreateIndex(
                name: "IX_NPEstimates_Symbol",
                table: "NPEstimates",
                column: "Symbol");

            migrationBuilder.CreateIndex(
                name: "IX_Stocks_ConId",
                table: "Stocks",
                column: "ConId");

            migrationBuilder.CreateIndex(
                name: "IX_Stocks_Symbol",
                table: "Stocks",
                column: "Symbol");

            migrationBuilder.CreateIndex(
                name: "IX_XueQiuDatas_Symbol",
                table: "XueQiuDatas",
                column: "Symbol");

            migrationBuilder.CreateIndex(
                name: "IX_XueQiuDatas_time",
                table: "XueQiuDatas",
                column: "time");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Financals");

            migrationBuilder.DropTable(
                name: "FinStatements");

            migrationBuilder.DropTable(
                name: "FinSummarys");

            migrationBuilder.DropTable(
                name: "FYEstimates");

            migrationBuilder.DropTable(
                name: "NPEstimates");

            migrationBuilder.DropTable(
                name: "Stocks");

            migrationBuilder.DropTable(
                name: "XueQiuDatas");
        }
    }
}
