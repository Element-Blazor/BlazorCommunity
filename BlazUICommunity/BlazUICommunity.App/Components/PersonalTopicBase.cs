using Blazui.Community.App.Pages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Blazui.Community.App.Components
{
    public class PersonalTopicBase: PersonalPageBase
    {
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
