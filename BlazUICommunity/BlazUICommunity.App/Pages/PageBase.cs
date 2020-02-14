using AutoMapper;
using Blazui.Community.App.Service;
using Blazui.Community.App.Shared;
using Blazui.Community.DTO;
using Blazui.Community.Model.Models;
using Blazui.Component;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Caching.Memory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Blazui.Community.App.Pages
{
    public abstract class PageBase : BComponentBase
    {
        [Inject]
        public IMemoryCache memoryCache { get; set; }
        [Inject]
        public NetworkService NetService { get; set; }

        [Inject]
        public MessageService MessageService { get; set; }
        [Inject]
        public MessageBox MessageBox { get; set; }

        [Inject]
        public IMapper mapper { get; set; }

        [CascadingParameter]
        public Task<AuthenticationState> authenticationStateTask { get; set; }
        [Inject]
        public UserManager<BZUserModel> userManager { get; set; }
        [Inject]
        public NavigationManager navigationManager { get; set; }

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            await base.OnAfterRenderAsync(firstRender);
            if (!firstRender)
            {
                return;
            }
            try
            {
                LoadingService.Show(new LoadingOption()
                {
                    Background = "rgba(0, 0, 0, 0.1)",
                    Text = "",
                    IconClass = "el-icon-loading"
                });
                await InitilizePageDataAsync();
                LoadingService.CloseFullScreenLoading();
                RequireRender = true;

                StateHasChanged();
            }
            catch (Exception ex)
            {
                //await Task.Delay(100);
                //await InitilizePageDataAsync();
                //LoadingService.CloseFullScreenLoading();
                //RequireRender = true;
                //StateHasChanged();
                Console.WriteLine(ex.Message);
            }
        }

        protected override bool ShouldRender()
        {
            return true;
        }
        static readonly SemaphoreSlim semaphoreSlim = new SemaphoreSlim(1, 1);
        protected virtual async Task<BZUserModel> GetUser()
        {
            await semaphoreSlim.WaitAsync();
            try
            {
                return await QueryUser();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return null;
            }
            finally
            {
                semaphoreSlim.Release();
            }

        }

        private async Task<BZUserModel> QueryUser()
        {
            var userstatue = await authenticationStateTask;
            if (userstatue.User.Identity.IsAuthenticated)
                return await userManager.GetUserAsync(userstatue.User);
            else return null;
        }
        protected abstract Task InitilizePageDataAsync();
    }
}
