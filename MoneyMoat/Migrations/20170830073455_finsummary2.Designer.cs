﻿// <auto-generated />
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.EntityFrameworkCore.Storage.Internal;
using MoneyMoat;
using System;

namespace MoneyMoat.Migrations
{
    [DbContext(typeof(MoatDbContext))]
    [Migration("20170830073455_finsummary2")]
    partial class finsummary2
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn)
                .HasAnnotation("ProductVersion", "2.0.0-rtm-26452");

            modelBuilder.Entity("MoneyModels.Financal", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Symbol");

                    b.Property<DateTime>("UpdateTime");

                    b.Property<float>("Value");

                    b.Property<long>("endMonth");

                    b.Property<long>("fYear");

                    b.Property<string>("periodType");

                    b.Property<string>("type");

                    b.HasKey("Id");

                    b.ToTable("Financals");
                });

            modelBuilder.Entity("MoneyModels.FinSummary", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn);

                    b.Property<float>("EPS");

                    b.Property<string>("Symbol");

                    b.Property<float>("TotalRevenue");

                    b.Property<DateTime>("UpdateTime");

                    b.Property<DateTime>("asofDate");

                    b.Property<string>("period");

                    b.Property<string>("reportType");

                    b.HasKey("Id");

                    b.HasIndex("Symbol");

                    b.HasIndex("asofDate");

                    b.HasIndex("period");

                    b.HasIndex("reportType");

                    b.ToTable("FinSummarys");
                });

            modelBuilder.Entity("MoneyModels.FYEstimate", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn);

                    b.Property<float>("High");

                    b.Property<float>("Low");

                    b.Property<float>("Mean");

                    b.Property<float>("Median");

                    b.Property<int>("NumOfEst");

                    b.Property<float>("StdDev");

                    b.Property<string>("Symbol");

                    b.Property<DateTime>("UpdateTime");

                    b.Property<long>("endMonth");

                    b.Property<long>("fYear");

                    b.Property<string>("periodType");

                    b.Property<string>("type");

                    b.HasKey("Id");

                    b.HasIndex("Symbol");

                    b.ToTable("FYEstimates");
                });

            modelBuilder.Entity("MoneyModels.NPEstimate", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn);

                    b.Property<float>("High");

                    b.Property<float>("Low");

                    b.Property<float>("Mean");

                    b.Property<float>("Median");

                    b.Property<int>("NumOfEst");

                    b.Property<float>("StdDev");

                    b.Property<string>("Symbol");

                    b.Property<DateTime>("UpdateTime");

                    b.Property<string>("type");

                    b.HasKey("Id");

                    b.HasIndex("Symbol");

                    b.ToTable("NPEstimates");
                });

            modelBuilder.Entity("MoneyModels.Recommendation", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("BUY");

                    b.Property<int>("HOLD");

                    b.Property<int>("OUTPERFORM");

                    b.Property<int>("SELL");

                    b.Property<string>("Symbol");

                    b.Property<int>("UNDERPERFORM");

                    b.Property<DateTime>("UpdateTime");

                    b.HasKey("Id");

                    b.ToTable("Recommendations");
                });

            modelBuilder.Entity("MoneyModels.Stock", b =>
                {
                    b.Property<string>("Symbol")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Category");

                    b.Property<int>("CommonShareholders");

                    b.Property<int>("ConId");

                    b.Property<string>("Currency");

                    b.Property<DateTime>("EarliestDate");

                    b.Property<int>("Employees");

                    b.Property<string>("Exchange");

                    b.Property<float>("MarketCap");

                    b.Property<string>("Name");

                    b.Property<long>("SharesOut");

                    b.Property<DateTime>("UpdateTime");

                    b.HasKey("Symbol");

                    b.HasIndex("ConId");

                    b.HasIndex("Symbol");

                    b.ToTable("Stocks");
                });

            modelBuilder.Entity("MoneyModels.XueQiuData", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Symbol");

                    b.Property<DateTime>("UpdateTime");

                    b.Property<float>("chg");

                    b.Property<float>("close");

                    b.Property<float>("dea");

                    b.Property<float>("dif");

                    b.Property<float>("high");

                    b.Property<int>("lot_volume");

                    b.Property<float>("low");

                    b.Property<float>("ma10");

                    b.Property<float>("ma20");

                    b.Property<float>("ma30");

                    b.Property<float>("ma5");

                    b.Property<float>("macd");

                    b.Property<float>("open");

                    b.Property<float>("percent");

                    b.Property<DateTime>("time");

                    b.Property<long>("timestamp");

                    b.Property<float>("turnrate");

                    b.Property<int>("volume");

                    b.HasKey("Id");

                    b.HasIndex("Symbol");

                    b.HasIndex("time");

                    b.ToTable("XueQiuDatas");
                });

            modelBuilder.Entity("MoneyModels.XueQiuQuote", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Symbol");

                    b.Property<DateTime>("UpdateTime");

                    b.Property<float>("afterHours");

                    b.Property<float>("afterHoursChg");

                    b.Property<float>("afterHoursPct");

                    b.Property<DateTime>("afterHoursTime");

                    b.Property<float>("amount");

                    b.Property<string>("amplitude");

                    b.Property<float>("benefit_after_tax");

                    b.Property<float>("benefit_before_tax");

                    b.Property<float>("beta");

                    b.Property<float>("change");

                    b.Property<string>("convert_bond_ratio");

                    b.Property<float>("current");

                    b.Property<float>("dividend");

                    b.Property<float>("eps");

                    b.Property<float>("fall_stop");

                    b.Property<float>("high52week");

                    b.Property<float>("instOwn");

                    b.Property<float>("last_close");

                    b.Property<float>("lot_volume");

                    b.Property<float>("low52week");

                    b.Property<float>("marketCapital");

                    b.Property<float>("net_assets");

                    b.Property<string>("outstandingamt");

                    b.Property<float>("pb");

                    b.Property<string>("pe_lyr");

                    b.Property<float>("pe_ttm");

                    b.Property<float>("percentage");

                    b.Property<float>("rise_stop");

                    b.Property<DateTime>("time");

                    b.Property<int>("totalShares");

                    b.Property<string>("totalissuescale");

                    b.Property<string>("turnover_rate");

                    b.Property<long>("updateAt");

                    b.Property<float>("volume");

                    b.Property<float>("volumeAverage");

                    b.Property<float>("yield");

                    b.HasKey("Id");

                    b.ToTable("XueQiuQuotes");
                });
#pragma warning restore 612, 618
        }
    }
}