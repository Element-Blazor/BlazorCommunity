using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.JsonPatch.Operations;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.Swagger;
using Swashbuckle.AspNetCore.SwaggerGen;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Blazui.Community.Utility.Filter
{
    public class SwaggerAuthorizationFilter : IOperationFilter
    {


        public void Apply(OpenApiOperation operation, OperationFilterContext context)
        {
            operation.Parameters ??= new List<OpenApiParameter>();
            _ = context.ApiDescription.ActionDescriptor.AttributeRouteInfo;

            //先判断是否是匿名访问,
            if (context.ApiDescription.ActionDescriptor is ControllerActionDescriptor descriptor)
            {
                var Authorizes = descriptor.MethodInfo.GetCustomAttributes(typeof(AuthorizeFilter), true);
                //非匿名的方法,链接中添加accesstoken值
                if (Authorizes.Any())
                {
                    operation.Responses.Add("401", new OpenApiResponse { Description = "Unauthorized" });
                    //operation.Parameters.Add(new OpenApiParameter()
                    //{
                    //    Required = true,
                    //    Name = "Bearer",
                    //    In = ParameterLocation.Header,
                    //    Description = "You Must  Request With  token",
                    //    Style = ParameterStyle.DeepObject,

                    //});

                }
            }

        }
    }
}
