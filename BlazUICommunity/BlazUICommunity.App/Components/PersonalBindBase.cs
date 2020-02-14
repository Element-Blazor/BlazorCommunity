using Blazui.Community.App.Pages;
using Microsoft.AspNetCore.Authorization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Blazui.Community.App.Components
{
    [Authorize]
    public class PersonalBindBase : PersonalPageBase
    {
        protected override async Task InitilizePageDataAsync()
        {
           await Task.CompletedTask;
        }
        protected override void InitTabTitle()
        {
            tabTitle = "我的绑定";
        }
    }
}
