using System.Net;

namespace BlazorCommunity.Response
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