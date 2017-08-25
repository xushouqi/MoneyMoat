using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MoneyModels;
using Foundatio.Caching;

namespace MoneyMoat
{
    public static class RepositoryServiceExtensions
    {
        //注册数据对象仓库
        public static IServiceCollection AddRepositoryService(this IServiceCollection services, IHostingEnvironment env, IConfigurationRoot config)
        {

            services.AddSingleton<IRepository<Stock>>(sp =>
            {
                var icache = sp.GetRequiredService<ICacheClient>();
                var context = sp.GetRequiredService<MoatDbContext>();
                return new Repository<Stock, MoatDbContext>(context, context.Stocks, icache);
            });
            services.AddSingleton<IRepository<Financal>>(sp =>
            {
                var icache = sp.GetRequiredService<ICacheClient>();
                var context = sp.GetRequiredService<TestDbContext>();
                return new Repository<Financal, TestDbContext>(context, context.Financals, icache);
            });

            return services;
        }
    }
}
