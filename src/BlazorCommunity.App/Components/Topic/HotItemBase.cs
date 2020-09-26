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
    public class HotItemBase:ElementComponentBase
    {
        [Parameter]
        public HotTopicDto Hot { get; set; }

        [Parameter]
        public EventCallback<MouseEventArgs> OnClick { get; set; }
       

    }
}
