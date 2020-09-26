using BlazorCommunity.App.Pages;
using BlazorCommunity.DTO;
using Blazui.Component;
using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BlazorCommunity.App.Components.Topic
{
    public class TopicUserInfoBase:PageBase
    {
        public HotUserDto TopicUser { get; set; } = new HotUserDto();

        protected async override Task InitilizePageDataAsync()
        {
            var TopicId = NavigationManager.Uri.Split("/").LastOrDefault();
            TopicUser = await NetService.QueryTopicUser(TopicId);
        }
    }
}
