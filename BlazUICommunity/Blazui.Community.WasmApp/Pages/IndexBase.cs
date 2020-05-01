using Blazui.Community.Enums;
using Blazui.Component;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Blazui.Community.WasmApp.Pages
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
            Tabs.Add(new TabItem() { Title = "首页", Category = -1 });
            Tabs.Add(new TabItem() { Title = "提问", Category = (int)TopicCategory.Ask });
            Tabs.Add(new TabItem() { Title = "分享", Category = (int)TopicCategory.Share });
            Tabs.Add(new TabItem() { Title = "讨论", Category = (int)TopicCategory.Discuss });
            Tabs.Add(new TabItem() { Title = "建议", Category = (int)TopicCategory.Suggest });
            Tabs.Add(new TabItem() { Title = "公告", Category = (int)TopicCategory.Notice });
        }

        public class TabItem
        {
            public string Title { get; set; }
            public int Category { get; set; }
        }
    }
}