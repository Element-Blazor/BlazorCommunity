using AutoMapper;
using Blazui.Community.App.Service;
using Blazui.Community.DTO;
using Blazui.Community.Model.Models;
using Blazui.Community.Utility.Response;
using Blazui.Component;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Configuration;
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

        [Inject]
         IConfiguration Configuration { get; set; }
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
        public string UploadUrl { get; protected set; }
        protected override void OnInitialized()
        {
            base.OnInitialized();
            UploadUrl =Configuration["ServerUrl"]+"/api/upload/";
        }

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            //await base.OnAfterRenderAsync(firstRender);
            if (!firstRender)
            {
                return;
            }
            try
            {
             
                await InitilizePageDataAsync();
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


        protected virtual Task InitilizePageDataAsync() => Task.CompletedTask;
        protected void ToastError(string message = "操作失败")
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


        protected async Task WithFullScreenLoading(Func<Task> action)
        {
            LoadingService.Show(new LoadingOption()
            {
                Background = "rgba(0, 0, 0, 0.1)",
                Text = "",
                IconClass = "el-icon-loading"
            });
            await action();
            LoadingService.CloseFullScreenLoading();
        }

        protected async Task WithFullScreenLoading(Func<Task> action,Action callback)
        {
            LoadingService.Show(new LoadingOption()
            {
                Background = "rgba(0, 0, 0, 0.1)",
                Text = "",
                IconClass = "el-icon-loading"
            });
            await action();
            callback?.Invoke();
            LoadingService.CloseFullScreenLoading();
        }


        public async Task WithFullScreenLoading(Func<Task<BaseResponse>> action, Action callback = null)
        {
            LoadingService.Show(new LoadingOption()
            {
                Background = "rgba(0, 0, 0, 0.1)",
                Text = "",
                IconClass = "el-icon-loading"
            });
            await action();
            callback?.Invoke();
            LoadingService.CloseFullScreenLoading();
        }

        public async Task WithFullScreenLoading(Func<Task<BaseResponse>> action, Action<BaseResponse> callback = null)
        {
            LoadingService.Show(new LoadingOption()
            {
                Background = "rgba(0, 0, 0, 0.1)",
                Text = "",
                IconClass = "el-icon-loading"
            });
           var result= await action();
            callback?.Invoke(result);
            LoadingService.CloseFullScreenLoading();
        }
    }
}
