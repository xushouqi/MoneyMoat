using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace MoneyMoat.Migrations
{
    public partial class remestimate2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "LTGROWTH",
                table: "NPEstimates");

            migrationBuilder.DropColumn(
                name: "TARGETPRICE",
                table: "NPEstimates");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<float>(
                name: "LTGROWTH",
                table: "NPEstimates",
                nullable: false,
                defaultValue: 0f);

            migrationBuilder.AddColumn<float>(
                name: "TARGETPRICE",
                table: "NPEstimates",
                nullable: false,
                defaultValue: 0f);
        }
    }
}
