using System;
using System.Collections.Generic;
using System.Reflection;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using NLog.Extensions.Logging;
using NLog.Web;
using IBApi;
using CommonLibs;
using IBConnector.Services;

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

            services.Configure<AppSettings>((settings)=> {
                settings.GatewayHost = Configuration.GetSection("GatewayHost").Value;
                settings.GatewayPort = int.Parse(Configuration.GetSection("GatewayPort").Value);
                settings.FundamentalPath = Configuration.GetSection("FundamentalPath").Value;
            });

            var app = new MyApp(services);

            Console.WriteLine("Welcome to IB Connector!");

            var fundService = app.ServiceProvider.GetRequiredService<FundamentalService>();
            //var symbolService = app.ServiceProvider.GetRequiredService<SymbolService>();
            //var data = fundService.UpdateAllFromIB("CTRP", "SMART", false);

            while (true)
            {
                Console.ForegroundColor = ConsoleColor.White;
                Console.Write(">>");
                var line = Console.ReadLine().Trim().ToLower();
                Console.ForegroundColor = ConsoleColor.Green;
                //Console.WriteLine(cmd);
                if (line.Equals("quit"))
                    break;
                else
                {
                    var arlist = line.Split(" ");
                    var servicePrefix = MethodBase.GetCurrentMethod().DeclaringType.Namespace + ".Services";
    
                    //当前程序集
                    var curAssem = Assembly.GetEntryAssembly();
                    var types = curAssem.GetTypes();
                    for (int i = 0; i < types.Length; i++)
                    {
                        var ctype = types[i];
                        var typeInfo = ctype.GetTypeInfo();
                        //api服务类
                        if (typeInfo.IsDefined(typeof(WebApiAttribute)))
                        {
                            var attr = (WebApiAttribute)typeInfo.GetCustomAttribute(typeof(WebApiAttribute), false);
                            //命令匹配服务
                            if (attr.CmdName.Equals(arlist[0]))
                            {
                                if (arlist.Length > 1)
                                {
                                    var cmd = arlist[1];
                                    //所有方法
                                    var methods = typeInfo.GetMethods();
                                    for (int j = 0; j < methods.Length; j++)
                                    {
                                        var method = methods[j];
                                        if (method.IsDefined(typeof(ApiAttribute)))
                                        {
                                            var matt = (ApiAttribute)method.GetCustomAttribute(typeof(ApiAttribute));
                                            //api方法
                                            if (matt.CmdName.Equals(cmd))
                                            {
                                                //获取服务实例
                                                var inst = app.ServiceProvider.GetRequiredService(ctype);
                                                bool canInvoke = true;
                                                var parList = new List<object>();
                                                //所有参数
                                                var mParams = method.GetParameters();
                                                for (int m = 0; m < mParams.Length; m++)
                                                {
                                                    var par = mParams[m];
                                                    //命令行中的参数
                                                    if (arlist.Length > m + 2)
                                                        parList.Add(arlist[m + 2]);
                                                    //用默认值
                                                    else if (par.HasDefaultValue)
                                                        parList.Add(par.DefaultValue);
                                                    else
                                                    {
                                                        canInvoke = false;
                                                        break;
                                                    }
                                                }
                                                if (canInvoke)
                                                    method.Invoke(inst, parList.ToArray());
                                                else
                                                    ConsoleWriteAlert("Wrong parameters!");
                                                break;
                                            }
                                        }
                                    }
                                }
                                else
                                    ConsoleWriteAlert("No command!");
                            }
                        }
                    }
                }
            }
        }

        static void ConsoleWriteAlert(string line)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine(line);
            Console.ForegroundColor = ConsoleColor.Green;
        }

        public class MyApp
        {
            public ServiceProvider ServiceProvider;

            public MyApp(IServiceCollection services)
            {
                ConfigureServices(services);

                ServiceProvider = services.BuildServiceProvider();

                Configure(ServiceProvider);
            }

            private void ConfigureServices(IServiceCollection services)
            {
                services
                    .AddLogging()
                    .AddOptions();

                services.AddSingleton<CommonManager>();

                services.AddSingleton(new IBClient(new EReaderMonitorSignal()));
                services.AddSingleton<IBManager>();
                services.AddTransient<FundamentalService>();

                var spr = services.BuildServiceProvider();
                var settings = spr.GetRequiredService<IOptions<AppSettings>>().Value;

            }

            private void Configure(IServiceProvider serviceProvider)
            {
                var nloger = serviceProvider.GetService<ILoggerFactory>().AddNLog();

            }
        }

    }
}
