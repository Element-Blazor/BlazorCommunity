using Blazui.Community.App.Pages;
using Blazui.Component.Container;
using Microsoft.AspNetCore.Authorization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Blazui.Community.App.Components
{
    [Authorize]
    public class PersonalTopicBase: PersonalPageBase
    {
        protected BCard bCard { get; set; }
        protected BTab mybTab { get; set; }
        protected BTabPanel bTabPanelPush { get; set; }

        protected BTabPanel bTabPanelCollection { get; set; }
        protected override async Task InitilizePageDataAsync()
        {
            await Task.CompletedTask;
        }
        protected override void InitTabTitle()
        {
            tabTitle = "我的帖子";
        }
    }
}
