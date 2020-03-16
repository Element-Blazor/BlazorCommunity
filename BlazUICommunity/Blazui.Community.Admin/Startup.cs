using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Blazui.Community.Model.Models;
using BlazAdmin.ServerRender;
using Microsoft.EntityFrameworkCore;
using Blazui.Community.Admin.Service;
using Blazui.Community.Admin.AutoConfiguration;
using AutoMapper;
using System;
using Microsoft.AspNetCore.Identity;

namespace Blazui.Community.Admin
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
            services.AddBlazAdmin<IdentityUser, AdminUserService, BlazUICommunityAdminDbContext>(null);
            services.AddScoped<AdminUserService>();
            services.ConfigureApplicationCookie(options =>
            {
                // Cookie settings
                options.Cookie.HttpOnly = true;
                options.ExpireTimeSpan = TimeSpan.FromMinutes(10);
                options.SlidingExpiration = true;
            });
            services.AddScoped<NetworkService>();
            services.AddTransient<ConfirmService>();
            services.AddAutoMapper(typeof(AutoMapConfiguration));
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
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
