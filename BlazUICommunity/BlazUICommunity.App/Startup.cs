using Blazui.Component;
using Blazui.Community.App.Features.Identity;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Blazui.Community.Model.Models;
using Arch.EntityFrameworkCore.UnitOfWork;
using Blazui.Community.Repository;
using Autofac;
using Blazui.Community.Utility;
using Blazui.Markdown;
using System;
using Blazui.Community.App.Service;
using Microsoft.AspNetCore.Http;
using AutoMapper;
using Microsoft.Extensions.Logging;
using NLog.Extensions.Logging;
using Blazui.Community.DTO;
using System.IO;
using Blazui.Community.Enums;
using Blazui.Community.Utility.Configure;
using NLog.LayoutRenderers;
using System.Collections.Generic;
using Blazui.Community.App.Model;
using System.Text;

namespace Blazui.Community.App
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
            services.AddDbContext<BlazUICommunityContext>(options => options.UseMySql(Configuration.GetConnectionString("DbConnectionString"))).AddUnitOfWork<BlazUICommunityContext>();
            services.AddHttpClient("BlazuiCommunitiyApp", client => client.BaseAddress = new Uri(Configuration["ServerUrl"] ?? throw new ArgumentNullException("ServerUrl is null")));
            services.AddCustomAspIdenitty<BZUserModel, BlazUICommunityContext>();

            services.ConfigureApplicationCookie(options =>
            {
                // Cookie settings
                options.Cookie.HttpOnly = true;
                options.ExpireTimeSpan = TimeSpan.FromMinutes(30);
                options.SlidingExpiration = false;
            });

            services.AddMemoryCache();
            services.AddRazorPages();
            services.AddControllers();
            services.AddServerSideBlazor();
            services.AddBlazuiServices();
            services.AddMarkdown();
            services.AddCustomRepository<BZUserModel, BZUserIdentityRepository>();
            services.AddAutoMapper(typeof(AutoMapConfiguration));
            services.AddScoped<NetworkService>();
            services.AddScoped<TokenService>();
            services.AddOptions<List<HeaderMenu>>().Configure(options => Configuration.GetSection("HeaderMenus").Bind(options));
        }



        /// <summary>
        /// 
        /// </summary>
        public IContainer AutofacContainer;
        /// <summary>
        /// 系统调用
        /// </summary>
        /// <param name="builder"></param>
        public void ConfigureContainer(ContainerBuilder builder)
        {
            builder.RegisterModule<CustomAutofacModule>();
        }
        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseDatabaseErrorPage();
            }
            else
            {
                app.UseExceptionHandler("/Error");
                app.UseHsts();
            }
            LayoutRenderer.Register("basedir", p => env.ContentRootPath);
            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
                endpoints.MapBlazorHub();
                endpoints.MapFallbackToPage("/_Host");
            });
        }
    }
}
