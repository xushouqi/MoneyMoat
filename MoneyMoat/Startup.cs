using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.EntityFrameworkCore;
using NLog.Extensions.Logging;
using NLog.Web;
using Pomelo.EntityFrameworkCore;
using Pomelo.EntityFrameworkCore.MySql;
using MoneyMoat.Services;
using IBApi;
using Foundatio.Caching;


namespace MoneyMoat
{
    public class Startup
    {
        public Startup(IHostingEnvironment env)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                //.AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true)
                .AddEnvironmentVariables();
            Configuration = builder.Build();

            env.ConfigureNLog("nlog.config");
            Environment = env;
        }

        public IHostingEnvironment Environment { get; }
        public IConfigurationRoot Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.Configure<AppSettings>(Configuration);

            string connstr = Configuration.GetConnectionString("MySQL");
            services.AddDbContextPool<MoatDbContext>(opt =>
                    opt.UseMySql(connstr));

            services.AddSingleton<CommonManager>();

            //缓存cache
            services.AddSingleton<ICacheClient, InMemoryCacheClient>();
            //注册数据仓库（生成代码）
            services.AddRepositoryService(Environment, Configuration);

            services.AddSingleton(new IBClient(new EReaderMonitorSignal()));
            services.AddSingleton<IBManager>();
            services.AddSingleton<AccountService>();
            services.AddSingleton<SymbolService>();
            services.AddSingleton<FundamentalService>();
            services.AddSingleton<HistoricalService>();
            services.AddSingleton<ScannerService>();
            services.AddSingleton<AnalyserService>();

            // Add framework services.
            services.AddMvc();

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            loggerFactory
                .AddConsole(Configuration.GetSection("Logging"))
                .AddDebug()
                .AddNLog();

            app.UseMvc();

            var services = app.ApplicationServices;
            var settings = services.GetRequiredService<IOptions<AppSettings>>().Value;

            var ibManager = (IBManager)services.GetService(typeof(IBManager));
            ibManager.Connect();
            ibManager.Work();
        }
    }
}
