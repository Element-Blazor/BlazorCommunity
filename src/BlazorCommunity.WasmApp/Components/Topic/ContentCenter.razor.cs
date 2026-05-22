using Element;
using Microsoft.AspNetCore.Components;

namespace BlazorCommunity.WasmApp.Components.Topic
{
    public partial class ContentCenter : ElementComponentBase
    {
        protected ElTabs tab;
        protected ElTabPane tabPanel_Comprehensive;
        protected ElTabPane tabPanel_Hot;
        protected ElTabPane tabPanel_Best;
        protected ElTabPane tabPanel_End;

        [Parameter]
        public int TopicType { get; set; }

        protected void ActiveTabChanged(ElementChangeEventArgs<ElTabPane> e)
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
