using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace MoneyMoat.Migrations
{
    public partial class remestimate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DnGradings",
                table: "NPEstimates");

            migrationBuilder.DropColumn(
                name: "UpGradings",
                table: "NPEstimates");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "DnGradings",
                table: "NPEstimates",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "UpGradings",
                table: "NPEstimates",
                nullable: false,
                defaultValue: 0);
        }
    }
}
