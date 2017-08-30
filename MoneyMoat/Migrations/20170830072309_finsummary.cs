using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace MoneyMoat.Migrations
{
    public partial class finsummary : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "after_hour_vol",
                table: "XueQiuQuotes");

            migrationBuilder.DropColumn(
                name: "circulation",
                table: "XueQiuQuotes");

            migrationBuilder.DropColumn(
                name: "ebitda",
                table: "XueQiuQuotes");

            migrationBuilder.DropColumn(
                name: "float_market_capital",
                table: "XueQiuQuotes");

            migrationBuilder.DropColumn(
                name: "par_value",
                table: "XueQiuQuotes");

            migrationBuilder.DropColumn(
                name: "peg_ratio",
                table: "XueQiuQuotes");

            migrationBuilder.DropColumn(
                name: "percent5m",
                table: "XueQiuQuotes");

            migrationBuilder.DropColumn(
                name: "short_ratio",
                table: "XueQiuQuotes");

            migrationBuilder.DropColumn(
                name: "volume_ratio",
                table: "XueQiuQuotes");

            migrationBuilder.CreateTable(
                name: "FinSummarys",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Symbol = table.Column<string>(type: "varchar(127)", nullable: true),
                    Type = table.Column<string>(type: "longtext", nullable: true),
                    UpdateTime = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    Value = table.Column<float>(type: "float", nullable: false),
                    asofDate = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    period = table.Column<string>(type: "varchar(127)", nullable: true),
                    reportType = table.Column<string>(type: "varchar(127)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FinSummarys", x => x.Id);
                });

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
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "FinSummarys");

            migrationBuilder.AddColumn<int>(
                name: "after_hour_vol",
                table: "XueQiuQuotes",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<float>(
                name: "circulation",
                table: "XueQiuQuotes",
                nullable: false,
                defaultValue: 0f);

            migrationBuilder.AddColumn<float>(
                name: "ebitda",
                table: "XueQiuQuotes",
                nullable: false,
                defaultValue: 0f);

            migrationBuilder.AddColumn<float>(
                name: "float_market_capital",
                table: "XueQiuQuotes",
                nullable: false,
                defaultValue: 0f);

            migrationBuilder.AddColumn<float>(
                name: "par_value",
                table: "XueQiuQuotes",
                nullable: false,
                defaultValue: 0f);

            migrationBuilder.AddColumn<float>(
                name: "peg_ratio",
                table: "XueQiuQuotes",
                nullable: false,
                defaultValue: 0f);

            migrationBuilder.AddColumn<float>(
                name: "percent5m",
                table: "XueQiuQuotes",
                nullable: false,
                defaultValue: 0f);

            migrationBuilder.AddColumn<float>(
                name: "short_ratio",
                table: "XueQiuQuotes",
                nullable: false,
                defaultValue: 0f);

            migrationBuilder.AddColumn<float>(
                name: "volume_ratio",
                table: "XueQiuQuotes",
                nullable: false,
                defaultValue: 0f);
        }
    }
}
