using System;
using Arch.EntityFrameworkCore.UnitOfWork;
using Autofac;
using Blazui.Community.Model.Models;
using Blazui.Community.Utility;
using Blazui.Community.Utility.Configure;
using Blazui.Community.Utility.Extensions;
using Blazui.Community.Utility.MiddleWare;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using AutoMapper;
using Blazui.Community.Api.Service;
using System.IO;
using Blazui.Community.Api.Configuration;
using Blazui.Community.Utility.Jwt;
using Blazui.Community.Enums;
using NLog.LayoutRenderers;
using static Blazui.Community.Api.Configuration.ConstantConfiguration;
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
            services.AddDbContext<BlazUICommunityContext>(opt => opt.UseMySql(Configuration.GetConnectionString("DbConnectionString"))).AddUnitOfWork<BlazUICommunityContext>();

            services.AddCustomAddControllers();
            services.AddMvc();
            services.AddCustomCors(Configuration.GetSection("AllowOrigins"));
            services.AddCustomSwagger();

            services.AddAutoMapper(typeof(AutoMapConfiguration));

            services.AddCustomAspIdenitty<BZUserModel, BlazUICommunityContext>();

            services.AddResponseCaching();
            services.AddCustomMemoryCache();

            services.AddScoped<ICacheService, CacheService>();
            services.AddScoped<IMessageService, MessageService>();
            services.AddScoped<ICodeService, CodeService>();
            services.AddScoped<ISmtpClientService, SmtpClientService>();

            services.AddScoped<JwtService>();
            services.AddJwtConfiguration(Configuration);

            services.Configure<EmailConfiguration>(Configuration.GetSection("EmailSetting"));

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
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, IHostApplicationLifetime appLitetime, ILoggerFactory factory)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            app.UseResponseCaching();

            //Console.WriteLine(Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT"));
            //app.UseHsts();
            //app.UseHttpsRedirection();
            app.UseStaticFiles();

            LayoutRenderer.Register("basedir", p => env.ContentRootPath);
            app.UseCustomSwaggerUI(p => p.Title = "Blazui 社区 WebApi Docs");

            app.UseRouting();

            app.UseLogMiddleware();
            //认证
            app.UseAuthentication();
            //授权
            app.UseAuthorization();
            app.UseCors("any");
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

            foreach (UploadPath path in Enum.GetValues(typeof(UploadPath)))
            {
                var physicsPath = Path.Combine(env.WebRootPath, UploadRootPath, path.Description());
                if (!Directory.Exists(physicsPath))
                    Directory.CreateDirectory(physicsPath);
            }

            //程序停止调用autofac函数
            appLitetime.ApplicationStopped.Register(() => { if (AutofacContainer != null) AutofacContainer.Dispose(); });
        }
    }
}
