using Blazui.Community.App.Model;
using Blazui.Component;
using Blazui.Component.Container;
using Blazui.Component.NavMenu;
using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using Blazui.Community.App.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Blazui.Component.Form;
using Blazui.Community.Model.Models;
using Microsoft.AspNetCore.Identity;
using Blazui.Community.App.Service;
using Blazui.Community.DTO;
using AutoMapper;
using Blazui.Community.Repository;

namespace Blazui.Community.App.Pages
{
    public class PersonCenterBase : PageBase
    {
        protected BLayout bLayout { get; set; }
        protected BTab bTab { get; set; }

        public int CurrentMenuIndex = 0;

        protected PersonalInfo personalInfo { get; set; }
        protected PersonalTopic top { get; set; }
        protected PersonalSet ava { get; set; }
        protected bool Disabled { get; set; }
        protected override async Task InitilizePageDataAsync()
        {
            Disabled = true;
         await   Task.CompletedTask;
        }

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            await base.OnAfterRenderAsync(firstRender);

            if ( firstRender )
            {
                RefreshUI();
            }
        }
        private void RefreshUI()
        {
            personalInfo?.MarkAsRequireRender();
            top?.MarkAsRequireRender();
            ava?.MarkAsRequireRender();
            bLayout.MarkAsRequireRender();
            bTab.MarkAsRequireRender();
         
            MarkAsRequireRender();
            StateHasChanged();
        }
        protected void EditUser()
        {
            Disabled = false;
            RefreshUI();
        }

        protected async void SaveUser()
        {
            if ( personalInfo.userInfoForm.IsValid() )
            {
                IdentityResult result = await UpdateUser();
                if ( result.Succeeded )
                {
                    Disabled = true;
                    RefreshUI();
                    MessageService.Show("更新成功" , MessageType.Success);
                    //await  Task.Delay(1000);
                   
                    //navigationManager.NavigateTo("/user" , true);
                }
                else
                {
                    MessageService.Show("更新失败" , MessageType.Error);
                }
            }
            else
            {
                MessageService.Show("更新失败" , MessageType.Error);
            }

        }

        private async Task<IdentityResult> UpdateUser()
        {
            var model = personalInfo.userInfoForm.GetValue<BZUserModel>();
            var user = await userManager.FindByNameAsync(model.UserName);
            user.NickName = model.NickName;
            user.Sex = model.Sex;
            user.Signature = model.Signature;
            user.Email = model.Email;
            user.Avatar = model.Avatar;
            user.Mobile = model.Mobile;
            user.LastLoginAddr = model.LastLoginAddr;
            var result = await userManager.UpdateAsync(user);
            if(result.Succeeded)
            {
                BzUser = user;
            }
            return result;
        }


        protected void ChangeMenuIndex(int index)
        {
            if ( CurrentMenuIndex == index )
                return;
            CurrentMenuIndex = index;
            RefreshUI();
        }
    }
}
