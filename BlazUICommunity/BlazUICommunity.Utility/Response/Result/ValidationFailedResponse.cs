using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System;
using System.Collections.Generic;
using System.Net;
using System.Text;

namespace BlazUICommunity.Utility.Response
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
