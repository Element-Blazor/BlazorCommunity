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

            services.AddTransient(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });
            services.AddHttpClient("BlazuiCommunitiyApp", client =>
             client.BaseAddress = new Uri(Configuration["ServerUrl"]));
            services.AddBlazoredLocalStorage();
            services.AddAuthorizationCore();
            services.AddScoped<AuthenticationStateProvider, ApiAuthenticationStateProvider>();
            services.AddScoped<IAuthenticationService, AuthenticationService>();
            services.AddScoped<NetworkService>();
            //services.AddTransient<SeoMiddleware>();
            await services.AddBlazuiServicesAsync();
            services.AddMarkdown();
            services.AddOptions<List<TopNaviHeaderMenuModel>>().Configure(options => Configuration.GetSection("HeaderMenus").Bind(options));

            await builder.Build().RunAsync();
        }
    }
}
