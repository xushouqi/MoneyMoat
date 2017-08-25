using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using Pomelo.EntityFrameworkCore;
using Pomelo.EntityFrameworkCore.MySql;
using MoneyModels;

namespace MoneyMoat
{
    public class TestDbContext : DbContext
    {
        //public DbSet<Stock> Stocks { get; set; }
        public DbSet<Financal> Financals { get; set; }

        public TestDbContext(DbContextOptions<TestDbContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            //builder.Entity<Stock>()
            //    .HasIndex(b => b.Symbol);
            //builder.Entity<Stock>()
            //    .HasIndex(b => b.ConId);
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

    public class TestDbContextFactory : IDesignTimeDbContextFactory<TestDbContext>
    {
        public TestDbContext CreateDbContext(string[] args)
        {
            var cbuilder = new ConfigurationBuilder()
            .AddJsonFile("appsettings.json");
            var config = cbuilder.Build();

            var conn = config.GetConnectionString("MySql");

            var builder = new DbContextOptionsBuilder<TestDbContext>();
            builder.UseMySql(conn);
            return new TestDbContext(builder.Options);
        }
    }
}
