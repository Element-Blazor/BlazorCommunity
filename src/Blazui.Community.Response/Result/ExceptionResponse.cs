using Microsoft.AspNetCore.Mvc;
using System;

namespace Blazui.Community.Response
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