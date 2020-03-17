using Blazui.Community.Response;
using Blazui.Component;
using System;
using System.Threading.Tasks;

namespace Blazui.Community.Admin.Service
{
    public class ConfirmService
    {
        public ConfirmService(MessageService messageService, MessageBox messageBox)
        {
            MessageService = messageService;
            MessageBox = messageBox;
        }

        private readonly MessageService MessageService;
        private readonly MessageBox MessageBox;

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