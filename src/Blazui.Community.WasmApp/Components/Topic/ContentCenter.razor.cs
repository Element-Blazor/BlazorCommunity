using Blazui.Component;
using Microsoft.AspNetCore.Components;

namespace Blazui.Community.WasmApp.Components.Topic
{
    public partial class ContentCenter : BComponentBase
    {
        protected BTab tab;
        protected BTabPanel tabPanel_Comprehensive;
        protected BTabPanel tabPanel_Hot;
        protected BTabPanel tabPanel_Best;
        protected BTabPanel tabPanel_End;

        [Parameter]
        public int TopicType { get; set; }

        protected void ActiveTabChanged(BChangeEventArgs<BTabPanelBase> e)
        {
            var title = e.NewValue.Title;
            switch (title)
            {
                case "综合":
                    tabPanel_Comprehensive?.Refresh();
                    break;

                case "人气":
                    tabPanel_Hot?.Refresh();
                    break;

                case "精华":
                    tabPanel_Best?.Refresh();
                    break;

                case "已结":
                    tabPanel_End?.Refresh();
                    break;
            }
            UpdateUI();
        }

        private void UpdateUI()
        {
            RequireRender = true;
            tab?.Refresh();
            StateHasChanged();
        }
    }
}