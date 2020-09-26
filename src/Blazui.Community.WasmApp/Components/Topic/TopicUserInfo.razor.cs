using Blazui.Community.WasmApp.Pages;
using Blazui.Community.DTO;
using Blazui.Component;
using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Blazored.LocalStorage;
using Blazui.Community.WasmApp.Model.Cache;

namespace Blazui.Community.WasmApp.Components.Topic
{
    public partial class TopicUserInfo:PageBase
    {
        public HotUserDto HotUser { get; set; } = new HotUserDto();
        protected async override Task InitilizePageDataAsync()
        {
            var TopicId = NavigationManager.Uri.Split("/").LastOrDefault();

            var cacheDatas =
                await localStorage.CreateOrGetCache<TopicAuthorInfoCache>($"HotUser-{TopicId}", async () =>
                {
                    var    HotUser = await NetService.QueryTopicUser(TopicId);
                    if (HotUser != null)
                        return new TopicAuthorInfoCache
                        {
                            Expire = DateTime.Now.AddDays(1), HotUser = HotUser
                        };

                    return new TopicAuthorInfoCache
                    {
                        Expire=null,
                        HotUser = null
                    };
                });
        }



    }
   
}
