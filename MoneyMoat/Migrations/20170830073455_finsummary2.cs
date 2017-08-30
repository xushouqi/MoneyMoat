using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace MoneyMoat.Migrations
{
    public partial class finsummary2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Type",
                table: "FinSummarys");

            migrationBuilder.DropColumn(
                name: "Value",
                table: "FinSummarys");

            migrationBuilder.AddColumn<float>(
                name: "EPS",
                table: "FinSummarys",
                type: "float",
                nullable: false,
                defaultValue: 0f);

            migrationBuilder.AddColumn<float>(
                name: "TotalRevenue",
                table: "FinSummarys",
                type: "float",
                nullable: false,
                defaultValue: 0f);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "EPS",
                table: "FinSummarys");

            migrationBuilder.DropColumn(
                name: "TotalRevenue",
                table: "FinSummarys");

            migrationBuilder.AddColumn<string>(
                name: "Type",
                table: "FinSummarys",
                nullable: true);

            migrationBuilder.AddColumn<float>(
                name: "Value",
                table: "FinSummarys",
                nullable: false,
                defaultValue: 0f);
        }
    }
}
