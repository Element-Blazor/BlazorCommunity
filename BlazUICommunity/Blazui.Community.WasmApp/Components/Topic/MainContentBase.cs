using Blazui.Component;
using Microsoft.AspNetCore.Components;

namespace Blazui.Community.WasmApp.Components
{
    public class MainContentBase : BComponentBase
    {
        [Parameter]
        public string Title { get; set; }

        [Parameter]
        public int TopicType { get; set; }
    }
}