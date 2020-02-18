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
            services.AddDbContext<BlazUICommunityContext>(options => options.UseMySql(Configuration.GetConnectionString("DefaultConnection"))).AddUnitOfWork<BlazUICommunityContext>();

            AddIdentityVerify(services);
            services.AddMemoryCache();
            services.AddRazorPages();
            services.AddControllers();
            services.AddServerSideBlazor();
            services.AddBlazuiServices();
            services.AddMarkdown();
            services.AddCustomRepository<BZUserModel , BZUserIdentityRepository>();
            services.AddHttpClient("product" , client =>
            {
                client.BaseAddress = new Uri(Configuration["ServerUrl"]?? "http://localhost:5000");
            });
            
            services.AddAutoMapper(typeof(AutoMapConfiguration));
            services.AddScoped<NetworkService>();
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
        public void Configure(IApplicationBuilder app , IWebHostEnvironment env)
        {

            if ( env.IsDevelopment() )
            {
                app.UseDeveloperExceptionPage();
                app.UseDatabaseErrorPage();
            }
            else
            {
                app.UseExceptionHandler("/Error");
                app.UseHsts();
            }

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



        private static void AddIdentityVerify(IServiceCollection services)
        {
            services.AddScoped<AuthenticationStateProvider , RevalidatingIdentityAuthenticationStateProvider<BZUserModel>>();
            services.AddIdentity<BZUserModel , ApplicationRole>(options =>
            {
                options.Password.RequiredLength = 4;
                options.Password.RequireUppercase = false;
                options.Password.RequireLowercase = false;
                options.Password.RequireUppercase = false;
                options.Password.RequireNonAlphanumeric = false;
                options.SignIn.RequireConfirmedEmail = true;
                // Lockout settings.
                options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5);
                options.Lockout.MaxFailedAccessAttempts = 5;
                options.Lockout.AllowedForNewUsers = true;

                // User settings.
                options.User.AllowedUserNameCharacters =
                    "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789-._@+";
                options.User.RequireUniqueEmail = false;

            }).AddEntityFrameworkStores<BlazUICommunityContext>()
            .AddDefaultTokenProviders();
            services.Configure<CookiePolicyOptions>(options =>
            {
                options.CheckConsentNeeded = context => true;
                options.MinimumSameSitePolicy = SameSiteMode.None;
            });
            services.ConfigureApplicationCookie(options =>
            {
                // Cookie settings
                options.Cookie.HttpOnly = true;
                options.ExpireTimeSpan = TimeSpan.FromMinutes(10);

                options.LoginPath = "/account/signin";
                options.SlidingExpiration = true;
            });
            services.AddAuthorization(options =>
            {
                options.AddPolicy("RequireAdmin" , c => c.RequireRole("Admin"));

            });
        }
    }
}
