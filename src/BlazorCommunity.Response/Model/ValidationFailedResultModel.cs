using Microsoft.AspNetCore.Mvc.ModelBinding;
using Newtonsoft.Json;
using System.Linq;
using System.Net;

namespace BlazorCommunity.Response
{
    public class ValidationFailedResultModel : BaseResponse
    {
        public ValidationFailedResultModel(ModelStateDictionary modelState)
        {
            Code = (int)HttpStatusCode.UnprocessableEntity;
            Message = "参数错误";
            Data = modelState.Keys.SelectMany(key => modelState[key].Errors.Select(x => new ValidationError(key, x.ErrorMessage)));
        }
    }

    public class ValidationError
    {
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string Field { get; }

        public string Message { get; }

        public ValidationError(string field, string message)
        {
            Field = field != string.Empty ? field : null;
            Message = message;
        }
    }
}