using System;
using System.Collections.Generic;
using System.Net;
using System.Text;

namespace BlazUICommunity.Utility.Response
{
    public class BadRequestResultModel : BaseResonse
    {
        public BadRequestResultModel(string message)
        {
            Code = (int)HttpStatusCode.BadRequest;
            Message = message;
            Data = null;
        }
    }
}
