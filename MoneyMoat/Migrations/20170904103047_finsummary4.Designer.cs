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
    [Migration("20170904103047_finsummary4")]
    partial class finsummary4
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

            modelBuilder.Entity("MoneyModels.FinStatement", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn);

                    b.Property<DateTime>("EndDate");

                    b.Property<string>("FiscalPeriod");

                    b.Property<int>("FiscalYear");

                    b.Property<string>("Symbol");

                    b.Property<DateTime>("UpdateTime");

                    b.Property<float>("Value");

                    b.Property<string>("coaCode");

                    b.HasKey("Id");

                    b.HasIndex("EndDate");

                    b.HasIndex("FiscalPeriod");

                    b.HasIndex("FiscalYear");

                    b.HasIndex("Symbol");

                    b.HasIndex("coaCode");

                    b.ToTable("FinStatements");
                });

            modelBuilder.Entity("MoneyModels.FinSummary", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn);

                    b.Property<float>("EPS");

                    b.Property<float>("FreeCashFlow");

                    b.Property<float>("FreeCashFlowYoY");

                    b.Property<float>("OTLO");

                    b.Property<float>("OTLOYoY");

                    b.Property<float>("PB");

                    b.Property<float>("PE");

                    b.Property<float>("PEG");

                    b.Property<float>("PEYoY");

                    b.Property<float>("PS");

                    b.Property<float>("Price");

                    b.Property<float>("PriceYoY");

                    b.Property<float>("SCEX");

                    b.Property<float>("SCSI");

                    b.Property<float>("SNCC");

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
#pragma warning restore 612, 618
        }
    }
}
