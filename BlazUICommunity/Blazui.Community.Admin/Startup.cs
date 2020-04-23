using Blazui.Community.Admin.Service;
using Blazui.Community.Model.Models;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using Blazui.Component;
using Blazui.Admin.ServerRender;

namespace Blazui.Community.Admin
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

     
        public async void ConfigureServices(IServiceCollection services)
        {
            services.AddRazorPages();
            services.AddServerSideBlazor();
            services.AddDbContext<BlazUICommunityAdminDbContext>(options => options.UseMySql(Configuration.GetConnectionString("DbConnectionString")));
            services.AddHttpClient("BlazuiCommunitiyAdmin", client =>
           {
               client.BaseAddress = new Uri(Configuration["ServerUrl"] ?? throw new ArgumentNullException("ServerUrl"));
               client.DefaultRequestHeaders.CacheControl = new System.Net.Http.Headers.CacheControlHeaderValue()
               {
                   NoCache = false,
                   NoStore = false,
                   MaxAge = TimeSpan.FromSeconds(0),
                   MustRevalidate = false,
                   Public = false,
               };
           });
            await services.AddBlazuiServicesAsync();
            services.AddScoped<DbContext, BlazUICommunityAdminDbContext>();
            services.AddAdmin<IdentityUser, AdminUserService, BlazUICommunityAdminDbContext>(null);
            services.AddScoped<AdminUserService>();
            services.ConfigureApplicationCookie(options =>
            {
                // Cookie settings
                options.Cookie.HttpOnly = true;
                options.ExpireTimeSpan = TimeSpan.FromMinutes(30);
                options.SlidingExpiration = true;
            });
            services.AddScoped<NetworkService>();
            services.AddTransient<ConfirmService>();
            //services.AddAutoMapper(typeof(AutoMapConfiguration));
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
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
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