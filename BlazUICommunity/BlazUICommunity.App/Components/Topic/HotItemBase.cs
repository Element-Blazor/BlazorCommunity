using Blazui.Community.DTO;
using Blazui.Component;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Blazui.Community.App.Components.Topic
{
    public class HotItemBase:BComponentBase
    {
        [Parameter]
        public HotTopicDto Hot { get; set; }

        protected string Title = string.Empty;
        [Parameter]
        public EventCallback<MouseEventArgs> OnClick { get; set; }
        protected override void OnParametersSet()
        {
            base.OnParametersSet();
            if (Hot != null && !string.IsNullOrWhiteSpace(Hot.Title))
            {
                Title = Hot.Title+"";
                Hot.Title = Hot.Title.Replace(" ", "");
                Hot.Title = Hot.Title.Length > 40 ? Hot.Title.Substring(0, 39) : Hot.Title;
            }
        }

    }
}
