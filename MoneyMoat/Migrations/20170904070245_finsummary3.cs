using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace MoneyMoat.Migrations
{
    public partial class finsummary3 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<float>(
                name: "OTLO",
                table: "FinSummarys",
                type: "float",
                nullable: false,
                defaultValue: 0f);

            migrationBuilder.AddColumn<float>(
                name: "SCEX",
                table: "FinSummarys",
                type: "float",
                nullable: false,
                defaultValue: 0f);

            migrationBuilder.AddColumn<float>(
                name: "SCSI",
                table: "FinSummarys",
                type: "float",
                nullable: false,
                defaultValue: 0f);

            migrationBuilder.AddColumn<float>(
                name: "SNCC",
                table: "FinSummarys",
                type: "float",
                nullable: false,
                defaultValue: 0f);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "OTLO",
                table: "FinSummarys");

            migrationBuilder.DropColumn(
                name: "SCEX",
                table: "FinSummarys");

            migrationBuilder.DropColumn(
                name: "SCSI",
                table: "FinSummarys");

            migrationBuilder.DropColumn(
                name: "SNCC",
                table: "FinSummarys");
        }
    }
}
