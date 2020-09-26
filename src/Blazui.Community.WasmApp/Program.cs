using System;
using System.Net.Http;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Blazui.Component;
using Blazui.Markdown;
using Blazui.Community.WasmApp.Service;
using Microsoft.AspNetCore.Components.Authorization;
using Blazui.Community.WasmApp.Features.Identity;
using Blazui.Community.WasmApp.Model;
using Blazored.LocalStorage;
using Microsoft.AspNetCore.Builder;
using Blazui.Community.WasmApp.Middleware;
using Microsoft.JSInterop;

namespace Blazui.Community.WasmApp
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebAssemblyHostBuilder.CreateDefault(args);
            var services = builder.Services;
            var Configuration = builder.Configuration;
            builder.RootComponents.Add<App>("app");

            services.AddHttpClient("BlazuiCommunitiyApp", client => client.BaseAddress = new Uri(builder.HostEnvironment.BaseAddress));
            services.AddBlazoredLocalStorage();
            services.AddScoped<AuthenticationStateProvider, ApiAuthenticationStateProvider>();
            services.AddScoped<IAuthenticationService, AuthenticationService>();
            services.AddScoped<ILocalStorageCacheService, LocalStorageCacheService>();
            await services.AddBlazuiServicesAsync();
            services.AddMarkdown();
            services.AddSingleton<BrowerService>();
            services.AddSingleton<NetworkService>();
            services.AddAuthorizationCore();
            services.Configure<TopNavMenuOption>(Configuration);
           
            await builder.Build().RunAsync();
        }

       
    }
}
