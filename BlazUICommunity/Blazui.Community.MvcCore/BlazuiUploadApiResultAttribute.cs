﻿using Microsoft.AspNetCore.Mvc.Filters;

namespace Blazui.Community.MvcCore
{
    public class BlazuiUploadApiResultAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            base.OnActionExecuting(context);
        }

        public override void OnResultExecuting(ResultExecutingContext context)
        {
            base.OnResultExecuting(context);
        }
    }
}