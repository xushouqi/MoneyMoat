using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace MoneyMoat.Migrations
{
    public partial class finsummary4 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<float>(
                name: "FreeCashFlow",
                table: "FinSummarys",
                type: "float",
                nullable: false,
                defaultValue: 0f);

            migrationBuilder.AddColumn<float>(
                name: "FreeCashFlowYoY",
                table: "FinSummarys",
                type: "float",
                nullable: false,
                defaultValue: 0f);

            migrationBuilder.AddColumn<float>(
                name: "OTLOYoY",
                table: "FinSummarys",
                type: "float",
                nullable: false,
                defaultValue: 0f);

            migrationBuilder.AddColumn<float>(
                name: "PEYoY",
                table: "FinSummarys",
                type: "float",
                nullable: false,
                defaultValue: 0f);

            migrationBuilder.AddColumn<float>(
                name: "PriceYoY",
                table: "FinSummarys",
                type: "float",
                nullable: false,
                defaultValue: 0f);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "FreeCashFlow",
                table: "FinSummarys");

            migrationBuilder.DropColumn(
                name: "FreeCashFlowYoY",
                table: "FinSummarys");

            migrationBuilder.DropColumn(
                name: "OTLOYoY",
                table: "FinSummarys");

            migrationBuilder.DropColumn(
                name: "PEYoY",
                table: "FinSummarys");

            migrationBuilder.DropColumn(
                name: "PriceYoY",
                table: "FinSummarys");
        }
    }
}
