using Microsoft.AspNetCore.Mvc;

namespace BlazorCommunity.Response
{
    public class NoContentResponse : ObjectResult
    {
        public NoContentResponse(string message = "no data")
            : base(message)
        {
        }
    }
}