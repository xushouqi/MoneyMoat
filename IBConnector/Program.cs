using System;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Configuration.Json;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using NLog.Web.AspNetCore;
using NLog;
using NLog.Web;

namespace IBConnector
{
    class Program
    {
        public static IConfigurationRoot Configuration;

        static void Main(string[] args)
        {
            var builder = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);
            Configuration = builder.Build();

            IServiceCollection services = new ServiceCollection();
            var app = new MyApp(services);

            Console.WriteLine("Welcome to IB Connector!");

            while (true)
            {
                Console.ForegroundColor = ConsoleColor.White;
                Console.Write(">>");
                var cmd = Console.ReadLine().Trim().ToLower();
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine(cmd);
                if (cmd.Equals("quit"))
                    break;
            }
        }

        public class MyApp
        {
            public MyApp(IServiceCollection services)
            {
                ConfigureServices(services);

                var serviceProvider = services.BuildServiceProvider();

                Configure(serviceProvider);
            }

            private void ConfigureServices(IServiceCollection services)
            {
                services
                    .AddLogging()
                    .AddOptions();

                var spr = services.BuildServiceProvider();

                var settings = spr.GetRequiredService<IOptions<AppSettings>>().Value;

            }

            private void Configure(IServiceProvider serviceProvider)
            {
                //var nloger = serviceProvider.GetService<ILoggerFactory>().AddNLog();

            }
        }

    }
}
