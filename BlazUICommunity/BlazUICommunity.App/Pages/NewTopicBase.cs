using Blazui.Community.App.Model;
using Blazui.Community.App.Service;
using Blazui.Community.DTO;
using Blazui.Community.Enums;
using Blazui.Community.Model.Models;
using Blazui.Component;
using Blazui.Component.EventArgs;
using Blazui.Component.Select;
using Blazui.Markdown;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Caching.Memory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Blazui.Community.App.Pages
{
    [Authorize]
    public partial class NewTopicBase : PageBase
    {
        internal BForm form;
        internal TopicFormModel article;
        protected BMarkdownEditor bMarkdownEditor;
        protected TopicCategory _TopicType;
        protected List<BZVersionDto> bZVersions;
        internal BSelect<string> bverNoSelect;

        protected override void OnInitialized()
        {
            base.OnInitialized();
        }

        protected async Task Submit()
        {
            if (!form.IsValid())
            {
                return;
            }
            var article = form.GetValue<TopicFormModel>();
            if (article is null)
            {
                form.Toast("验证不通过");
                return;
            }
            if (article.Title.Length > 100)
            {
                form.Toast("标题不能超过100");
                return;
            }
            await AddTopic(article);
        }

        private async Task AddTopic(TopicFormModel article)
        {
            var User = await GetUser();

            BZTopicDto bZTopicDto = new BZTopicDto()
            {
                Content = article.Content,
                Title = article.Title,
                Category = (int)article.Category,
                CreatorId= User.Id,
                VersionId = bZVersions.FirstOrDefault(p=>p.VerNo== article.VerNo)?.Id,
                LastModifyDate = DateTime.Now,
                CreateDate = DateTime.Now,
                Good = 0,
                Hot = 0,
                ReplyCount = 0,
                Status = 0,
                Top = 0,
               
            };

       await    WithFullScreenLoading(async ()=> {
           var result = await NetService.AddTopic(bZTopicDto);

           if (result.IsSuccess)
           {
               ToastSuccess("发布成功");
               await Task.Delay(100);
               navigationManager.NavigateTo($"/topic/{result.Data}");
           }
           else
           {
               ToastError($"发布失败{result.Message}");
           }
       });
        }
        protected async Task OnChange(ProjectType value)
        {
            await   LoadProjects(value);
            form?.Refresh();
            RequireRender = true;
            bverNoSelect?.Refresh();
            MarkAsRequireRender();
            StateHasChanged();
        }

        protected override async Task InitilizePageDataAsync()
        {

            article = new TopicFormModel() { Title = "", Content = "" };
            await LoadProjects(ProjectType.Blazui);
        }

        private async Task LoadProjects(ProjectType type)
        {
            var resut = await QueryVersions();
            bZVersions = resut.Where(p => p.Project == (int)type).ToList();
        }
        protected async Task<List<BZVersionDto>> QueryVersions()
        {
            return await memoryCache.GetOrCreateAsync("Version", async p =>
            {
                p.SetSlidingExpiration(TimeSpan.FromMinutes(10));
                var result = await NetService.GetAllVersions();
                if (result.IsSuccess)
                    return result.Data;
                else return new List<BZVersionDto>();
            });
        }
        internal void GoHome()
        {
            navigationManager.NavigateTo("/");
        }
    }
}
