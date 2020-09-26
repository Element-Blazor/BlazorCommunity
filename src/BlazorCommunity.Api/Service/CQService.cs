using BlazorCommunity.Api.Options;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;

namespace BlazorCommunity.Api.Service
{
    public class CQService
    {
        private readonly HttpClient httpClient;
        private readonly IOptions<CQHttpOptions> CQOption;
        private readonly IOptions<BaseDomainOptions> domainOption;
        private readonly ILogger<CQService> _logger;
        public CQService(IHttpClientFactory httpClientFactory, IOptions<CQHttpOptions> options, IOptions<BaseDomainOptions> DomainOption, ILogger<CQService> logger)
        {
            httpClient = httpClientFactory.CreateClient("CQService");
            this.CQOption = options;
            domainOption = DomainOption;
            _logger = logger;
        }


        public async Task<CQMessageResponse> SendGroupMessageToManager(string TopicId,string TopicTtile)
        {
            try
            {
                var sendQQ = SelectQQByWeight().QQ;
                var content = $"[CQ:at,qq={sendQQ}] 社区有新的提问-“{TopicTtile}”，请尽快回复，也欢迎群内各位大佬参与讨论 {domainOption.Value.BaseDomain}topic/{TopicId}";
                var message = HttpUtility.UrlEncode(content);
                var responseJson = await httpClient.GetStringAsync(string.Format(CQApiList.SendGroup, message ));
                Console.WriteLine(responseJson) ;
                return JsonConvert.DeserializeObject<CQMessageResponse>(responseJson);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.StackTrace);
                return new CQMessageResponse { retcode = -1, status = "failed", data = null };
            }

        }


        public async Task<CQMessageResponse> SendGroupMessageToUser(string TopicId, string TopicTtile, string UserQQ,string ReplyUserQQ)
        {
            try
            {
                var CQCodeUser = string.IsNullOrWhiteSpace(UserQQ)?"": $"[CQ:at,qq={UserQQ}]";
                var CQCodeReplyUser = string.IsNullOrWhiteSpace(ReplyUserQQ) ? "" : $"（感谢[CQ:at,qq={ReplyUserQQ}]的回复）";
                var content = $"{CQCodeUser} 帖子-“{TopicTtile}”，有新的回复 {CQCodeReplyUser}，请及时查看  {domainOption.Value.BaseDomain}topic/{TopicId}";
                var message = HttpUtility.UrlEncode(content);
                var responseJson = await httpClient.GetStringAsync(string.Format(CQApiList.SendGroup,
                    message));
                Console.WriteLine(responseJson);
                return JsonConvert.DeserializeObject<CQMessageResponse>(responseJson);

            }
            catch (Exception ex)
            {
                _logger.LogError(ex.StackTrace);
                return new CQMessageResponse { retcode = -1, status = "failed", data = null };
            }

        }

        private QQS SelectQQByWeight()
        {
            var qqs = CQOption.Value.ManageQQ
               .Select(s => new KeyValuePair<QQS, int>(s, s.Weight));
            List<QQS> temps = new List<QQS>();//总数等于权重的和
            foreach (var sw in qqs)
            {
                for (int i = 0; i < sw.Value; i++)
                {
                    temps.Add(sw.Key);
                }
            }
            int total = qqs.Sum(s => s.Value);
            int index = new Random().Next(0, total);//左边闭区间  右边开区间
            return temps[index];

        }
    }

    public class CQApiList
    {
        /// <summary>
        /// 发送群消息
        /// </summary>
        public static string SendGroup = "/send_group_msg?group_id=74522853&message={0}";
    }

    public class CQMessageResponse
    {
        public object data { get; set; }
        public int retcode { get; set; }
        public string status { get; set; }
    }

    public class MessageId
    {
        public string message_id { get; set; }
    }
}
