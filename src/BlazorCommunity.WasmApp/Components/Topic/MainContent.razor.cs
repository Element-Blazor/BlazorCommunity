using Element;
using Microsoft.AspNetCore.Components;

namespace BlazorCommunity.WasmApp.Components.Topic
{
    public partial class MainContent : ElementComponentBase
    {
        [Parameter]
        public string Title { get; set; }

        [Parameter]
        public int TopicType { get; set; }
    }
}