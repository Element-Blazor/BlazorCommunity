using Blazui.Community.WasmApp.Service;
using Blazui.Community.DTO;
using Blazui.Component;
using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Blazui.Community.WasmApp.Pages;
using Blazui.Community.WasmApp.Model.Cache;

namespace Blazui.Community.WasmApp.Components.Topic
{
    public partial class HotUser 
    {

        protected string Title { get; set; }
      
        protected  List<HotUserDto> Datas { get; set; } = new List<HotUserDto>();
        
     
        protected override bool ShouldRender() => true;

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            await base.OnAfterRenderAsync(firstRender);
            if (firstRender)
            {
                Title = "活跃用户";

                var cacheDatas = await localStorage.CreateOrGetCache<HotUsersCache>("HotUsersCache", async () =>
                {
                    var UserResult = await NetService.QueryHotUsers();
                    if(UserResult!=null)
                    return new HotUsersCache
                    {
                         Expire=DateTime.Now.AddDays(1), hotUserDtos= UserResult.IsSuccess ? UserResult.Data : new List<HotUserDto>()

                    };
                    return new HotUsersCache
                    {
                        Expire = null,
                        hotUserDtos = new List<HotUserDto>()
                    };

                });

                Datas = cacheDatas.hotUserDtos;
                StateHasChanged();
            }
        }
    }
}
