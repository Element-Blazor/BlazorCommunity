using BlazorCommunity.DTO;
using Blazui.Component;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BlazorCommunity.App.Components.Topic
{
    public class HotItemBase:BComponentBase
    {
        [Parameter]
        public HotTopicDto Hot { get; set; }

        [Parameter]
        public EventCallback<MouseEventArgs> OnClick { get; set; }
       

    }
}
