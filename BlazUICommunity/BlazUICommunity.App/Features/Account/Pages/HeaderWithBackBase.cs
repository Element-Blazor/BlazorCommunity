using Blazui.Component;
using Microsoft.AspNetCore.Components;

namespace Blazui.Community.App.Features.Account.Pages
{
    public class HeaderWithBackBase : BComponentBase
    {
        [Inject]
        private NavigationManager navigationManager { get; set; }

        [Parameter]
        public string Title { get; set; }

        [Parameter]
        public string Route { get; set; }

        internal void OnBackClick()
        {
            if (!string.IsNullOrWhiteSpace(Route)) navigationManager.NavigateTo(Route, true);
        }
    }
}