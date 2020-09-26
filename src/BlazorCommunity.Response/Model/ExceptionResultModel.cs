using System;

namespace BlazorCommunity.Response
{
    public class ExceptionResultModel : BaseResponse
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