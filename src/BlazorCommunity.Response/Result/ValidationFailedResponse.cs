using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.Net;

namespace BlazorCommunity.Response
{
    public class ValidationFailedResponse : ObjectResult
    {
        public ValidationFailedResponse(ModelStateDictionary modelState)
       : base(new ValidationFailedResultModel(modelState))
        {
            StatusCode = (int)HttpStatusCode.UnprocessableEntity;
        }
    }
}