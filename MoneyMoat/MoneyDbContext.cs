using Microsoft.EntityFrameworkCore;
using MoneyModels;

namespace MoneyMoat
{
    public class MoneyDbContext : DbContext
    {
        public DbSet<Stock> Stocks { get; set; }
    }
}
