using BlazorCommunity.DTO;
using Element;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BlazorCommunity.App.Components.Topic
{
    public class BHotTopicBase:ElementComponentBase
    {
        [Parameter]
        public string Title { get; set; }
        [Parameter]
        public virtual List<HotTopicDto> Datas { get; set; } = new List<HotTopicDto>();

        [Parameter]
        public EventCallback<string> OnItemClick { get; set; }


        protected async Task ItemClick(string TopicId)
        {
            if (OnItemClick.HasDelegate)
                await OnItemClick.InvokeAsync(TopicId);
        }

    }
}
