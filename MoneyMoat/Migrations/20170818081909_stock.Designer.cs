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
    [Migration("20170818081909_stock")]
    partial class stock
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn)
                .HasAnnotation("ProductVersion", "2.0.0-rtm-26452");

            modelBuilder.Entity("MoneyModels.Stock", b =>
                {
                    b.Property<string>("Symbol")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("ConId");

                    b.Property<string>("Currency");

                    b.Property<DateTime>("EarliestDate");

                    b.Property<string>("Exchange");

                    b.Property<string>("Name");

                    b.HasKey("Symbol");

                    b.HasIndex("Symbol");

                    b.ToTable("Stocks");
                });
#pragma warning restore 612, 618
        }
    }
}