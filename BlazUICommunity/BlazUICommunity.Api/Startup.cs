using Arch.EntityFrameworkCore.UnitOfWork;
using Autofac;
using AutoMapper;
using Blazui.Community.Api.Extensions;
using Blazui.Community.Api.Jwt;
using Blazui.Community.Api.Options;
using Blazui.Community.Api.Service;
using Blazui.Community.AutofacModules;
using Blazui.Community.Enums;
using Blazui.Community.IdentityExtensions;
using Blazui.Community.JWTServiceCollectionExtensions;
using Blazui.Community.Model.Models;
using Blazui.Community.MvcCore;
using Blazui.Community.SwaggerExtensions;
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
using Microsoft.AspNetCore.Http;
using static Blazui.Community.Api.Configuration.ConstantConfiguration;
using Blazui.Community.Api.Middleware;

namespace Blazui.Community.Api
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
            services.AddCustomDbService(Configuration);
            services.AddCustomWebApi(Configuration);
            services.AddResponseCaching();
            services.AddCustomServices();
            services.AddCustomConfigure(Configuration);
            services.AddRazorPages();
        }

        /// <summary>
        /// ϵͳ����
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
            //����
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
            //��֤
            app.UseAuthentication();
            //��Ȩ
            app.UseAuthorization();

            app.UseCors(PolicyName);
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapRazorPages();
                endpoints.MapControllers();
                endpoints.MapFallbackToFile("index.html");
            });

            env.CreateUplodFolder();
            appLitetime.DisposeAutofacContainerWhenAppStopped(AutofacContainer);
        }

    }
}