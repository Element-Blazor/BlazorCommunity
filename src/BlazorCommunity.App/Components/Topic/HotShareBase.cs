using BlazorCommunity.DTO;
using BlazorCommunity.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BlazorCommunity.App.Components.Topic
{
    public class HotShareBase : HotComponentBase
    {
        protected async override Task<BaseResponse<List<HotTopicDto>>> InitDatas()
        {
            Title = "热点分享";
            return await Service.QueryShareHot();
        }

    }
}
