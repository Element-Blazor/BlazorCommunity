using Arch.EntityFrameworkCore.UnitOfWork;
using Autofac;
using AutoMapper;
using Blazui.Community.App.Model;
using Blazui.Community.App.Service;
using Blazui.Community.AutofacModules;
using Blazui.Community.IdentityExtensions;
using Blazui.Community.Model.Models;
using Blazui.Community.Repository;
using Blazui.Component;
using Blazui.Markdown;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using NLog.LayoutRenderers;
using System;
using System.Collections.Generic;
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