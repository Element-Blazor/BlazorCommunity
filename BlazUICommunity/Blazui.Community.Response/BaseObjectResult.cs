using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Net;

namespace Blazui.Community.Response
{
    public class BaseObjectResult<T> : ObjectResult
    {
        public BaseObjectResult(T t) : base(t)
        {
        }

        /// <summary>
        /// 200 是 正常  其他都是错误
        /// </summary>
        [JsonProperty("code")]
        public int? Code { get; set; }

        [JsonProperty("msg")]
        public string Message { get; set; }

        //[JsonIgnore]
        [JsonProperty("IsSuccess")]
        public bool IsSuccess => Code == (int)HttpStatusCode.OK;

        [JsonProperty("data")]
        public T Data { get; set; }
    }
}