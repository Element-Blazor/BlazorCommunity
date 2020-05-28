using Blazui.Community.WasmApp.Pages;
using Blazui.Community.DTO;
using Blazui.Component;
using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Blazui.Community.WasmApp.Components.Topic
{
    public partial class TopicUserInfo:PageBase
    {
        public HotUserDto HotUser { get; set; } = new HotUserDto();

        protected async override Task InitilizePageDataAsync()
        {
            var TopicId = NavigationManager.Uri.Split("/").LastOrDefault();
            HotUser = await NetService.QueryTopicUser(TopicId);
        }
    }
}
