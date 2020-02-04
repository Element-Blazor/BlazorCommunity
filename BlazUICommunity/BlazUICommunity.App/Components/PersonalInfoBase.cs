using AutoMapper;
using Blazui.Community.App.Model;
using Blazui.Community.App.Pages;
using Blazui.Community.App.Service;
using Blazui.Community.DTO;
using Blazui.Community.Model.Models;
using Blazui.Community.Repository;
using Blazui.Component;
using Blazui.Component.EventArgs;
using Blazui.Component.Form;
using Blazui.Component.Radio;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Identity;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Blazui.Community.App.Components
{
    public class PersonalInfoBase : PageBase
    {
        [Parameter]
        public BForm userInfoForm { get; set; }
        //[Parameter]
        protected BZUserModel User { get; set; }
        [Parameter]
        public bool Disabled { get; set; }

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
     
            await base.OnAfterRenderAsync(firstRender);
            userInfoForm?.MarkAsRequireRender();
            this.MarkAsRequireRender();
            if ( firstRender )
            {
                return;
            }
        }
        protected void OnStatusChanging(BChangeEventArgs<RadioStatus> e)
        {
            e.DisallowChange = Disabled;
        }

        protected override async Task InitilizePageDataAsync()
        {
            var userstatue = await authenticationStateTask;
            User = await userManager.GetUserAsync(userstatue.User);
            //return Task.CompletedTask;
        }

        [Parameter]
        public EventCallback<MouseEventArgs> EditUser { get; set; }
        [Parameter]
        public EventCallback<MouseEventArgs> SaveUser { get; set; }
 
    }

 
}
