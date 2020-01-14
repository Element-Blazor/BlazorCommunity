using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net;
using System.Text;

namespace BlazUICommunity.Utility.Response
{

    public class BaseResonse
    {

        public BaseResonse(int? code = null, string message = null, object result = null)
        {
            this.Code = code;
            this.Data = result;
            this.Message = message;
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
        public object Data { get; set; }

    }



}
