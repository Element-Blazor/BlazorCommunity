namespace BlazorCommunity.Response
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