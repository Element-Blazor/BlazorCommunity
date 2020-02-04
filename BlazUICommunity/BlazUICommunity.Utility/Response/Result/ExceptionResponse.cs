using Blazui.Community.Utility.Response;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Net;
using System.Text;

namespace Blazui.Community.Utility.Response
{


    public class ExceptionResponse : ObjectResult
    {
        public ExceptionResponse(int? code, Exception exception)
                : base(new ExceptionResultModel(code, exception))
        {
            StatusCode = code;
        }
    }
}
