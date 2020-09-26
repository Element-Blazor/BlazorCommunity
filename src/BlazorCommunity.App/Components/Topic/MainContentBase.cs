using Element;
using Microsoft.AspNetCore.Components;

namespace BlazorCommunity.App.Components
{
    public class MainContentBase : ElementComponentBase
    {
        [Parameter]
        public string Title { get; set; }

        [Parameter]
        public int TopicType { get; set; }
    }
}