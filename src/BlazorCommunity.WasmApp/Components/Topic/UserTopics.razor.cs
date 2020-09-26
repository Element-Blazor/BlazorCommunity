using BlazorCommunity.DTO;
using BlazorCommunity.Response;
using Element;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BlazorCommunity.WasmApp.Model.Cache;

namespace BlazorCommunity.WasmApp.Components.Topic
{
    public partial class UserTopics : HotComponentBase
    {
        protected override async Task<List<HotTopicDto>> InitDatas()
        {
            Title = "作者分享";
            var TopicId = NavigationManager.Uri.Split("/").LastOrDefault();

            var cacheDatas = await localStorage.CreateOrGetCache($"HotUserTopic-{TopicId}", async () =>
            {
                var datas = await Service.QueryTopicByAuthor(TopicId);
                if (datas.IsSuccess)
                {
                    return new HotTopicCache
                    {
                        Expire = DateTime.Now.AddMinutes(30),
                        List = datas.Data
                    };
                }
                return new HotTopicCache
                {
                    Expire = null,
                    List = new List<HotTopicDto>()
                };
            });
            return cacheDatas.List;
        }
    }
}
