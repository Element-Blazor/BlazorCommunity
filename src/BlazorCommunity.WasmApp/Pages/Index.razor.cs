using BlazorCommunity.Enums;
using BlazorCommunity.WasmApp.Service;
using Element;
using Microsoft.AspNetCore.Components;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components.Rendering;

namespace BlazorCommunity.WasmApp.Pages
{
    public partial class Index : ElementComponentBase
    {
        protected List<TabItem> Tabs = new List<TabItem>();
        protected BTab btab;
        protected BLayout blayout;
        [Inject]
         BrowerService browerService { get; set; }

        
        protected void ActiveTabChanged(BChangeEventArgs<BTabPanel> e)
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
            Tabs.Add(new TabItem() { Title = "教程", Category = (int)TopicCategory.Doc });
        }
        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            await base.OnAfterRenderAsync(firstRender);
            if(firstRender)
            {
                if (await browerService.IsMobile())
                    NavigationManager.NavigateTo("m/index/");
            }
        }

        public class TabItem
        {
            public string Title { get; set; }
            public int Category { get; set; }
        }
    }
}