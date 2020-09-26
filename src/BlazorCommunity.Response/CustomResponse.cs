using Newtonsoft.Json;

namespace BlazorCommunity.Response
{
    public class CustomResponse
    {
        /// <summary>
        /// 200 是 正常  其他都是错误
        /// </summary>
        [JsonProperty("code")]
        public int? Code { get; set; }

        [JsonProperty("msg")]
        public string Message { get; set; }

        //[JsonIgnore]
        [JsonProperty("IsSuccess")]
        public bool IsSuccess => Code == 200;
    }
}