using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace MoneyMoat.Migrations
{
    public partial class estimate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
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
                    DnGradings = table.Column<int>(type: "int", nullable: false),
                    High = table.Column<float>(type: "float", nullable: false),
                    LTGROWTH = table.Column<float>(type: "float", nullable: false),
                    Low = table.Column<float>(type: "float", nullable: false),
                    Mean = table.Column<float>(type: "float", nullable: false),
                    Median = table.Column<float>(type: "float", nullable: false),
                    NumOfEst = table.Column<int>(type: "int", nullable: false),
                    StdDev = table.Column<float>(type: "float", nullable: false),
                    Symbol = table.Column<string>(type: "varchar(127)", nullable: true),
                    TARGETPRICE = table.Column<float>(type: "float", nullable: false),
                    UpGradings = table.Column<int>(type: "int", nullable: false),
                    UpdateTime = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    type = table.Column<string>(type: "longtext", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_NPEstimates", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Recommendations",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    BUY = table.Column<int>(type: "int", nullable: false),
                    HOLD = table.Column<int>(type: "int", nullable: false),
                    OUTPERFORM = table.Column<int>(type: "int", nullable: false),
                    SELL = table.Column<int>(type: "int", nullable: false),
                    Symbol = table.Column<string>(type: "longtext", nullable: true),
                    UNDERPERFORM = table.Column<int>(type: "int", nullable: false),
                    UpdateTime = table.Column<DateTime>(type: "datetime(6)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Recommendations", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_FYEstimates_Symbol",
                table: "FYEstimates",
                column: "Symbol");

            migrationBuilder.CreateIndex(
                name: "IX_NPEstimates_Symbol",
                table: "NPEstimates",
                column: "Symbol");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "FYEstimates");

            migrationBuilder.DropTable(
                name: "NPEstimates");

            migrationBuilder.DropTable(
                name: "Recommendations");
        }
    }
}
