using System;
using System.Collections.Generic;
using System.Net;
using System.Text;

namespace BlazUICommunity.Utility.Response
{
    public class NoContentResultModel : BaseResonse
    {
        public NoContentResultModel(string message)
        {
            Code = 444;//没有数据
            Message = message;
            Data = null;
        }
    }
}
