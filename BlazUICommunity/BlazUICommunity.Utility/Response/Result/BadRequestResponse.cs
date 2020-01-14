using BlazUICommunity.Utility.Response;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Net;
using System.Text;

namespace BlazUICommunity.Utility.Response
{


    public class BadRequestResponse : ObjectResult
    {
        public BadRequestResponse(string message) : base(new BadRequestResultModel(message))
        {
            StatusCode = (int)HttpStatusCode.BadRequest;
        }
    }
}
