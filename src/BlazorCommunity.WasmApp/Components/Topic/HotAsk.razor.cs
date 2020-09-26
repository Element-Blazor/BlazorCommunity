using BlazorCommunity.DTO;
using BlazorCommunity.Response;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Blazored.LocalStorage;
using BlazorCommunity.WasmApp.Model.Cache;
using Microsoft.AspNetCore.Components;

namespace BlazorCommunity.WasmApp.Components.Topic
{
    public partial class HotAsk: HotComponentBase
    {
        
        protected override async Task<List<HotTopicDto>> InitDatas()
        {
            Title = "热点问题";

            var cacheDatas = await localStorage.CreateOrGetCache("HotAsk", async () =>
            {
                var datas = await Service.QueryAskHot();
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
