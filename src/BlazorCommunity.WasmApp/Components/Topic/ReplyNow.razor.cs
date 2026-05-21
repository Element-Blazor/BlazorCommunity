using BlazorCommunity.WasmApp.Components.Topic;
using BlazorCommunity.WasmApp.Model;
using BlazorCommunity.WasmApp.Pages;
using BlazorCommunity.DTO;
using BlazorCommunity.Model.Models;
using Element;
using Element.Markdown;
using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BlazorCommunity.WasmApp.Components.Topic
{
    public partial class ReplyNow : PageBase
    {
        [Parameter]
        public BZTopicDto Topic { get; set; }

        internal ElForm form;
        internal ElFormItem<string> formItem;
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
        protected async Task SubmitData()
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
            await NewReply(article, user.Id);
        }

        private async Task NewReply(NewReplyModel model, string UserId)
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
                CreatorId = UserId
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
                    await NewReply(model, user.Id);
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