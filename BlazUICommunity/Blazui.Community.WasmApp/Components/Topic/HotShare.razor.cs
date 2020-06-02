using Blazored.LocalStorage;
using Blazui.Community.DTO;
using Blazui.Community.Response;
using Blazui.Community.WasmApp.Model.Cache;
using Blazui.Community.WasmApp.Service;
using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Blazui.Community.WasmApp.Components.Topic
{
    public partial class HotShare : HotComponentBase
    {
        protected override async Task<List<HotTopicDto>> InitDatas()
        {
            Title = "热点分享";

            var cacheDatas =await localStorage.CreateOrGetCache("HotShare", async () =>
            {
                var datas = await Service.QueryShareHot();
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
