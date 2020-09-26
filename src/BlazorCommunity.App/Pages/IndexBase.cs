using BlazorCommunity.App.Service;
using BlazorCommunity.Enums;
using Element;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Primitives;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BlazorCommunity.App.Pages
{
    public class IndexBase : ElementComponentBase
    {
        [Inject]
        private BrowerService  browerService{ get; set; }
        protected List<TabItem> Tabs = new List<TabItem>();
        protected BTab btab;
        protected BLayout blayout;
        protected void ActiveTabChanged(BChangeEventArgs<BTabPanel> e)
        {
         
            blayout?.Refresh();
            btab?.Refresh();
            RequireRender = true;
            this.MarkAsRequireRender();
            StateHasChanged();
        }
        [Inject]
        public ILogger<IndexBase> _logger { get; set; }
        protected override async Task OnInitializedAsync()
        {
            if (browerService.IsMobile())
                NavigationManager.NavigateTo("m/index/");

            Tabs.Add(new TabItem() { Title = "首页", Category = -1 });
            Tabs.Add(new TabItem() { Title = "提问", Category = (int)TopicCategory.Ask });
            Tabs.Add(new TabItem() { Title = "分享", Category = (int)TopicCategory.Share });
            Tabs.Add(new TabItem() { Title = "讨论", Category = (int)TopicCategory.Discuss });
            Tabs.Add(new TabItem() { Title = "建议", Category = (int)TopicCategory.Suggest });
            Tabs.Add(new TabItem() { Title = "公告", Category = (int)TopicCategory.Notice });
            Tabs.Add(new TabItem() { Title = "教程", Category = (int)TopicCategory.Doc });
        }

        public class TabItem
        {
            public string Title { get; set; }
            public int Category { get; set; }
        }
    }
}