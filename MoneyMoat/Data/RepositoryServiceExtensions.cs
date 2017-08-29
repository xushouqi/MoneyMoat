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
            services.AddScoped<IRepository<Stock>, Repository<Stock, MoatDbContext>>();
            services.AddScoped<IRepository<Financal>, Repository<Financal, MoatDbContext>>();
            services.AddScoped<IRepository<FYEstimate>, Repository<FYEstimate, MoatDbContext>>();
            services.AddScoped<IRepository<NPEstimate>, Repository<NPEstimate, MoatDbContext>>();
            services.AddScoped<IRepository<Recommendation>, Repository<Recommendation, MoatDbContext>>();
            services.AddScoped<IRepository<XueQiuData>, Repository<XueQiuData, MoatDbContext>>();
            services.AddScoped<IRepository<XueQiuQuote>, Repository<XueQiuQuote, MoatDbContext>>();

            //services.AddSingleton<IRepository<Stock>>(sp =>
            //{
            //    var icache = sp.GetRequiredService<ICacheClient>();
            //    var context = sp.GetRequiredService<MoatDbContext>();
            //    return new Repository<Stock, MoatDbContext>(context, icache);
            //});
            //services.AddSingleton<IRepository<Financal>>(sp =>
            //{
            //    var icache = sp.GetRequiredService<ICacheClient>();
            //    var context = sp.GetRequiredService<TestDbContext>();
            //    return new Repository<Financal, TestDbContext>(context,icache);
            //});

            return services;
        }
    }
}
