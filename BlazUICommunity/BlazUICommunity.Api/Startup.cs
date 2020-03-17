using System;
using Arch.EntityFrameworkCore.UnitOfWork;
using Autofac;
using Blazui.Community.Model.Models;
using Blazui.Community.Utility;
using Blazui.Community.Utility.Configure;
using Blazui.Community.Utility.Extensions;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using AutoMapper;
using Blazui.Community.Api.Service;
using System.IO;
using Blazui.Community.Api.Configuration;
using Blazui.Community.Utility.Jwt;
using Blazui.Community.Enums;
using NLog.LayoutRenderers;
using Marvin.Cache.Headers;
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
            services.AddHttpContextAccessor();
            //services.AddResponseCaching();
            services.AddHttpCacheHeaders(
                expires =>
                {
                    expires.MaxAge = 60;
                    expires.CacheLocation = CacheLocation.Public;
                },
                validation => { validation.MustRevalidate = true; }
                );

            services.AddTransient<LoggerMiddleware>();
            services.AddDbContext<BlazUICommunityContext>(opt => opt.UseMySql(Configuration.GetConnectionString("DbConnectionString"))).AddUnitOfWork<BlazUICommunityContext>();

            services.AddCustomAddControllers();
            services.AddCustomCors(Configuration.GetSection("AllowOrigins"));
            services.AddCustomSwagger();

            services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

            services.AddCustomAspIdenitty<BZUserModel, BlazUICommunityContext>();

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
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, IHostApplicationLifetime appLitetime)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseLogMiddleware();//必须放在UseResponseCaching，UseHttpCacheHeaders 前面，否则Etag不生成，原因未知

            //app.UseHsts();
            //app.UseHttpsRedirection();
            app.UseStaticFiles();
            LayoutRenderer.Register("basedir", p => env.ContentRootPath);
            app.UseCustomSwaggerUI(p => p.Title = "Blazui 社区 WebApi Docs");

            //缓存
            app.UseResponseCaching();
            app.UseHttpCacheHeaders();

            app.UseRouting();
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
