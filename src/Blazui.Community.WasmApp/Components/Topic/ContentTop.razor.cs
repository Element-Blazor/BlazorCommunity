using Blazui.Community.WasmApp.Pages;
using Blazui.Community.DTO;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using Blazored.LocalStorage;
using Blazui.Community.WasmApp.Model.Cache;
using Blazui.Community.WasmApp.Service;
using System;

namespace Blazui.Community.WasmApp.Components.Topic
{
    public partial class ContentTop : PageBase
    {
        protected List<BZTopicDto> Topics = new List<BZTopicDto>();

       
        protected override async Task InitilizePageDataAsync()
        {
           var  Datas=await localStorage.CreateOrGetCache<ContentTopDataCache>("ContentTop", async () =>
            {
                var result = await NetService.QueryTopdTopics();
                if (result.IsSuccess)
                {
                   
                        return new ContentTopDataCache
                        {
                            Expire = DateTime.Now.AddMinutes(30),
                            Topics = result.Data
                        };
                }
                else
                {
                    return new ContentTopDataCache
                    {
                        Expire = null, Topics = new List<BZTopicDto>()
                    };
                }
            });
           Topics = Datas.Topics;
        }
    }
}