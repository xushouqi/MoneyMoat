using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using AutoMapper;
using CommonLibs;
using StockModels;
using StockModels.ViewModels;

namespace MoneyMoat
{
    public static class AutoMapperExtensions
    {
        public static IServiceCollection AddMapperModels(this IServiceCollection services, IHostingEnvironment env, IConfigurationRoot config)
        {
            Mapper.Initialize(cfg =>
            {
                cfg.CreateMap<Financal, FinancalData>();
                cfg.CreateMap<FinStatement, FinStatementData>();
                cfg.CreateMap<FinSummary, FinSummaryData>();
                cfg.CreateMap<FYEstimate, FYEstimateData>();
                cfg.CreateMap<Historical, HistoricalData>();
                cfg.CreateMap<NPEstimate, NPEstimateData>();
                cfg.CreateMap<Recommendation, RecommendationData>();
                cfg.CreateMap<Stock, StockData>();
                cfg.CreateMap<XueQiuQuote, XueQiuQuoteData>();

            });
            return services;
        }
    }
}
