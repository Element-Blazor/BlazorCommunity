﻿using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System.Collections.Generic;

namespace Blazui.Community.MvcCore
{
    public static class CustomMvcCoreServiceCollectionExtensions
    {
        public static IServiceCollection AddCustomAddControllers(this IServiceCollection services)
        {
            services.AddControllers(o =>
            {
                o.Filters.Add<CustomApiResultAttribute>();
                o.Filters.Add<CustomExceptionAttribute>();
                o.Filters.Add<CustomValidateModelAttribute>();
                var settings = new JsonSerializerSettings
                {
                    DateFormatString = "yyyy-MM-dd HH:mm:ss",
                    ContractResolver = new DefaultContractResolver(),
                    //NullValueHandling = NullValueHandling.Ignore ,
                    DateFormatHandling = DateFormatHandling.IsoDateFormat,
                    DateParseHandling = DateParseHandling.None,
                    ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
                };
                o.ReturnHttpNotAcceptable = true;
                o.OutputFormatters.Insert(0, new KJsonOutputFormatter(settings));
            })
              .AddXmlSerializerFormatters();
            return services;
        }

        public static IServiceCollection AddCustomCors(this IServiceCollection services, string[] AllowOrigins,string PolicyName)
        {
            services.AddCors(options =>
            {
                options.AddPolicy(PolicyName, policy =>
                {
                    policy.WithOrigins(AllowOrigins)
                    //.AllowAnyOrigin() //允许任何来源的主机访问
                    .AllowAnyHeader()
                    .AllowAnyMethod()
                    .AllowCredentials()
                    .SetIsOriginAllowedToAllowWildcardSubdomains();//指定处理cookie
                });
            });
            return services;
        }
    }
}