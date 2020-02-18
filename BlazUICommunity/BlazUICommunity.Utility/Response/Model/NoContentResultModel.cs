using System;
using System.Collections.Generic;
using System.Net;
using System.Text;

namespace Blazui.Community.Utility.Response
{
    public class NoContentResultModel : BaseResponse
    {
        public NoContentResultModel(string message)
        {
            Code = 200;//没有数据
            Message = message;
            Data = null;
        }
    }
}
