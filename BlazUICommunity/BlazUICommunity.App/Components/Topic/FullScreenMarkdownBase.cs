using Blazui.Community.App.Model;
using Blazui.Community.Enums;
using Blazui.Component;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.Extensions.Configuration;
using System.Threading.Tasks;

namespace Blazui.Community.App.Components.Topic
{

    public class FullScreenMarkdownBase : BDialogBase
    {

        [Inject]
        MessageBox MessageBox { get; set; }
        [Inject]
        MessageService MessageService { get; set; }
        [Inject]
        IConfiguration Configuration { get; set; }
        internal string UploadApiUrl { get; private set; }
        [Parameter]
        public ReplyModel model { get; set; }
        internal BForm form;
        [CascadingParameter]
        Task<AuthenticationState> authenticationStateTask { get; set; }
        protected override void OnInitialized()
        {
            base.OnInitialized();
            UploadApiUrl = Configuration["ServerUrl"] + "/api/upload/" + UploadPath.Topic.Description();
        }



        internal async Task ReplyNow()
        {
            var model = form.GetValue<ReplyModel>();
            if (string.IsNullOrWhiteSpace(model?.Content))
                await CloseAsync(new ReplyModel());
            else
            {
                MessageBoxResult Confirm = await MessageBox.ConfirmAsync("是否要提交回复");
                if (Confirm == MessageBoxResult.Ok)
                {
                    if ((await authenticationStateTask).User.Identity.IsAuthenticated)
                        await CloseAsync(model);
                    else
                        MessageService.Show("请登录后再回复帖子", MessageType.Warning);
                }
                else
                    MessageService.Show("您选择了取消", MessageType.Info);
            }
        }

    }
}
