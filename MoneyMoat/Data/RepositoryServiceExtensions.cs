
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using CommonLibs;
using CommonServices.Caching;

namespace StockModels
{
    public static class RepositoryServiceExtensions
    {
        //注册数据对象仓库
        public static IServiceCollection AddRepositoryService(this IServiceCollection services, IHostingEnvironment env, IConfigurationRoot config)
        {
            services.AddTransient<IRepository<Financal>, Repository<Financal, MainDbContext>>();
            services.AddTransient<IRepository<FinStatement>, Repository<FinStatement, MainDbContext>>();
            services.AddTransient<IRepository<FinSummary>, Repository<FinSummary, MainDbContext>>();
            services.AddTransient<IRepository<FYEstimate>, Repository<FYEstimate, MainDbContext>>();
            services.AddTransient<IRepository<Historical>, Repository<Historical, MainDbContext>>();
            services.AddTransient<IRepository<NPEstimate>, Repository<NPEstimate, MainDbContext>>();
            services.AddTransient<IRepository<Recommendation>, Repository<Recommendation, MainDbContext>>();
            services.AddTransient<IRepository<Stock>, Repository<Stock, MainDbContext>>();
            services.AddTransient<IRepository<XueQiuQuote>, Repository<XueQiuQuote, MainDbContext>>();

            services.AddSingleton<ICacheClient<Stock>, HybridCacheClient<Stock>>();

            return services;
        }
    }
}
