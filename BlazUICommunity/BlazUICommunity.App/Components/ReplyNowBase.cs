using Blazui.Community.App.Model;
using Blazui.Community.App.Pages;
using Blazui.Community.DTO;
using Blazui.Community.Model.Models;
using Blazui.Component;
using Blazui.Markdown;
using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web;

namespace Blazui.Community.App.Components
{
    public class ReplyNowBase : PageBase
    {
        
        internal BForm form;
        internal ReplyModel article;
        protected BMarkdownEditor bMarkdownEditor;

        protected async Task ReplyNow()
        {
            var user = await GetUser();

            if (user is null || user.Id == 0)
            {
                ToastWarning("请先登录");
                //await Task.Delay(1000);
                //navigationManager.NavigateTo("/account/signin?returnUrl=" + WebUtility.UrlEncode(new Uri(navigationManager.Uri).PathAndQuery));
                return;
            }

            if (!form.IsValid())
            {
                return;
            }

            var article = form.GetValue<ReplyModel>();
            if (article is null)
            {
                form.Toast("验证不通过");
                return;
            }

            var parsedQuery = HttpUtility.ParseQueryString(new Uri(navigationManager.Uri).Query);
            int.TryParse(parsedQuery["tpid"], out int TopicId);

            if(TopicId<0)
            {
                navigationManager.NavigateTo("/");
                return;
            }
            BZReplyDto bZReplyDto = new BZReplyDto()
            {
                Content = article.Content,
                UserId = user.Id,
                Favor = 0,
                ModifyTime = DateTime.Now,
                PublishTime = DateTime.Now,
                Status = 0,
                TopicId = TopicId
            };

            var addResult = await NetService.AddReply(bZReplyDto);
            if (addResult.IsSuccess)
            {
                ToastSuccess("回复成功");
                await Task.Delay(500);
                navigationManager.NavigateTo(navigationManager.Uri,true);//跳转到+"&golast=1"
            }
            else
            {
                ToastError("回复失败");
                return;
            }
        }

        private void UpdateUI()
        {
            form.MarkAsRequireRender();
            bMarkdownEditor.MarkAsRequireRender();
            form.Refresh();
            bMarkdownEditor.Refresh();
            MarkAsRequireRender();
            StateHasChanged();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        protected override async Task InitilizePageDataAsync()
        {
            await Task.CompletedTask;
        }
    }
}
