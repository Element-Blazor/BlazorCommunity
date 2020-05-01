using Arch.EntityFrameworkCore.UnitOfWork;
using Autofac;
using AutoMapper;
using Blazui.Community.Api.Jwt;
using Blazui.Community.Api.Options;
using Blazui.Community.Api.Service;
using Blazui.Community.AutofacModules;
using Blazui.Community.Enums;
using Blazui.Community.IdentityExtensions;
using Blazui.Community.JWTServiceCollectionExtensions;
using Blazui.Community.Model.Models;
using Blazui.Community.MvcCore;
using Blazui.Community.SwaggerExtensions;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.ResponseCompression;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using NLog.LayoutRenderers;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
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

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<BlazUICommunityContext>(opt =>
            opt.UseMySql(Configuration.GetConnectionString("DbConnectionString"))).AddUnitOfWork<BlazUICommunityContext>()
                .AddCustomAddControllers()
                .AddCustomCors(GetAllowOrigins(), PolicyName)
                .AddCustomSwagger()
                .AddHttpContextAccessor()
                .AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies())
                .AddMemoryCache(p => p.ExpirationScanFrequency = TimeSpan.FromSeconds(100))
                .AddCustomAspIdenitty<BZUserModel, BlazUICommunityContext>();

            services.AddScoped<JwtService>()
                    .AddJwtConfiguration(Configuration);

            services.AddScoped<ICacheService, CacheService>()
                    .AddScoped<IMessageService, MessageService>()
                    .AddScoped<ICodeService, CodeService>()
                    .AddScoped<ISmtpClientService, SmtpClientService>()
                    .AddTransient<ImgCompressService>()
                    .Configure<EmailStmpOptions>(Configuration.GetSection("EmailSetting"))
                    .Configure<EmailNoticeOptions>(Configuration)
                    .Configure<BaseDomainOptions>(Configuration);
            //services.AddOptions<EmailNoticeOptions>().Configure(option => Configuration.Bind(option));

            services.AddResponseCompression(opts => {
                opts.MimeTypes = ResponseCompressionDefaults.MimeTypes.Concat(
                    new[] { "application/octet-stream" });
            });
            string[] GetAllowOrigins()
            {
                var AllowOrigins = new List<string>();
                Configuration.GetSection("AllowOrigins").Bind(AllowOrigins);
                return AllowOrigins.ToArray();
            }
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
            //app.UseResponseCaching();
            //app.UseHttpCacheHeaders();


            app.UseRouting();
            //认证
            app.UseAuthentication();
            //授权
            app.UseAuthorization();
            app.UseCors(PolicyName);
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