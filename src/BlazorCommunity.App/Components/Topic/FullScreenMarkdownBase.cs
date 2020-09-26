using BlazorCommunity.App.Model;
using BlazorCommunity.Enums;
using Element;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.Extensions.Configuration;
using System.Threading.Tasks;

namespace BlazorCommunity.App.Components.Topic
{
    public class FullScreenMarkdownBase : BDialogBase
    {
        [Inject]
        private MessageBox MessageBox { get; set; }

        [Inject]
        private MessageService MessageService { get; set; }

        [Inject]
        private IConfiguration Configuration { get; set; }

        internal string UploadApiUrl { get; private set; }

        [Parameter]
        public NewReplyModel model { get; set; }

        internal BForm form;

        [CascadingParameter]
        private Task<AuthenticationState> authenticationStateTask { get; set; }

        protected override void OnInitialized()
        {
            base.OnInitialized();
            UploadApiUrl = Configuration["ServerUrl"] + "/api/upload/" + UploadPath.Topic.Description();
        }

        internal async Task ReplyNow()
        {
            var model = form.GetValue<NewReplyModel>();
            if (string.IsNullOrWhiteSpace(model?.Content))
                await CloseAsync(new NewReplyModel());
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