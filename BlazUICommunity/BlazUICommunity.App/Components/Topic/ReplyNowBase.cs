﻿using Blazui.Community.App.Components.Topic;
using Blazui.Community.App.Model;
using Blazui.Community.App.Pages;
using Blazui.Community.DTO;
using Blazui.Community.Model.Models;
using Blazui.Component;
using Blazui.Markdown;
using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Blazui.Community.App.Components
{
    public class ReplyNowBase : PageBase
    {
        [Parameter]
        public string TopicId { get; set; }

        internal BForm form;
        internal BFormItem<string> formItem;
        protected BMarkdownEditor bMarkdownEditor;
        internal ReplyModel Model = new ReplyModel();

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

            var article = form.GetValue<ReplyModel>();
            await NewReply(article, user);
        }

        private async Task NewReply(ReplyModel model, BZUserModel userModel)
        {
            if (string.IsNullOrWhiteSpace(model.Content))
            {
                form.Toast("还是写点什么吧");
                return;
            }

            if (string.IsNullOrWhiteSpace(TopicId))
            {
                navigationManager.NavigateTo("/");
                return;
            }
            var topicResult = await NetService.QueryTopicById(TopicId);
            if (!topicResult.IsSuccess)
            {
                ToastError($"主贴已被删除");
                return;
            }
            BZReplyDto bZReplyDto = new BZReplyDto()
            {
                Content = model.Content,
                UserId = topicResult.Data?.CreatorId,
                Favor = 0,
                CreateDate = DateTime.Now,
                LastModifyDate = DateTime.Now,
                Status = 0,
                TopicId = TopicId,
                CreatorId = userModel.Id
            };

            await WithFullScreenLoading(async () =>
            {
                var addResult = await NetService.AddReply(bZReplyDto);
                if (addResult.IsSuccess)
                {
                    //article = new ReplyModel() { Content = "" };
                    //form.Reset();
                    ////bMarkdownEditor?.Refresh();
                    ////RequireRender = true;
                    ////StateHasChanged();
                    //ToastSuccess("回复成功");
                    ////await Task.Delay(500);
                    //if (OnReplySuccess.HasDelegate)
                    //{
                    //    await OnReplySuccess.InvokeAsync("");
                    //}
                    navigationManager.NavigateTo(navigationManager.Uri, true);//跳转到+"&golast=1"
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
            var replyModel = form.GetValue<ReplyModel>();
            Dictionary<string, object> values = new Dictionary<string, object>
            {
                { "model", replyModel }
            };
            var dialogResult = await DialogService.ShowDialogAsync<FullScreenMarkdown>("", true, values);
            if (dialogResult.Result is ReplyModel model && !string.IsNullOrWhiteSpace(model.Content))
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
                Model = new ReplyModel();
                this.MarkAsRequireRender();
                StateHasChanged();
            }
        }
    }
}