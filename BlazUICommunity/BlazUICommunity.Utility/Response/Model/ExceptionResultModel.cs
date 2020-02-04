using System;
using System.Collections.Generic;
using System.Text;

namespace Blazui.Community.Utility.Response
{
    public class ExceptionResultModel : BaseResonse
    {
        public ExceptionResultModel(int? code, Exception exception)
        {
            Code = code;
            Message = exception.InnerException != null ?
                exception.InnerException.Message :
                exception.Message;
            Data = null;
        }
    }
}
