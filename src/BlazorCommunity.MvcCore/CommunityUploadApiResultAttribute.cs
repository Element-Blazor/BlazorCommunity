using Microsoft.AspNetCore.Mvc.Filters;

namespace BlazorCommunity.MvcCore
{
    public class IgnoreApiResultAttribute : ActionFilterAttribute
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