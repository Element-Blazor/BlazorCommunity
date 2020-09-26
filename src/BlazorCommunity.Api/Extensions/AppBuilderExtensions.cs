using Arch.EntityFrameworkCore.UnitOfWork;
using BlazorCommunity.Api.Jwt;
using BlazorCommunity.Api.Options;
using BlazorCommunity.Api.Service;
using BlazorCommunity.IdentityExtensions;
using BlazorCommunity.JWTServiceCollectionExtensions;
using BlazorCommunity.Model.Models;
using BlazorCommunity.MvcCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using BlazorCommunity.SwaggerExtensions;
using AutoMapper;
using static BlazorCommunity.Api.Configuration.ConstantConfiguration;
using Microsoft.AspNetCore.Hosting;
using BlazorCommunity.Enums;
using System.IO;
using Microsoft.Extensions.Hosting;
using Autofac;
using BlazorCommunity.AppDbContext;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.ResponseCompression;
using System.Linq;

namespace BlazorCommunity.Api.Extensions
{
    public static class AppBuilderExtensions
    {

        public static IServiceCollection AddCustomServices(this IServiceCollection services)
        {
            return services.AddScoped<JwtService>()
                  .AddScoped<ICacheService, CacheService>()
                  .AddScoped<IMessageService, MessageService>()
                  .AddScoped<ICodeService, CodeService>()
                  .AddScoped<ISmtpClientService, SmtpClientService>()
                  .AddScoped<ImgCompressService>()
                  .AddScoped<CQService>();
        }
        public static IServiceCollection AddCustomWebApi(this IServiceCollection services, IConfiguration Configuration)
        {
            services.AddHttpClient("CQService",
              client => client.BaseAddress = new Uri(Configuration.GetSection("CQHTTP").GetSection("ApiUrl").Value));
            services.AddResponseCompression(opts =>
            {
                opts.MimeTypes = ResponseCompressionDefaults.MimeTypes.Concat(
                    new[] { "application/octet-stream" });
            });
            return services.AddCustomAddControllers()
                   .AddCustomSwagger()
                   .AddCustomCors(Configuration, PolicyName)
                   .AddHttpContextAccessor()
                   .AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies())
                   .AddMemoryCache();
        }

        public static IServiceCollection AddCustomConfigure(this IServiceCollection services, IConfiguration Configuration)
        {
            return services.AddJwtConfiguration(Configuration)
                     .Configure<EmailStmpOption>(Configuration)
                     .Configure<EmailNoticeOptions>(Configuration)
                     .Configure<BaseDomainOptions>(Configuration)
                     .Configure<CQHttpOptions>(Configuration.GetSection("CQHTTP"));
        }

        public static IServiceCollection AddCustomDbService(this IServiceCollection services, IConfiguration Configuration)
        {
            services.AddDbContext<BlazorCommunityContext>(opt =>
                 opt.UseMySql(Configuration.GetConnectionString("DbConnectionString")))
                    .AddUnitOfWork<BlazorCommunityContext>()
                    .AddCustomAspIdenitty<BZUserModel, BlazorCommunityContext>();
            return services;
        }
        public static void CreateUplodFolder(this IWebHostEnvironment webHostEnvironment)
        {
            foreach (UploadPath path in Enum.GetValues(typeof(UploadPath)))
            {
                if(!string.IsNullOrWhiteSpace(webHostEnvironment.WebRootPath))
                {
                    var physicsPath = Path.Combine(webHostEnvironment.WebRootPath, UploadRootPath, path.Description());
                    if (!Directory.Exists(physicsPath))
                        Directory.CreateDirectory(physicsPath);
                }
            }
        }

        public static void DisposeAutofacContainerWhenAppStopped(this IHostApplicationLifetime hostApplicationLifetime, IContainer AutofacContainer)
        {
            //程序停止调用autofac函数
            hostApplicationLifetime.ApplicationStopped.Register(() => { if (AutofacContainer != null) AutofacContainer.Dispose(); });
        }
    }
}
