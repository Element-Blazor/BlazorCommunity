using Blazui.Community.WasmApp.Service;
using Blazui.Community.Response;
using Blazui.Component;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;
using Blazui.Community.DTO;
using System.Net;
using Blazored.LocalStorage;

namespace Blazui.Community.WasmApp.Pages
{
    public abstract class PageBase : BComponentBase
    {
        [Inject]
        private IConfiguration Configuration { get; set; }

        [Inject]
        public ILogger<PageBase> _logger { get; set; }

      

        [Inject]
        public NetworkService NetService { get; set; }

        [Inject]
        public MessageService MessageService { get; set; }

        [Inject]
        public MessageBox MessageBox { get; set; }

        [CascadingParameter]
        public Task<AuthenticationState> authenticationStateTask { get; set; }
        [Inject]
         ILocalStorageService localStorageService { get; set; }

        public string UploadUrl { get; protected set; }

        protected BZUserDto User;
        /// <summary>
        /// 发送验证码，按钮禁用倒计时
        /// </summary>
        public const int CountDownTime = 120;

        protected override void OnInitialized() => UploadUrl = Configuration["ServerUrl"] + "/api/upload/";

        protected async Task NavigateToReturnUrl()
        {

            await Task.Delay(200);
            var returnUrl = NavigationManager.Uri.Contains("returnUrl=") ?
                     WebUtility.UrlDecode(NavigationManager.Uri.Split("returnUrl=")[1]) : "/";
            returnUrl = string.IsNullOrWhiteSpace(returnUrl) ? "/" : returnUrl;
            NavigationManager.NavigateTo(returnUrl);
        }
        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
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
                _logger.LogError($"OnAfterRenderAsync----->>{ex.StackTrace}");
            }
        }

        protected override bool ShouldRender() => true;


        protected virtual async Task<BZUserDto> GetUser()
        {
            try
            {
                var UserId =await localStorageService.GetItemAsync<string>("CurrentUserId");
                if (UserId != null)
                {
                    var user= await NetService.FindUserByIdAsync(UserId);
                    return user;
                }
                return null;
            }
            catch (Exception ex)
            {

                ToastError(ex.Message);
                _logger.LogError($"OnAfterRenderAsync----->>{ex.StackTrace}");
                return null;
            }
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

        protected async Task WithFullScreenLoading(Func<Task> action, Action callback)
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
            var result = await action();
            callback?.Invoke(result);
            LoadingService.CloseFullScreenLoading();
        }
    }
}