
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net;
using System.Text;

namespace Blazui.Community.Utility.Response
{

    public class BaseResponse: CustomResponse
    {
        public BaseResponse()
        {

        }
       
        public BaseResponse(int? code = null, string message = null, object result = null)
        {
            this.Code = code;
            this.Data = result;
            this.Message = message;
        }
        [JsonProperty("data")]
        public object Data { get; set; }

    }

    public class BaseResponse<T>: CustomResponse
    {
        public BaseResponse(int? code = null, string message = null)
        {
            this.Code = code;
            this.Message = message;
        }
        [JsonProperty("data")]
        public T Data { get; set; }

    }
}
