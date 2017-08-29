﻿using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using Pomelo.EntityFrameworkCore;
using Pomelo.EntityFrameworkCore.MySql;
using MoneyModels;

namespace MoneyMoat
{
    public class MoatDbContext : DbContext
    {
        public DbSet<Stock> Stocks { get; set; }
        public DbSet<Financal> Financals { get; set; }

        public MoatDbContext(DbContextOptions<MoatDbContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<Stock>()
                .HasIndex(b => b.Symbol);
            builder.Entity<Stock>()
                .HasIndex(b => b.ConId);
        }

        //protected override void OnConfiguring(DbContextOptionsBuilder builder)
        //{
        //    if (!builder.IsConfigured)
        //    {
        //        var cbuilder = new ConfigurationBuilder()
        //        .AddJsonFile("appsettings.json");
        //        var config = cbuilder.Build();

        //        var conn = config.GetConnectionString("MySql");
        //        builder.UseMySql(conn);
        //    }
        //    base.OnConfiguring(builder);
        //}
    }

    public class ToDoContextFactory : IDesignTimeDbContextFactory<MoatDbContext>
    {
        public MoatDbContext CreateDbContext(string[] args)
        {
            var cbuilder = new ConfigurationBuilder()
            .AddJsonFile("appsettings.json");
            var config = cbuilder.Build();

            var conn = config.GetConnectionString("MySql");

            var builder = new DbContextOptionsBuilder<MoatDbContext>();
            builder.UseMySql(conn);
            return new MoatDbContext(builder.Options);
        }
    }
}
