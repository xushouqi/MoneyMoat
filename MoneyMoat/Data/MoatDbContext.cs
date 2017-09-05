using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using Pomelo.EntityFrameworkCore;
using Pomelo.EntityFrameworkCore.MySql;
using StockModels;
using CommonLibs;

namespace MoneyMoat
{
    [DbContext]
    public class MoatDbContext : DbContext
    {
        public DbSet<Stock> Stocks { get; set; }
        public DbSet<Financal> Financals { get; set; }
        public DbSet<FYEstimate> FYEstimates { get; set; }
        public DbSet<NPEstimate> NPEstimates { get; set; }
        //public DbSet<Recommendation> Recommendations { get; set; }
        public DbSet<Historical> XueQiuDatas { get; set; }
        //public DbSet<XueQiuQuote> XueQiuQuotes { get; set; }
        public DbSet<FinSummary> FinSummarys { get; set; }
        public DbSet<FinStatement> FinStatements { get; set; }

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

            builder.Entity<FYEstimate>()
                .HasIndex(b => b.Symbol);
            builder.Entity<NPEstimate>()
                .HasIndex(b => b.Symbol);

            builder.Entity<Historical>()
                .HasIndex(b => b.Symbol);
            builder.Entity<Historical>()
                .HasIndex(b => b.time);

            builder.Entity<FinSummary>()
                .HasIndex(b => b.Symbol);
            builder.Entity<FinSummary>()
                .HasIndex(b => b.asofDate);
            builder.Entity<FinSummary>()
                .HasIndex(b => b.reportType);
            builder.Entity<FinSummary>()
                .HasIndex(b => b.period);

            builder.Entity<FinStatement>()
                .HasIndex(b => b.Symbol);
            builder.Entity<FinStatement>()
                .HasIndex(b => b.FiscalYear);
            builder.Entity<FinStatement>()
                .HasIndex(b => b.FiscalPeriod);
            builder.Entity<FinStatement>()
                .HasIndex(b => b.coaCode);
            builder.Entity<FinStatement>()
                .HasIndex(b => b.EndDate);
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
