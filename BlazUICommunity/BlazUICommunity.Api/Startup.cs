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

            services.AddCustomRepository<BZUserModel , BZUserRepository>();
            services.AddSingleton(typeof(JwtService));
            services.AddAutoMapper(typeof(AutoMapConfiguration));
            services.AddJwtConfiguration(Configuration); 
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
