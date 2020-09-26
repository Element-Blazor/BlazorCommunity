using Blazui.Community.Response;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Blazui.Community.MvcCore
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