using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Arch.EntityFrameworkCore.UnitOfWork;
using Autofac;
using BlazUICommunity.Model.Models;
using BlazUICommunity.Repository;
using BlazUICommunity.Utility;
using BlazUICommunity.Utility.Configure;
using BlazUICommunity.Utility.Extensions;
using BlazUICommunity.Utility.MiddleWare;
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

namespace BlazUICommunity.Api
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
            services.AddDbContext<BlazUICommunityContext>(opt => opt.UseMySql(Configuration.GetConnectionString("DbConnectionString")))
                .AddUnitOfWork<BlazUICommunityContext>();
     
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
        public void Configure(IApplicationBuilder app , IWebHostEnvironment env , IHostApplicationLifetime appLitetime)
        {
            if ( env.IsDevelopment() )
            {
                app.UseDeveloperExceptionPage();
            }
            Console.WriteLine(Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT"));
            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseCustomSwaggerUI(p => p.Title = "Blazui 社区 WebApi Docs");

            app.UseRouting();

            app.UseLogMiddleware();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

            //程序停止调用autofac函数
            appLitetime.ApplicationStopped.Register(() => { if ( AutofacContainer != null ) AutofacContainer.Dispose(); });
        }
    }
}
