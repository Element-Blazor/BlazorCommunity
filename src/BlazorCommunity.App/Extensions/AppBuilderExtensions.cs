using Element.Markdown;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Element;
using BlazorCommunity.App.Service;

namespace BlazorCommunity.App.Extensions
{
    public static class AppBuilderExtensions
    {
        public static IServiceCollection AddElement(this IServiceCollection services)
        {
             services.AddElementServices();
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
