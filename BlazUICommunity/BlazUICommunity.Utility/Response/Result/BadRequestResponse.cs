using Blazui.Community.Utility.Response;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Net;
using System.Text;

namespace Blazui.Community.Utility.Response
{


    public class BadRequestResponse : ObjectResult
    {
        public BadRequestResponse(string message) : base(new BadRequestResultModel(message))
        {
            StatusCode = (int)HttpStatusCode.BadRequest;
        }
    }
}
