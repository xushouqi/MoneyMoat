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
    partial class MoatDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
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
#pragma warning restore 612, 618
        }
    }
}
