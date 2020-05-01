using Blazui.Community.App.Pages;
using Blazui.Community.DTO;
using Blazui.Component;
using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Blazui.Community.App.Components.Topic
{
    public class TopicUserInfoBase:PageBase
    {
        public HotUserDto User { get; set; } = new HotUserDto();

        protected async override Task InitilizePageDataAsync()
        {
            var TopicId = NavigationManager.Uri.Split("/").LastOrDefault();
            User = await NetService.QueryTopicUser(TopicId);
        }
    }
}
