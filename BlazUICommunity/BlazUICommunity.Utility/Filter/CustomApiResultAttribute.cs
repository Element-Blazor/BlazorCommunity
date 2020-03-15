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
            if (objectResult?.StatusCode == 401)
            {
                context.Result = new UnauthorizedResult();
            }
            else
            {
                if (!context.Controller.GetType().IsDefined(typeof(BlazuiUploadApiResultAttribute), true))
                {
                    if(context.Result is NoContentResponse)
                    {
                        context.Result = new OkObjectResult(new BaseResponse(code: 205, result: "", message: "no data"));
                    }
                    if (context.Result is NoContentResult)
                    {
                        context.Result = new OkObjectResult(new BaseResponse(code: 204, result: "", message: "Not Modified"));
                    }
                    else if (context.Result is ValidationFailedResponse)
                    {
                        context.Result = new OkObjectResult(new BaseResponse(code: 400, result: "", message: "error request 参数错误"));
                    }
                    else if (context.Result is BadRequestObjectResult || objectResult?.StatusCode == 400)
                    {
                        var Message = "bad request";
                        if (objectResult.Value is BadRequestResultModel bad)
                            Message = bad.Message;
                        context.Result = new OkObjectResult(new BaseResponse(code: 400, result: "", message: Message));
                    }
                    else
                    {
                        context.Result = new OkObjectResult(new BaseResponse(code: 200, result: objectResult?.Value, message: "success"));
                    }
                }
            }
        }
    }
}
