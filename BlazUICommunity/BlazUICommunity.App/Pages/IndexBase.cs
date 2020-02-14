using Blazui.Community.App.Components;
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
            var title = e.NewValue.Title;
            blayout?.Refresh();
            btab?.Refresh();
            Tabs.First(p => p.Title == title).main.Refresh();
            RequireRender = true;
            this.MarkAsRequireRender();
            StateHasChanged();

        }

        protected override async Task OnInitializedAsync()
        {
            await base.OnInitializedAsync();
            Tabs.Add(new TabItem() { Title = "首页", TopicType = -1, main=new MainContent() });
            Tabs.Add(new TabItem() { Title = "提问", TopicType = 0, main = new MainContent() });
            Tabs.Add(new TabItem() { Title = "分享", TopicType = 1, main = new MainContent() });
            Tabs.Add(new TabItem() { Title = "讨论", TopicType = 2, main = new MainContent() });
            Tabs.Add(new TabItem() { Title = "建议", TopicType = 3, main = new MainContent() });
            Tabs.Add(new TabItem() { Title = "公告", TopicType = 4, main = new MainContent() });

        }

        [Inject]
        public NavigationManager navigationManager { get; set; }

        internal void NewTopic()
        {
            navigationManager.NavigateTo("new", true);
        }

        public class TabItem
        {
            public string Title { get; set; }
            public int TopicType { get; set; }
            public MainContent  main { get; set; }
        }
    }
}
