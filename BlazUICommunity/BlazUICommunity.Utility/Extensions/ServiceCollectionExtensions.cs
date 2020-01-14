
using BlazUICommunity.Utility.Filter;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Caching.Redis;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;
using System;
using System.Linq;
using System.Reflection;
using Swashbuckle.AspNetCore.SwaggerGen;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using BlazUICommunity.Utility.Formatter;

namespace BlazUICommunity.Utility.Configure
{
    public static class ServiceCollectionExtensions
    {


        /// <summary>
        /// redis
        /// </summary>
        /// <param name="services"></param>
        /// <param name="Configuration"></param>
        public static void AddCustomRedis(this IServiceCollection services , string ConnectionStrings)
        {
            var csredis = new CSRedis.CSRedisClient(ConnectionStrings);
            //初始化 RedisHelper
            RedisHelper.Initialization(csredis);
            //注册mvc分布式缓存
            services.AddSingleton<IDistributedCache>(new CSRedisCache(RedisHelper.Instance));
        }
        /// <summary>
        /// redis
        /// </summary>
        /// <param name="services"></param>
        /// <param name="Configuration"></param>
        public static void AddCustomRedis(this IServiceCollection services , params string[] ConnectionStrings)
        {
            var csredis = new CSRedis.CSRedisClient(null , ConnectionStrings);

            //"127.0.0.1:6372,password=123,defaultDatabase=12,poolsize=11",
            //"127.0.0.1:6373,password=123,defaultDatabase=13,poolsize=12",
            //"127.0.0.1:6374,password=123,defaultDatabase=14,poolsize=13");
            //实现思路：根据CRC16(key) % 节点总数量，确定连向的节点
            //也可以自定义规则(第一个参数设置)

            //初始化 RedisHelper
            RedisHelper.Initialization(csredis);
            //注册mvc分布式缓存
            services.AddSingleton<IDistributedCache>(new CSRedisCache(RedisHelper.Instance));
        }
        /// <summary>
        /// redis
        /// </summary>
        /// <param name="services"></param>
        /// <param name="Configuration"></param>
        public static void AddCustomRedis(this IServiceCollection services , IConfiguration Configuration)
        {
            var csredis = new CSRedis.CSRedisClient(null , Configuration["Redis:ConnectionString"]);

            //"127.0.0.1:6372,password=123,defaultDatabase=12,poolsize=11",
            //"127.0.0.1:6373,password=123,defaultDatabase=13,poolsize=12",
            //"127.0.0.1:6374,password=123,defaultDatabase=14,poolsize=13");
            //实现思路：根据CRC16(key) % 节点总数量，确定连向的节点
            //也可以自定义规则(第一个参数设置)

            //初始化 RedisHelper
            RedisHelper.Initialization(csredis);
            //注册mvc分布式缓存
            services.AddSingleton<IDistributedCache>(new CSRedisCache(RedisHelper.Instance));
        }
        /// <summary>
        /// redis
        /// </summary>
        /// <param name="services"></param>
        /// <param name="Configuration"></param>
        public static void AddCustomRedis(this IServiceCollection services , IConfiguration Configuration , Func<string , string> NodeRule)
        {
            var csredis = new CSRedis.CSRedisClient(NodeRule ,
           Configuration["Redis:ConnectionString"]);

            //初始化 RedisHelper
            RedisHelper.Initialization(csredis);
            //注册mvc分布式缓存
            services.AddSingleton<IDistributedCache>(new CSRedisCache(RedisHelper.Instance));
        }
        /// <summary>
        /// 权限
        /// </summary>
        /// <param name="services"></param>
        /// <param name="Configuration"></param>
        public static IServiceCollection AddCustomIdentityServer(this IServiceCollection services , IConfiguration Configuration)
        {

            services.AddAuthentication("Bearer")
                .AddIdentityServerAuthentication(options =>
                {
                    options.Authority = Configuration["IdentityServer4:Host"];
                    options.RequireHttpsMetadata = false;
                    options.ApiName = Configuration["IdentityServer4:ServerName"];
                    //options.ApiSecret = "apiservice123";
                });
            return services;
        }
        /// <summary>
        /// Swagger
        /// </summary>
        /// <param name="services"></param>
        public static IServiceCollection AddCustomSwagger(this IServiceCollection services , Action<SwaggerGenOptions> options = null)
        {
            if ( options != null )
                services.AddSwaggerGen(options);
            else
                services.AddSwaggerGen(DefaultSwaggerGenOptions());
            return services;
        }
        public static IServiceCollection AddCustomMvcCore(this IServiceCollection services)
        {
            services.AddMvcCore(o =>
            {
                o.Filters.Add<CustomApiResultAttribute>();
                o.Filters.Add<CustomExceptionAttribute>();
                o.Filters.Add<CustomValidateModelAttribute>();
                var settings = new JsonSerializerSettings
                {
                    DateFormatString = "yyyy-MM-dd HH:mm:ss" ,
                    ContractResolver = new DefaultContractResolver() ,
                    NullValueHandling = NullValueHandling.Ignore ,
                    DateFormatHandling = DateFormatHandling.IsoDateFormat ,
                    DateParseHandling = DateParseHandling.None ,
                    ReferenceLoopHandling = ReferenceLoopHandling.Ignore ,
                };
                o.ReturnHttpNotAcceptable = true;
                o.OutputFormatters.Insert(0 , new KJsonOutputFormatter(settings));
            })
                        .AddXmlSerializerFormatters().AddApiExplorer();
            return services;
        }
        public static IServiceCollection AddCustomAddControllers(this IServiceCollection services)
        {
            services.AddControllers(o =>
            {
                o.Filters.Add<CustomApiResultAttribute>();
                o.Filters.Add<CustomExceptionAttribute>();
                o.Filters.Add<CustomValidateModelAttribute>();
                var settings = new JsonSerializerSettings
                {
                    DateFormatString = "yyyy-MM-dd HH:mm:ss" ,
                    ContractResolver = new DefaultContractResolver() ,
                    NullValueHandling = NullValueHandling.Ignore ,
                    DateFormatHandling = DateFormatHandling.IsoDateFormat ,
                    DateParseHandling = DateParseHandling.None ,
                    ReferenceLoopHandling = ReferenceLoopHandling.Ignore ,
                };
                o.ReturnHttpNotAcceptable = true;
                o.OutputFormatters.Insert(0 , new KJsonOutputFormatter(settings));
            })
              .AddXmlSerializerFormatters();
            return services;
        }

        public static IServiceCollection AddCustomCors(this IServiceCollection services)
        {
            services.AddCors(options =>
            {
                options.AddPolicy("any" , b =>
                {
                    b.AllowAnyOrigin() //允许任何来源的主机访问
                    .AllowAnyMethod()
                    .AllowAnyHeader()
                    .AllowCredentials();//指定处理cookie
                });
            });
            return services;
        }
        public static IServiceCollection AddCustomSession(this IServiceCollection services)
        {
            services.AddSession(options =>
            {
                options.Cookie.Name = "MyCookie";
                options.IdleTimeout = TimeSpan.FromMinutes(10);//设置session的过期时间
                options.Cookie.HttpOnly = true;//设置在浏览器不能通过js获得该cookie的值
            });
            return services;
        }
        public static IServiceCollection AddCustomMemoryCache(this IServiceCollection services)
        {
            services.AddMemoryCache();

            return services;
        }
        public static IServiceCollection AddCustomDistributedMemoryCache(this IServiceCollection services)
        {
            services.AddDistributedMemoryCache();

            return services;
        }
        private static Action<SwaggerGenOptions> DefaultSwaggerGenOptions()
        {
            Action<SwaggerGenOptions> options = (o) =>
            {
                o.OperationFilter<SwaggerAuthorizationFilter>();

                o.SwaggerDoc("v1" , new OpenApiInfo
                {
                    Version = "v1" ,
                    Title = "WebApi Swagger Document" ,
                    Description = "WebApi Swagger Document" ,

                    TermsOfService = new Uri("http://www.webapi.com") ,
                    Contact = new OpenApiContact
                    {
                        Name = "浮生寄墟丘" ,
                        Email = "xxxx@qq.com" ,
                        Url = new Uri("http://www.webapi.com") ,
                    } ,
                    License = new OpenApiLicense
                    {
                        Name = "许可证" ,
                        Url = new Uri("http://www.webapi.com") ,
                    }
                });
                o.OrderActionsBy((apiDesc) => $"{apiDesc.ActionDescriptor.RouteValues["controller"]}_{apiDesc.HttpMethod}");
                o.AddSecurityDefinition("Bearer" , new OpenApiSecurityScheme()
                {
                    Description = "请在下方输入：Bearer {Token}" ,
                    Name = "Authorization" ,
                    In = ParameterLocation.Header ,
                    Type = SecuritySchemeType.ApiKey ,
                    BearerFormat = "JWT" ,
                    Scheme = "Bearer" ,

                });
                o.AddSecurityRequirement(new OpenApiSecurityRequirement
               {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference {
                                Type = ReferenceType.SecurityScheme,
                                Id = "Bearer",
                            }
                        },
                        new[] { "readAccess", "writeAccess" }
                    }
               });

                o.DocumentFilter<SwaggerHiddenApiFilter>();
                var XmlPath = $"{AppContext.BaseDirectory}{AppDomain.CurrentDomain.FriendlyName}.xml";
                o.IncludeXmlComments(XmlPath);
            };
            return options;
        }

    }
}
