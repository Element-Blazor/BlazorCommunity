using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using System;

namespace BlazorCommunity.SwaggerExtensions
{
    public static class SwaggerServiceConllectionExtensions
    {
        /// <summary>
        /// Swagger
        /// </summary>
        /// <param name="services"></param>
        public static IServiceCollection AddCustomSwagger(this IServiceCollection services, Action<SwaggerGenOptions> options = null)
        {
            if (options != null)
                services.AddSwaggerGen(options);
            else
                services.AddSwaggerGen(DefaultSwaggerGenOptions());
            return services;
        }

        private static Action<SwaggerGenOptions> DefaultSwaggerGenOptions()
        {
            Action<SwaggerGenOptions> options = o =>
            {
                o.OperationFilter<SwaggerAuthorizationFilter>();

                o.SwaggerDoc("v1", new OpenApiInfo
                {
                    Version = "v1",
                    Title = "WebApi Swagger Document",
                    Description = "WebApi Swagger Document",

                    TermsOfService = new Uri("http://www.webapi.com"),
                    Contact = new OpenApiContact
                    {
                        Name = "浮生寄墟丘",
                        Email = "xxxx@qq.com",
                        Url = new Uri("http://www.webapi.com"),
                    },
                    License = new OpenApiLicense
                    {
                        Name = "许可证",
                        Url = new Uri("http://www.webapi.com"),
                    }
                });
                o.OrderActionsBy((apiDesc) => $"{apiDesc.ActionDescriptor.RouteValues["controller"]}_{apiDesc.HttpMethod}");
                o.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme()
                {
                    Description = "请在下方输入：Bearer {Token}",
                    Name = "Authorization",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.ApiKey,
                    BearerFormat = "JWT",
                    Scheme = "Bearer",
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

                o.EnableAnnotations(true);
            };
            return options;
        }
    }
}