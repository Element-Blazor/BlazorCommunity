using Blazui.Markdown;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Blazui.Component;
using Blazui.Community.App.Service;

namespace Blazui.Community.App.Extensions
{
    public static class AppBuilderExtensions
    {
        public static async Task<IServiceCollection> AddBlazui(this IServiceCollection services)
        {
            await services.AddBlazuiServicesAsync();
            services.AddMarkdown();
            return services;
        }

        public static IServiceCollection AddCustomService(this IServiceCollection services)
        {
            return services.AddScoped<NetworkService>()
            .AddScoped<TokenService>()
            .AddSingleton<BrowerService>();
        }
    }
}
