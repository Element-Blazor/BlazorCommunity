using Microsoft.AspNetCore.Builder;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerUI;
using System;
using System.Collections.Generic;
using System.Text;

namespace BlazUICommunity.Utility.Extensions
{
    public static class ApplicationBuilderExtensions
    {

        public class SwaggerOptions
        {
            public string Title { get; set; }
        }
        /// <summary>
        /// SwaggerUi
        /// </summary>
        /// <param name="app"></param>
        public static void UseCustomSwaggerUI(this IApplicationBuilder app  ,Action<SwaggerOptions> options)
        {
            SwaggerOptions option = new SwaggerOptions();
            options?.Invoke(option);
            //启用中间件服务生成Swagger作为JSON终结点
            app.UseSwagger(c =>
            {
                //c.SerializeAsV2 = true;
                //c.RouteTemplate = "api-docs/{documentName}/swagger.json";
                c.PreSerializeFilters.Add((swaggerDoc , httpReq) =>
                {
                    swaggerDoc.Servers = new List<OpenApiServer> { new OpenApiServer { Url = $"{httpReq.Scheme}://{httpReq.Host.Value}" } };
                    OpenApiPaths paths = new OpenApiPaths();
                    foreach ( var path in swaggerDoc.Paths )
                    {
                        //if ( path.Key.StartsWith("/v1/api") )//做版本控制
                        paths.Add(path.Key , path.Value);
                    }
                    swaggerDoc.Paths = paths;
                });
            });
            //启用中间件服务对swagger-ui，指定Swagger JSON终结点
            app.UseSwaggerUI(c =>
            {
                //c.MaxDisplayedTags(5);
                //c.DisplayOperationId();//唯一标识操作
                c.SwaggerEndpoint("/swagger/v1/swagger.json" , option.Title);
                //c.SwaggerEndpoint("/swagger/v2/swagger.json", "V2 Docs");
                c.RoutePrefix = "swagger";//根路由
                c.EnableDeepLinking();//启用深度链接--不知道干嘛的
                c.DisplayRequestDuration();//调试，显示接口响应时间
                c.EnableValidator();//验证
                c.DocExpansion(DocExpansion.List);//默认展开
                c.DefaultModelsExpandDepth(-1);//隐藏model
                c.DefaultModelExpandDepth(3);//model展开层级
                c.EnableFilter();//筛选--如果接口过多可以开启
                c.DefaultModelRendering(ModelRendering.Model);//设置显示参数的实体或Example
                //c.SupportedSubmitMethods(SubmitMethod.Get , SubmitMethod.Head , SubmitMethod.Post);//


                //c.OAuthClientId("test-id");
                //c.OAuthClientSecret("test-secret");
                //c.OAuthRealm("test-realm");
                //c.OAuthAppName("test-app");
                //c.OAuthScopeSeparator(" ");
                //c.OAuthAdditionalQueryStringParams(new Dictionary<string, string> { { "foo", "bar" } });
                //c.OAuthUseBasicAuthenticationWithAccessCodeGrant();
            });
        }
        /// <summary>
        /// SwaggerUi
        /// </summary>
        /// <param name="app"></param>
        public static void UseCustomSwaggerUI(this IApplicationBuilder app , string ApiTitle)
        {
            //启用中间件服务生成Swagger作为JSON终结点
            app.UseSwagger(c =>
            {
                //c.SerializeAsV2 = true;
                //c.RouteTemplate = "api-docs/{documentName}/swagger.json";
                c.PreSerializeFilters.Add((swaggerDoc , httpReq) =>
                {
                    swaggerDoc.Servers = new List<OpenApiServer> { new OpenApiServer { Url = $"{httpReq.Scheme}://{httpReq.Host.Value}" } };
                    OpenApiPaths paths = new OpenApiPaths();
                    foreach ( var path in swaggerDoc.Paths )
                    {
                        //if ( path.Key.StartsWith("/v1/api") )//做版本控制
                        paths.Add(path.Key , path.Value);
                    }
                    swaggerDoc.Paths = paths;
                });
            });
            //启用中间件服务对swagger-ui，指定Swagger JSON终结点
            app.UseSwaggerUI(c =>
            {
                //c.MaxDisplayedTags(5);
                //c.DisplayOperationId();//唯一标识操作
                c.SwaggerEndpoint("/swagger/v1/swagger.json" , ApiTitle);
                //c.SwaggerEndpoint("/swagger/v2/swagger.json", "V2 Docs");
                c.RoutePrefix = "swagger";//根路由
                c.EnableDeepLinking();//启用深度链接--不知道干嘛的
                c.DisplayRequestDuration();//调试，显示接口响应时间
                c.EnableValidator();//验证
                c.DocExpansion(DocExpansion.None);//默认展开
                c.DefaultModelsExpandDepth(-1);//隐藏model
                c.DefaultModelExpandDepth(3);//model展开层级
                c.EnableFilter();//筛选--如果接口过多可以开启
                c.DefaultModelRendering(ModelRendering.Model);//设置显示参数的实体或Example
                //c.SupportedSubmitMethods(SubmitMethod.Get , SubmitMethod.Head , SubmitMethod.Post , SubmitMethod.Put , SubmitMethod.Delete);


                //c.OAuthClientId("test-id");
                //c.OAuthClientSecret("test-secret");
                //c.OAuthRealm("test-realm");
                //c.OAuthAppName("test-app");
                //c.OAuthScopeSeparator(" ");
                //c.OAuthAdditionalQueryStringParams(new Dictionary<string, string> { { "foo", "bar" } });
                //c.OAuthUseBasicAuthenticationWithAccessCodeGrant();
            });
        }
    }
}
