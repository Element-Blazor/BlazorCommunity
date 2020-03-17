﻿using Blazui.Community.App.Pages;
using System.Threading.Tasks;

namespace Blazui.Community.App.Components
{
    public abstract class PersonalPageBase : PageBase
    {
        protected string tabTitle { get; set; }

        protected abstract void InitTabTitle();

        protected override bool ShouldRender()
        {
            return true;
        }

        protected override async Task OnParametersSetAsync()
        {
            await base.OnParametersSetAsync();
            InitTabTitle();
        }

        public async Task navigationToUpdateUserUI(string Uri)
        {
            ToastSuccess("更新成功");
            await Task.Delay(200);
            navigationManager.NavigateTo(Uri, true);
        }
    }
}