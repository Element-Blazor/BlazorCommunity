using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.Swagger;
using Swashbuckle.AspNetCore.SwaggerGen;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace BlazUICommunity.Utility.Filter
{
    /// <summary>
    /// 
    /// </summary>
    [AttributeUsage(AttributeTargets.Method | AttributeTargets.Class)]
    public partial class HiddenApiAttribute : Attribute { }
    /// <summary>
    /// 
    /// </summary>
    public class SwaggerHiddenApiFilter : IDocumentFilter
    {

        public void Apply(OpenApiDocument swaggerDoc, DocumentFilterContext context)
        {
            foreach (ApiDescription apiDescription in context.ApiDescriptions)
            {
                if (apiDescription.TryGetMethodInfo(out MethodInfo method))
                {
                    if (method.ReflectedType.CustomAttributes.Any(t => t.AttributeType == typeof(HiddenApiAttribute))
                            || method.CustomAttributes.Any(t => t.AttributeType == typeof(HiddenApiAttribute)))
                    {
                        string key = "/" + apiDescription.RelativePath;
                        if (key.Contains("?"))
                        {
                            int idx = key.IndexOf("?", System.StringComparison.Ordinal);
                            key = key.Substring(0, idx);
                        }
                        swaggerDoc.Paths.Remove(key);
                    }
                }
            }
        }
    }
}
