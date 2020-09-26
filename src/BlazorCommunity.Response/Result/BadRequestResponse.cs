using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace BlazorCommunity.Response
{
    public class BadRequestResponse : ObjectResult
    {
        public BadRequestResponse(string message) : base(new BadRequestResultModel(message))
        {
            StatusCode = (int)HttpStatusCode.BadRequest;
        }
    }
}