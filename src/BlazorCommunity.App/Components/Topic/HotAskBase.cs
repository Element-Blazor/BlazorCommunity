using BlazorCommunity.DTO;
using BlazorCommunity.Response;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BlazorCommunity.App.Components.Topic
{
    public class HotAskBase: HotComponentBase
    {
        protected async override Task<BaseResponse<List<HotTopicDto>>> InitDatas()
        {
            Title = "热点问题";
            return await Service.QueryAskHot();
        }
    }
}
