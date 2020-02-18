using Blazui.Community.Utility.Response;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Blazui.Community.Utility.Filter
{
    public class CustomApiResultAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            base.OnActionExecuting(context);
        }
        public override void OnResultExecuting(ResultExecutingContext context)
        {
            base.OnResultExecuting(context);
            var objectResult = context.Result as ObjectResult;
            if (context.Result is ValidationFailedResponse)
            {
                context.Result = objectResult;
            }
            else if (context.Result is BadRequestObjectResult || objectResult?.StatusCode == 400)
            {
                context.Result = new BadRequestResponse(JsonConvert.SerializeObject(objectResult?.Value));
            }
            else if (context.Result is NoContentResponse)
            {
                context.Result = new OkObjectResult(new BaseResponse(code: 200, result: objectResult?.Value, message: objectResult?.Value?.ToString()));
            }
            else
            {
                context.Result =  new OkObjectResult(new BaseResponse(code: 200, result: objectResult?.Value, message: "success"));
            }
        }
    }
}
