using System;
using System.Net.Http;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Element;
using Element.Markdown;
using BlazorCommunity.WasmApp.Service;
using Microsoft.AspNetCore.Components.Authorization;
using BlazorCommunity.WasmApp.Features.Identity;
using BlazorCommunity.WasmApp.Model;
using Blazored.LocalStorage;
using Microsoft.AspNetCore.Builder;
using BlazorCommunity.WasmApp.Middleware;
using Microsoft.JSInterop;

namespace BlazorCommunity.WasmApp
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebAssemblyHostBuilder.CreateDefault(args);
            var services = builder.Services;
            var Configuration = builder.Configuration;
            builder.RootComponents.Add<App>("app");

            services.AddHttpClient("ElementCommunitiyApp", client => client.BaseAddress = new Uri(builder.HostEnvironment.BaseAddress));
            services.AddBlazoredLocalStorage();
            services.AddScoped<AuthenticationStateProvider, ApiAuthenticationStateProvider>();
            services.AddScoped<IAuthenticationService, AuthenticationService>();
            services.AddScoped<ILocalStorageCacheService, LocalStorageCacheService>();
            services.AddElementServices();
            services.AddMarkdown();
            services.AddSingleton<BrowerService>();
            services.AddSingleton<NetworkService>();
            services.AddAuthorizationCore();
            services.Configure<TopNavMenuOption>(Configuration);
           
            await builder.Build().RunAsync();
        }

       
    }
}
