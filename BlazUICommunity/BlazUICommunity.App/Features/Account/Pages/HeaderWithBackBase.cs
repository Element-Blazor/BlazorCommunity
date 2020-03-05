using Blazui.Component;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Blazui.Community.App.Features.Account.Pages
{
    public class HeaderWithBackBase : BComponentBase
    {
        [Inject]
        NavigationManager navigationManager { get; set; }
        [Parameter]
        public string Title { get; set; }
 
        [Parameter]
        public string Route { get; set; }

        internal void OnBackClick()
        {
            if (!string.IsNullOrWhiteSpace(Route))navigationManager.NavigateTo(Route, true);
        }
    }
}
