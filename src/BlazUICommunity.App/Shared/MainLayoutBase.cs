using Blazui.Community.App.Components;
using Blazui.Community.App.Service;
using Blazui.Component;
using Microsoft.AspNetCore.Components;
using System;
using System.Threading.Tasks;

namespace Blazui.Community.App.Shared
{
    public class MainLayoutBase : LayoutComponentBase
    {
       
        [Parameter]
        public RenderFragment ChildContent { get; set; }

      
    }
}