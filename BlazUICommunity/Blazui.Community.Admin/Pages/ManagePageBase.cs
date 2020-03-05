using AutoMapper;
using Blazui.Community.Admin.Service;
using Blazui.Community.Utility.Response;
using Blazui.Component;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Blazui.Community.Admin.Pages
{
    public abstract class ManagePageBase : BComponentBase
    {
        [Inject]
        public ILogger<ManagePageBase> _logger { get; set; }
        [Inject]
        public IMemoryCache MemoryCache { get; set; }
        [Inject]
        public NetworkService NetService { get; set; }

        [Inject]
        public MessageService MessageService { get; set; }
        [Inject]
        public MessageBox MessageBox { get; set; }

        [Inject]
        public IMapper Mapper { get; set; }




        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            await base.OnAfterRenderAsync(firstRender);
            if (!firstRender)
                return;
            try
            {
                await InitilizePageDataAsync();
                RequireRender = true;
                StateHasChanged();
            }
            catch (Exception ex)
            {
                _logger.LogInformation($"OnAfterRenderAsync----->>{ex.StackTrace}");
            }
        }



        protected virtual Task InitilizePageDataAsync()
        {
            Console.WriteLine(this.ToString());
            return Task.CompletedTask;
        }
        protected override bool ShouldRender() => true;


        public async Task ConfirmAsync(Func<Task<BaseResponse>> action, Action<BaseResponse> callback = null, string ConfirmMessage = "确定要执行该操作吗?")
        {
            MessageBoxResult Confirm = await MessageBox.ConfirmAsync(ConfirmMessage);
            if (Confirm == MessageBoxResult.Ok)
            {
                var result = await action();
                if (result.IsSuccess)
                    callback?.Invoke(result);
                MessageService.Show(result.Message, result.IsSuccess ? MessageType.Success : MessageType.Error);
            }
            else
                MessageService.Show("您选择了取消", MessageType.Info);
        }
        public async Task ConfirmAsync(Func<Task<BaseResponse>> action, Action callback = null, string ConfirmMessage = "确定要执行该操作吗?")
        {
            MessageBoxResult Confirm = await MessageBox.ConfirmAsync(ConfirmMessage);
            if (Confirm == MessageBoxResult.Ok)
            {
                var result = await action();
                if (result.IsSuccess)
                    callback?.Invoke();
                MessageService.Show(result.Message, result.IsSuccess ? MessageType.Success : MessageType.Error);
            }
            else
                MessageService.Show("您选择了取消", MessageType.Info);
        }
    }
}
