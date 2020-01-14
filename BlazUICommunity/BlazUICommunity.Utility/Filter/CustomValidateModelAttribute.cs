using BlazUICommunity.Utility.Response;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BlazUICommunity.Utility.Filter
{
    public class CustomValidateModelAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuted(ActionExecutedContext context)
        {
        }

        public override void OnResultExecuted(ResultExecutedContext context)
        {
        }
        public override void OnResultExecuting(ResultExecutingContext context)
        {
            if (!context.ModelState.IsValid)
            {
                context.Result = new ValidationFailedResponse(context.ModelState);
            }
        }
        public override void OnActionExecuting(ActionExecutingContext context)
        {

        }


    }

}
