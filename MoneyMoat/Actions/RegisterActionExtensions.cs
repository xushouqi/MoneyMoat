using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MoneyMoat.Actions;

namespace MoneyMoat
{
    public static class RegisterActionExtensions
    {
        public static IServiceCollection AddRegisterActions(this IServiceCollection services, IHostingEnvironment env, IConfigurationRoot config)
        {
            services.AddTransient<Action1001>();
            services.AddTransient<Action1002>();
            services.AddTransient<Action1003>();
            services.AddTransient<Action1004>();
            services.AddTransient<Action1005>();

            return services;
        }
    }
}
