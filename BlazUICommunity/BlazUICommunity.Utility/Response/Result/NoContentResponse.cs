using BlazUICommunity.Utility.Response;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Net;
using System.Text;

namespace BlazUICommunity.Utility.Response
{


    public class NoContentResponse : ObjectResult
    {
        public NoContentResponse(string message = "no data")
            : base(new NoContentResultModel(message))
        {
            StatusCode = 444;
        }
    }
}
