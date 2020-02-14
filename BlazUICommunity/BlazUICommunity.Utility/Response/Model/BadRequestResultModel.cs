using System;
using System.Collections.Generic;
using System.Net;
using System.Text;

namespace Blazui.Community.Utility.Response
{
    public class BadRequestResultModel : BaseResponse
    {
        public BadRequestResultModel(string message)
        {
            Code = (int)HttpStatusCode.BadRequest;
            Message = message;
            Data = null;
        }
    }
}
