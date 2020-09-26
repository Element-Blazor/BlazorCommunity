using BlazorCommunity.App.Components;
using BlazorCommunity.App.Service;
using Element;
using Microsoft.AspNetCore.Components;
using System;
using System.Threading.Tasks;

namespace BlazorCommunity.App.Shared
{
    public class MainLayoutBase : LayoutComponentBase
    {
       
        [Parameter]
        public RenderFragment ChildContent { get; set; }

      
    }
}