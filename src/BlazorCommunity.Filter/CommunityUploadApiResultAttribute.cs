using Microsoft.AspNetCore.Mvc.Filters;

namespace BlazorCommunity.Filter
{
    public class CommunityUploadApiResultAttribute : ActionFilterAttribute
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