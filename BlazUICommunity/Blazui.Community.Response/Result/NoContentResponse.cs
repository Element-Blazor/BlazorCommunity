using Microsoft.AspNetCore.Mvc;

namespace Blazui.Community.Response
{
    public class NoContentResponse : ObjectResult
    {
        public NoContentResponse(string message = "no data")
            : base(message)
        {
        }
    }
}