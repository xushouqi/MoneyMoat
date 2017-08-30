using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace MoneyMoat.Migrations
{
    public partial class finstatement : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Recommendations");

            migrationBuilder.DropTable(
                name: "XueQiuQuotes");

            migrationBuilder.AddColumn<float>(
                name: "PB",
                table: "FinSummarys",
                type: "float",
                nullable: false,
                defaultValue: 0f);

            migrationBuilder.AddColumn<float>(
                name: "PE",
                table: "FinSummarys",
                type: "float",
                nullable: false,
                defaultValue: 0f);

            migrationBuilder.AddColumn<float>(
                name: "PEG",
                table: "FinSummarys",
                type: "float",
                nullable: false,
                defaultValue: 0f);

            migrationBuilder.AddColumn<float>(
                name: "PS",
                table: "FinSummarys",
                type: "float",
                nullable: false,
                defaultValue: 0f);

            migrationBuilder.AddColumn<float>(
                name: "Price",
                table: "FinSummarys",
                type: "float",
                nullable: false,
                defaultValue: 0f);

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
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "FinStatements");

            migrationBuilder.DropColumn(
                name: "PB",
                table: "FinSummarys");

            migrationBuilder.DropColumn(
                name: "PE",
                table: "FinSummarys");

            migrationBuilder.DropColumn(
                name: "PEG",
                table: "FinSummarys");

            migrationBuilder.DropColumn(
                name: "PS",
                table: "FinSummarys");

            migrationBuilder.DropColumn(
                name: "Price",
                table: "FinSummarys");

            migrationBuilder.CreateTable(
                name: "Recommendations",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    BUY = table.Column<int>(nullable: false),
                    HOLD = table.Column<int>(nullable: false),
                    OUTPERFORM = table.Column<int>(nullable: false),
                    SELL = table.Column<int>(nullable: false),
                    Symbol = table.Column<string>(nullable: true),
                    UNDERPERFORM = table.Column<int>(nullable: false),
                    UpdateTime = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Recommendations", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "XueQiuQuotes",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Symbol = table.Column<string>(nullable: true),
                    UpdateTime = table.Column<DateTime>(nullable: false),
                    afterHours = table.Column<float>(nullable: false),
                    afterHoursChg = table.Column<float>(nullable: false),
                    afterHoursPct = table.Column<float>(nullable: false),
                    afterHoursTime = table.Column<DateTime>(nullable: false),
                    amount = table.Column<float>(nullable: false),
                    amplitude = table.Column<string>(nullable: true),
                    benefit_after_tax = table.Column<float>(nullable: false),
                    benefit_before_tax = table.Column<float>(nullable: false),
                    beta = table.Column<float>(nullable: false),
                    change = table.Column<float>(nullable: false),
                    convert_bond_ratio = table.Column<string>(nullable: true),
                    current = table.Column<float>(nullable: false),
                    dividend = table.Column<float>(nullable: false),
                    eps = table.Column<float>(nullable: false),
                    fall_stop = table.Column<float>(nullable: false),
                    high52week = table.Column<float>(nullable: false),
                    instOwn = table.Column<float>(nullable: false),
                    last_close = table.Column<float>(nullable: false),
                    lot_volume = table.Column<float>(nullable: false),
                    low52week = table.Column<float>(nullable: false),
                    marketCapital = table.Column<float>(nullable: false),
                    net_assets = table.Column<float>(nullable: false),
                    outstandingamt = table.Column<string>(nullable: true),
                    pb = table.Column<float>(nullable: false),
                    pe_lyr = table.Column<string>(nullable: true),
                    pe_ttm = table.Column<float>(nullable: false),
                    percentage = table.Column<float>(nullable: false),
                    rise_stop = table.Column<float>(nullable: false),
                    time = table.Column<DateTime>(nullable: false),
                    totalShares = table.Column<int>(nullable: false),
                    totalissuescale = table.Column<string>(nullable: true),
                    turnover_rate = table.Column<string>(nullable: true),
                    updateAt = table.Column<long>(nullable: false),
                    volume = table.Column<float>(nullable: false),
                    volumeAverage = table.Column<float>(nullable: false),
                    yield = table.Column<float>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_XueQiuQuotes", x => x.Id);
                });
        }
    }
}
