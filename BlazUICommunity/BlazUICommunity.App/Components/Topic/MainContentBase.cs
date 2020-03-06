using Blazui.Community.DTO;
using Blazui.Community.Enums;
using Blazui.Component;
using Microsoft.AspNetCore.Components;

namespace Blazui.Community.App.Components
{
    public class MainContentBase:BComponentBase
    {
        [Parameter]
        public string Title { get; set; }

        [Parameter]
        public int TopicType { get; set; }

      
    }
}
