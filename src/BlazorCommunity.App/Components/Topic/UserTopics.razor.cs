using BlazorCommunity.DTO;
using BlazorCommunity.Response;
using Blazui.Component;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BlazorCommunity.App.Components.Topic
{
    public partial class UserTopics : HotComponentBase
    {
        protected override async Task<BaseResponse<List<HotTopicDto>>> InitDatas()
        {
            Title = "作者分享";
            var TopicId = NavigationManager.Uri.Split("/").LastOrDefault();
            return await Service.QueryTopicByAuthor(TopicId);
        }
    }
}
