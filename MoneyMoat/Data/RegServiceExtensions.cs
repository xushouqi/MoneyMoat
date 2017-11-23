using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using CommonLibs;
using CommonServices;
using StockModels;

namespace MoneyMoat
{
    public static class RegServiceExtensions
    {
        //注册数据对象仓库
        public static IServiceCollection AddRegServices(this IServiceCollection services, IHostingEnvironment env, IConfigurationRoot config)
        {
            services.AddSingleton<EntityContainer<int, Financal, MainDbContext>>();
            services.AddSingleton<EntityContainer<int, FinStatement, MainDbContext>>();
            services.AddSingleton<EntityContainer<int, FinSummary, MainDbContext>>();
            services.AddSingleton<EntityContainer<int, FYEstimate, MainDbContext>>();
            services.AddSingleton<EntityContainer<int, Historical, MainDbContext>>();
            services.AddSingleton<EntityContainer<int, NPEstimate, MainDbContext>>();
            services.AddSingleton<EntityContainer<int, Recommendation, MainDbContext>>();
            services.AddSingleton<EntityContainer<int, Stock, MainDbContext>>();
            services.AddSingleton<EntityContainer<int, XueQiuQuote, MainDbContext>>();


			return services;
        }
    }
}
