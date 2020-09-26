
using AspectCore.Configuration;
using AspectCore.Extensions.DependencyInjection;
using Blazui.Admin.ServerRender;
using BlazorCommunity.Admin.Service;
using BlazorCommunity.AdminDbContext;
using BlazorCommunity.Model.Models;
using Blazui.Component;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Threading.Tasks;

namespace BlazorCommunity.Admin
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }


        public void ConfigureServices(IServiceCollection services)
        {
            services.AddRazorPages();
            services.AddServerSideBlazor();
            services.AddDbContext<BlazorCommunityAdminDbContext>(
                options => options.UseMySql(Configuration.GetConnectionString("DbConnectionString")));
            services.AddHttpClient("BlazuiCommunitiyAdmin",
                client => client.BaseAddress = new Uri(Configuration["ServerUrl"]));
            services.AddBlazuiServicesAsync().Wait();

            services.AddScoped<DbContext, BlazorCommunityAdminDbContext>();
            services.AddAdmin<IdentityUser, AdminUserService, BlazorCommunityAdminDbContext>(null);
            services.AddTransient<ConfirmService>();
            services.AddScoped<AdminUserService>();
            services.ConfigureApplicationCookie(options =>
            {
                options.Cookie.HttpOnly = true;
                options.ExpireTimeSpan = TimeSpan.FromMinutes(30);
                options.SlidingExpiration = true;
            });
            services.AddScoped<NetworkService>();
         
            //根据属性注入来配置全局拦截器
            //services.ConfigureDynamicProxy(config =>
            //{
            //    config.Interceptors.AddTyped<SuperAdminAuthorizeAttribute>();//CustomInterceptorAttribute这个是需要全局拦截的拦截器
            //});
            //services.ConfigureDynamicProxy();
            //services.BuildDynamicProxyProvider();
        }


        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Error");
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthorization();
            app.UseAuthentication();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapDefaultControllerRoute();
                endpoints.MapBlazorHub();
                endpoints.MapFallbackToPage("/_Host");
            });
        }
    }
}