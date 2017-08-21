using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace MoneyMoat.Migrations
{
    public partial class financal : Migration
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

            migrationBuilder.CreateIndex(
                name: "IX_Stocks_ConId",
                table: "Stocks",
                column: "ConId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Financals");

            migrationBuilder.DropIndex(
                name: "IX_Stocks_ConId",
                table: "Stocks");
        }
    }
}
