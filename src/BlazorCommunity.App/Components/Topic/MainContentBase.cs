using Blazui.Component;
using Microsoft.AspNetCore.Components;

namespace BlazorCommunity.App.Components
{
    public class MainContentBase : BComponentBase
    {
        [Parameter]
        public string Title { get; set; }

        [Parameter]
        public int TopicType { get; set; }
    }
}