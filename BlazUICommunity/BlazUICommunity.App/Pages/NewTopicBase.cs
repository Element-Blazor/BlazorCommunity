using Blazui.Community.App.Model;
using Blazui.Community.App.Service;
using Blazui.Community.DTO;
using Blazui.Component;
using Blazui.Markdown;
using Microsoft.AspNetCore.Authorization;
using System;
using System.Threading.Tasks;

namespace Blazui.Community.App.Pages
{
    [Authorize]
    public partial class NewTopicBase : PageBase
    {
        internal BForm form;
        internal ArticleModel article;
        protected BMarkdownEditor bMarkdownEditor;
        protected TopicType _TopicType;
        protected override void OnInitialized()
        {
            base.OnInitialized();
        }

        protected override Task OnAfterRenderAsync(bool firstRender)
        {
            if (!firstRender)
                return Task.CompletedTask;
            return base.OnAfterRenderAsync(firstRender);
        }

        protected async Task Submit()
        {
            if (!form.IsValid())
            {
                return;
            }
            var article = form.GetValue<ArticleModel>();
            if (article is null)
            {
                form.Toast("验证不通过");
                return;
            }
            await AddTopic(article);
        }

        private async Task AddTopic(ArticleModel article)
        {
            BZTopicDto bZTopicDto = new BZTopicDto()
            {
                Content = article.Content,
                Good = 0,
                Hot = 0,
                ModifyTime = DateTime.Now,
                PublishTime = DateTime.Now,
                ReplyCount = 0,
                Status = 0,
                Title = article.Title,
                TopicType = (int)article.TopicType,
                Top = 0,
                //UserId = BzUser.Id
            };
            var userstatue = await authenticationStateTask;
            var User = await userManager.GetUserAsync(userstatue.User);
            bZTopicDto.UserId = User.Id;
            var result = await ProductService.AddTopic(bZTopicDto);
            if (result.IsSuccess)
            {
                MessageService.Show($"发布成功", MessageType.Success);
                navigationManager.NavigateTo("/", true);
            }
            else
            {
                MessageService.Show($"发布失败{result.Message}", MessageType.Error);
            }
        }

        protected override Task InitilizePageDataAsync()
        {
            article = new ArticleModel() { Title = "", Content = "" };
            return Task.CompletedTask;
        }
    }
}
