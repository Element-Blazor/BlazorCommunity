using AutoMapper;
using Blazui.Community.Admin.Service;
using Blazui.Component;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Blazui.Community.Admin.Data
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

  

        protected virtual Task InitilizePageDataAsync() {
            Console.WriteLine(this.ToString());
            return Task.CompletedTask;
        }
        protected override bool ShouldRender() => true;


        public async Task ConfirmAsync(Action action,string Message= "确定要执行该操作吗?")
        {
            MessageBoxResult Confirm = await MessageBox.ConfirmAsync(Message);
            if (Confirm == MessageBoxResult.Ok)
            {
               action();
            }
            else
            {
                MessageService.Show("您选择了取消", MessageType.Info);
            }
        }
        
    }
}
