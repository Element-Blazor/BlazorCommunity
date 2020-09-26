using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using System;

namespace BlazorCommunity.SwaggerExtensions
{
    public class SwaggerParameterFilter : IParameterFilter
    {
        public void Apply(OpenApiParameter parameter, ParameterFilterContext context)
        {
            throw new NotImplementedException();
        }
    }
}