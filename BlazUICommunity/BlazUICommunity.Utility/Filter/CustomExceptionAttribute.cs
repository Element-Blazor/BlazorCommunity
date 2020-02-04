using Blazui.Community.Utility.Response;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Net;
using System.Text;

namespace Blazui.Community.Utility.Filter
{
    public class CustomExceptionAttribute : IExceptionFilter
    {
        private readonly ILogger<CustomExceptionAttribute> logger;
        public CustomExceptionAttribute(ILogger<CustomExceptionAttribute> logger)
        {
            this.logger = logger;
        }
        public void OnException(ExceptionContext context)
        {
            HttpStatusCode status = HttpStatusCode.InternalServerError;

            //处理各种异常
            logger.LogError(context.Exception, context.Exception.Message);
            context.ExceptionHandled = true;
            context.Result = new ExceptionResponse((int)status, context.Exception);
        }
    }
}
