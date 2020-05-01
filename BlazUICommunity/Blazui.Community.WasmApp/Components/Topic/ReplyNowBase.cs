using Blazui.Community.WasmApp.Components.Topic;
using Blazui.Community.WasmApp.Model;
using Blazui.Community.WasmApp.Pages;
using Blazui.Community.DTO;
using Blazui.Community.Model.Models;
using Blazui.Component;
using Blazui.Markdown;
using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Blazui.Community.WasmApp.Components
{
    public class ReplyNowBase : PageBase
    {
        [Parameter]
        public BZTopicDto Topic { get; set; }

        internal BForm form;
        internal BFormItem<string> formItem;
        protected BMarkdownEditor bMarkdownEditor;

        private NewReplyModel _model;
        [Parameter]
        public NewReplyModel Model { get; set; }
        //{
        //    get { return _model; }
        //    set
        //    {
        //        _model = value;
        //        form?.MarkAsRequireRender();
        //        formItem?.MarkAsRequireRender();
        //        bMarkdownEditor?.MarkAsRequireRender();
        //        this.MarkAsRequireRender();
        //        StateHasChanged();
        //    }
        //}
      


        [Parameter] public EventCallback OnReplySuccess { get; set; }

        /// <summary>
        /// 遗留问题 回复成功后 form.reset没有清空markdown的数据
        /// </summary>
        /// <returns></returns>
        protected async Task ReplyNow()
        {
            var user = await GetUser();

            if (string.IsNullOrWhiteSpace(user?.Id))
            {
                ToastWarning("请登录");
                return;
            }

            if (!form.IsValid())
            {
                return;
            }

            var article = form.GetValue<NewReplyModel>();
            await NewReply(article, user);
        }

        private async Task NewReply(NewReplyModel model, BZUserDto userModel)
        {
            if (string.IsNullOrWhiteSpace(model.Content))
            {
                form.Toast("还是写点什么吧");
                return;
            }

            if (Topic is null)
            {
                ToastError($"主贴不存在或已被删除");
                NavigationManager.NavigateTo("/");
                return;
            }
          
            BZReplyDto bZReplyDto = new BZReplyDto()
            {
                Content = model.Content,
                UserId =    Topic.CreatorId,
                Favor = 0,
                CreateDate = DateTime.Now,
                LastModifyDate = DateTime.Now,
                Status = 0,
                TopicId = Topic.Id,
                CreatorId = userModel.Id
            };

            await WithFullScreenLoading(async () =>
            {
                var addResult = await NetService.AddReply(bZReplyDto);
                if (addResult.IsSuccess)
                {
                    NavigationManager.NavigateTo(NavigationManager.Uri, true);//跳转到+"&golast=1"
                }
                else
                {
                    ToastError("回复失败");
                    return;
                }
            });
        }
       

        protected override bool ShouldRender() => true;

        internal async Task FullScreen()
        {
            //ToastWarning("全屏markdon编辑器，尚未实现");
            var replyModel = form.GetValue<NewReplyModel>();
            Dictionary<string, object> values = new Dictionary<string, object>
            {
                { "model", replyModel }
            };
            var dialogResult = await DialogService.ShowFullScreenDialogAsync<FullScreenMarkdown>("", values);
            if (dialogResult.Result is NewReplyModel model && !string.IsNullOrWhiteSpace(model.Content))
            {
                Model = model;
                var user = await GetUser();
                if (string.IsNullOrWhiteSpace(user?.Id))
                    ToastWarning("请登录");
                else
                    await NewReply(model, user);
            }
            else
            {
                Model = new NewReplyModel();
                this.MarkAsRequireRender();
                StateHasChanged();
            }
        }
    }
}