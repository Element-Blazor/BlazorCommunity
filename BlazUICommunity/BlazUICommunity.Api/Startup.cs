using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Arch.EntityFrameworkCore.UnitOfWork;
using Autofac;
using Blazui.Community.Model.Models;
using Blazui.Community.Repository;
using Blazui.Community.Utility;
using Blazui.Community.Utility.Configure;
using Blazui.Community.Utility.Extensions;
using Blazui.Community.Utility.MiddleWare;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using NLog.Extensions.Logging;
using NLog.Web;
using AutoMapper;
using Blazui.Community.Utility.Jwt;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Components.Authorization;

namespace Blazui.Community.Api
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddTransient<LoggerMiddleware>();
            services.AddCustomAddControllers();
            services.AddCustomSwagger();
            services.AddCustomMemoryCache();
            services.AddDbContext<BlazUICommunityContext>(opt => opt.UseMySql(Configuration.GetConnectionString("DbConnectionString")))
                .AddUnitOfWork<BlazUICommunityContext>();//.AddCustomRepository<BZTopicModel , BZTopicRepository>(); ;
            //services.AddCustomRepository<BZUserModel , BZUserRepository>();
            //services.AddCustomRepository<BzVerifyCodeModel, BZVerifyCodeRepository>();
            //services.AddSingleton(typeof(JwtService));
            services.AddAutoMapper(typeof(AutoMapConfiguration));
            //services.AddJwtConfiguration(Configuration); 
            services.AddIdentity<BZUserModel, ApplicationRole>(
                options =>
                {
                    options.Password.RequiredLength = 6;
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
            .AddDefaultTokenProviders(); ;

        }
        /// <summary>
        /// 
        /// </summary>
        public IContainer AutofacContainer;
        /// <summary>
        /// ϵͳ����
        /// </summary>
        /// <param name="builder"></param>
        public void ConfigureContainer(ContainerBuilder builder)
        {
            builder.RegisterModule<CustomAutofacModule>();
        }
        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app , IWebHostEnvironment env , IHostApplicationLifetime appLitetime,ILoggerFactory factory)
        {
            if ( env.IsDevelopment() )
            {
                app.UseDeveloperExceptionPage();
            }

            //Console.WriteLine(Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT"));
            //app.UseHsts();
            //app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseCustomSwaggerUI(p => p.Title = "Blazui ���� WebApi Docs");

            app.UseRouting();

            app.UseLogMiddleware();
            //��֤
            app.UseAuthentication();
            //��Ȩ
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

            //����ֹͣ����autofac����
            appLitetime.ApplicationStopped.Register(() => { if ( AutofacContainer != null ) AutofacContainer.Dispose(); });
        }
    }
}
