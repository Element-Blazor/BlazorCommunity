using Blazui.Community.App.Model;
using Blazui.Community.App.Pages;
using Blazui.Community.App.Service;
using Blazui.Community.DTO;
using Blazui.Component;
using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Blazui.Community.App.Components
{
    public class ContentTopBase : PageBase
    {
        protected List<TopicItemModel> Topics = new List<TopicItemModel>();
        protected override async Task InitilizePageDataAsync()
        {
            var result = await NetService.GetTopdTopics();
            if (result.IsSuccess)
            {
                if (result.Data.Any())
                {
                    Topics = mapper.Map <List<TopicItemModel>>(result.Data);
                }
            }
        }

    }
}
