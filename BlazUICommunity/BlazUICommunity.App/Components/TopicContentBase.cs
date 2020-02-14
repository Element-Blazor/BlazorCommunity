using Blazui.Community.App.Model;
using Blazui.Community.App.Pages;
using Blazui.Community.Utility;
using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Blazui.Community.App.Components
{
    public class TopicContentBase : PageBase
    {
        [Parameter]
        public int TopicId { get; set; }
        protected TopicItemModel TopicModel { get; set; }

        protected override async Task InitilizePageDataAsync()
        {
            if (TopicId > 0)
            {
                var result = await NetService.GetTopicById(TopicId);
                if (result.IsSuccess)
                    TopicModel = mapper.Map<TopicItemModel>(result.Data);
                TopicModel.ReleaseTime = Convert.ToDateTime(TopicModel.ReleaseTime).ConvertToDateDiffStr();
            }
        }
    }
}
