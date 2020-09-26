using System.Net;

namespace Blazui.Community.Response
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