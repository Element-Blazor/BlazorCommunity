using Microsoft.AspNetCore.Mvc;
using System;

namespace BlazorCommunity.Response
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