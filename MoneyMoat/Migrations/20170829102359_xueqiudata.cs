using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace MoneyMoat.Migrations
{
    public partial class xueqiudata : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
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
                name: "XueQiuDatas");
        }
    }
}
