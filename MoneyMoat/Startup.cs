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
            //services.AddDbContext<MoneyDbContext>(opt =>
            //        opt.UseMySql(connstr), ServiceLifetime.Scoped);

            // Add framework services.
            services.AddMvc();

            services.AddSingleton(new IBClient(new EReaderMonitorSignal()));
            services.AddSingleton<IBManager>();
            services.AddSingleton<AccountService>();
            services.AddSingleton<SymbolSamplesService>();
            services.AddSingleton<FundamentalService>();
            services.AddSingleton<HistoricalService>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            loggerFactory
                .AddConsole(Configuration.GetSection("Logging"))
                .AddDebug()
                .AddNLog();

            app.UseMvc();

            System.Threading.Thread.Sleep(500);

            var settings = app.ApplicationServices.GetRequiredService<IOptions<AppSettings>>().Value;
            var ibManager = app.ApplicationServices.GetRequiredService<IBManager>();

            ibManager.Connect();
            ibManager.Test();
        }
    }
}
