using Blazui.Community.App.Components;
using Blazui.Community.DTO;
using Blazui.Community.Enums;
using Blazui.Component;
using Blazui.Component.Container;
using Blazui.Component.EventArgs;
using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Blazui.Community.App.Pages
{
    public class IndexBase : BComponentBase
    {
        protected List<TabItem> Tabs = new List<TabItem>();
        protected BTab btab;
        protected BLayout blayout;
        protected void ActiveTabChanged(BChangeEventArgs<BTabPanelBase> e)
        {
            blayout?.Refresh();
            btab?.Refresh();
            RequireRender = true;
            this.MarkAsRequireRender();
            StateHasChanged();

        }

        protected override async Task OnInitializedAsync()
        {
            await base.OnInitializedAsync();
            Tabs.Add(new TabItem() { Title = "首页", Category =TopicCategory.Home });
            Tabs.Add(new TabItem() { Title = "提问", Category = TopicCategory.Ask });
            Tabs.Add(new TabItem() { Title = "分享", Category = TopicCategory.Share });
            Tabs.Add(new TabItem() { Title = "讨论", Category = TopicCategory.Discuss });
            Tabs.Add(new TabItem() { Title = "建议", Category = TopicCategory.Suggest });
            Tabs.Add(new TabItem() { Title = "公告", Category = TopicCategory.Notice });

        }

        public class TabItem
        {
            public string Title { get; set; }
            public TopicCategory Category { get; set; }
        }
    }
}
