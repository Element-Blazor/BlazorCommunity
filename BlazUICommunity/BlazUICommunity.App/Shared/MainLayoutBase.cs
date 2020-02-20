using Blazui.Community.App.Components;
using Blazui.Community.Model.Models;
using Blazui.Component;
using Blazui.Component.Button;
using Blazui.Component.Container;
using Blazui.Component.NavMenu;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace Blazui.Community.App.Shared
{
    public class MainLayoutBase : LayoutComponentBase
    {
        protected BLayout Blayout { get; set; }
        protected AppHeader Appheader { get; set; }
        protected BCard Bcard { get; set; }

        //protected override async Task OnParametersSetAsync()
        //{
        //    try
        //    {
        //        await base.OnParametersSetAsync();
        //    }
        //    catch (Exception ex)
        //    {
        //        Console.WriteLine("OnParametersSetAsync" + ex.Message);
        //    }
        //}
        //protected override async Task OnInitializedAsync()
        //{
        //    try
        //    {
        //        await base.OnInitializedAsync();
        //    }
        //    catch (Exception ex)
        //    {
        //        Console.WriteLine("OnInitializedAsync" + ex.Message);
        //    }
        //}
        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            try
            {
                await base.OnAfterRenderAsync(firstRender);
                if (!firstRender)
                    return;

                Blayout.Refresh();
                Appheader.Refresh();
                Bcard.Refresh();
            }
            catch (Exception ex)
            {
                Console.WriteLine("OnAfterRenderAsync"+ex.Message);
            }
        }
    }
}
