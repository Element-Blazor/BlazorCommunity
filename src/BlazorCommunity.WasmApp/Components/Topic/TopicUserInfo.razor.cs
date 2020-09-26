using BlazorCommunity.WasmApp.Pages;
using BlazorCommunity.DTO;
using Element;
using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Blazored.LocalStorage;
using BlazorCommunity.WasmApp.Model.Cache;

namespace BlazorCommunity.WasmApp.Components.Topic
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
