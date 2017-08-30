using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace MoneyMoat.Migrations
{
    public partial class quote : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "XueQiuQuotes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Symbol = table.Column<string>(type: "longtext", nullable: true),
                    UpdateTime = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    afterHours = table.Column<float>(type: "float", nullable: false),
                    afterHoursChg = table.Column<float>(type: "float", nullable: false),
                    afterHoursPct = table.Column<float>(type: "float", nullable: false),
                    afterHoursTime = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    after_hour_vol = table.Column<int>(type: "int", nullable: false),
                    amount = table.Column<float>(type: "float", nullable: false),
                    amplitude = table.Column<string>(type: "longtext", nullable: true),
                    benefit_after_tax = table.Column<float>(type: "float", nullable: false),
                    benefit_before_tax = table.Column<float>(type: "float", nullable: false),
                    beta = table.Column<float>(type: "float", nullable: false),
                    change = table.Column<float>(type: "float", nullable: false),
                    circulation = table.Column<float>(type: "float", nullable: false),
                    convert_bond_ratio = table.Column<string>(type: "longtext", nullable: true),
                    current = table.Column<float>(type: "float", nullable: false),
                    dividend = table.Column<float>(type: "float", nullable: false),
                    ebitda = table.Column<float>(type: "float", nullable: false),
                    eps = table.Column<float>(type: "float", nullable: false),
                    fall_stop = table.Column<float>(type: "float", nullable: false),
                    float_market_capital = table.Column<float>(type: "float", nullable: false),
                    high52week = table.Column<float>(type: "float", nullable: false),
                    instOwn = table.Column<float>(type: "float", nullable: false),
                    last_close = table.Column<float>(type: "float", nullable: false),
                    lot_volume = table.Column<float>(type: "float", nullable: false),
                    low52week = table.Column<float>(type: "float", nullable: false),
                    marketCapital = table.Column<float>(type: "float", nullable: false),
                    net_assets = table.Column<float>(type: "float", nullable: false),
                    outstandingamt = table.Column<string>(type: "longtext", nullable: true),
                    par_value = table.Column<float>(type: "float", nullable: false),
                    pb = table.Column<float>(type: "float", nullable: false),
                    pe_lyr = table.Column<string>(type: "longtext", nullable: true),
                    pe_ttm = table.Column<float>(type: "float", nullable: false),
                    peg_ratio = table.Column<float>(type: "float", nullable: false),
                    percent5m = table.Column<float>(type: "float", nullable: false),
                    percentage = table.Column<float>(type: "float", nullable: false),
                    rise_stop = table.Column<float>(type: "float", nullable: false),
                    short_ratio = table.Column<float>(type: "float", nullable: false),
                    time = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    totalShares = table.Column<int>(type: "int", nullable: false),
                    totalissuescale = table.Column<string>(type: "longtext", nullable: true),
                    turnover_rate = table.Column<string>(type: "longtext", nullable: true),
                    updateAt = table.Column<long>(type: "bigint", nullable: false),
                    volume = table.Column<float>(type: "float", nullable: false),
                    volumeAverage = table.Column<float>(type: "float", nullable: false),
                    volume_ratio = table.Column<float>(type: "float", nullable: false),
                    yield = table.Column<float>(type: "float", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_XueQiuQuotes", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "XueQiuQuotes");
        }
    }
}
