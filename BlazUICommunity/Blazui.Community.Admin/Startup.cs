
using AspectCore.Configuration;
using AspectCore.Extensions.DependencyInjection;
using Blazui.Admin.ServerRender;
using Blazui.Community.Admin.Service;
using Blazui.Community.Model.Models;
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

namespace Blazui.Community.Admin
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
            services.AddDbContext<BlazUICommunityAdminDbContext>(
                options => options.UseMySql(Configuration.GetConnectionString("DbConnectionString")));
            services.AddHttpClient("BlazuiCommunitiyAdmin",
                client => client.BaseAddress = new Uri(Configuration["ServerUrl"]));
            services.AddBlazuiServicesAsync().Wait();

            services.AddScoped<DbContext, BlazUICommunityAdminDbContext>();
            services.AddAdmin<IdentityUser, AdminUserService, BlazUICommunityAdminDbContext>(null);
            services.AddTransient<ConfirmService>();
            services.AddScoped<AdminUserService>();
            services.ConfigureApplicationCookie(options =>
            {
                options.Cookie.HttpOnly = true;
                options.ExpireTimeSpan = TimeSpan.FromMinutes(30);
                options.SlidingExpiration = true;
            });
            services.AddScoped<NetworkService>();
         
            //��������ע��������ȫ��������
            //services.ConfigureDynamicProxy(config =>
            //{
            //    config.Interceptors.AddTyped<SuperAdminAuthorizeAttribute>();//CustomInterceptorAttribute�������Ҫȫ�����ص�������
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