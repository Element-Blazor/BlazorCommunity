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
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Blazui.Community.App.Pages
{
    public abstract class PageBase : BComponentBase
    {
        public static string UploadHeadUrl= "api/upload/uploadavator";

        public static string UploadTopicFileUrl = "api/upload/UploadFile";
        [Inject]
        public ILogger<PageBase> _logger { get; set; }
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
                ToastError(ex.Message);
                await InitilizePageDataAsync();
                _logger.LogError($"OnAfterRenderAsync----->>{ex.StackTrace}");
            }
        }

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


        protected override bool ShouldRender() => true;
       
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
                _logger.LogInformation($"PageBase-->>GetUser------->>{ex.StackTrace}");
                return null;
            }
            finally
            {
                semaphoreSlim.Release();
            }

        }
        protected async Task<List<BZVersionModel>> QueryVersions()
        {
            return await memoryCache.GetOrCreateAsync("Version", async p =>
            {
                p.SetSlidingExpiration(TimeSpan.FromMinutes(10));
                var result= await NetService.GetAllVersions();
                if (result.IsSuccess)
                    return result.Data;
                else return new List<BZVersionModel>();
            });
        }

        private async Task<BZUserModel> QueryUser()
        {
            var userstatue = await authenticationStateTask;
            if (userstatue.User.Identity.IsAuthenticated)
            {
                return await memoryCache.GetOrCreateAsync(userstatue.User, async p =>
                {
                    p.SetSlidingExpiration(TimeSpan.FromMinutes(10));
                    return await userManager.GetUserAsync(userstatue.User);
                });
            }
            else return null;
        }
        protected virtual Task InitilizePageDataAsync() {
         
            return Task.CompletedTask;
        }
     protected void ToastError(string message="操作失败")
        {
            MessageService.Show(message, MessageType.Error);
        }
        protected void ToastInfo(string message = "普通消息")
        {
            MessageService.Show(message, MessageType.Info);
        }
        protected void ToastSuccess(string message = "操作成功")
        {
            MessageService.Show(message, MessageType.Success);
        }
        protected void ToastWarning(string message = "警告消息")
        {
            MessageService.Show(message, MessageType.Warning);
        }
    }
}
