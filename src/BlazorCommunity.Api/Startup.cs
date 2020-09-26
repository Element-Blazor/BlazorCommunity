using Arch.EntityFrameworkCore.UnitOfWork;
using Autofac;
using AutoMapper;
using BlazorCommunity.Api.Extensions;
using BlazorCommunity.Api.Jwt;
using BlazorCommunity.Api.Options;
using BlazorCommunity.Api.Service;
using BlazorCommunity.AutofacModules;
using BlazorCommunity.Enums;
using BlazorCommunity.IdentityExtensions;
using BlazorCommunity.JWTServiceCollectionExtensions;
using BlazorCommunity.Model.Models;
using BlazorCommunity.MvcCore;
using BlazorCommunity.SwaggerExtensions;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.ResponseCompression;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using NLog.LayoutRenderers;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Encodings.Web;
using System.Text.Unicode;
using System.Threading.Tasks;
using Blazored.LocalStorage;
using Microsoft.AspNetCore.Http;
using static BlazorCommunity.Api.Configuration.ConstantConfiguration;
using BlazorCommunity.Api.Middleware;
using BlazorCommunity.WasmApp.Features.Identity;
using BlazorCommunity.WasmApp.Service;
using Element;
using Element.Markdown;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.JSInterop;

namespace BlazorCommunity.Api
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        /// <summary>
        ///
        /// </summary>
        public IContainer AutofacContainer;

        public void ConfigureServices(IServiceCollection services)
        {
            //services.AddMvc();
            services.AddRazorPages();
            services.AddCustomDbService(Configuration);
            services.AddCustomWebApi(Configuration);
            services.AddResponseCaching();
            services.AddCustomServices();
            services.AddCustomConfigure(Configuration);

            //services.AddSingleton(HtmlEncoder.Create(UnicodeRanges.All));

            //services.AddHttpClient("ElementCommunitiyApp", client => client.BaseAddress = new Uri("http://localhost:5000/"));

            //services.AddServerSideBlazor();
            //services.AddBlazoredLocalStorage();
            //services.AddScoped<ILocalStorageCacheService, LocalStorageCacheService>();
            //services.AddScoped<AuthenticationStateProvider, ApiAuthenticationStateProvider>();
            //services.AddScoped<IAuthenticationService, AuthenticationService>();
            //services.AddScoped<NetworkService>();
            //services.AddSingleton<BrowerService>();
            //services.AddElementServicesAsync().Wait();
            //services.AddMarkdown();

        }

        /// <summary>
        /// 系统调用
        /// </summary>
        /// <param name="builder"></param>
        public void ConfigureContainer(ContainerBuilder builder)
        {
            builder.RegisterModule<CustomAutofacModule>();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, IHostApplicationLifetime appLitetime)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            //缓存
            app.UseResponseCompression();
            app.UseMiddleware<SeoMiddleware>();
            app.UseLogMiddleware();

            //app.UseHsts();
            //app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseBlazorFrameworkFiles();

            app.UseResponseCaching();

            //app.UsePrecompressedPrecompressedBlazor();
            
            LayoutRenderer.Register("basedir", p => env.ContentRootPath);
            app.UseCustomSwaggerUI(p => p.Title = "  WebApi Docs");

            //app.UseHttpCacheHeaders();


            app.UseRouting();
            //认证
            app.UseAuthentication();
            //授权
            app.UseAuthorization();

            app.UseCors(PolicyName);
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapRazorPages();
                endpoints.MapControllers();
                endpoints.MapFallbackToFile("/index.html");
            });

            env.CreateUplodFolder();
            appLitetime.DisposeAutofacContainerWhenAppStopped(AutofacContainer);
        }

    }
}