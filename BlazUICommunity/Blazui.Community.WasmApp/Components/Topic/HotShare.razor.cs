using Blazui.Community.DTO;
using Blazui.Community.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Blazui.Community.WasmApp.Components.Topic
{
    public partial class HotShare : HotComponentBase
    {
        protected async override Task<BaseResponse<List<HotTopicDto>>> InitDatas()
        {
            Title = "热点分享";
          return await Service.QueryShareHot();
        }

    }
}
