using BlazorCommunity.WasmApp.Model;
using BlazorCommunity.DTO;
using BlazorCommunity.Enums;
using Blazui.Component;
using Blazui.Markdown;
using Microsoft.AspNetCore.Authorization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BlazorCommunity.WasmApp.Pages
{
    [Authorize]
    public partial class NewTopic : PageBase
    {
        internal BForm form;
        internal NewTopicModel article;
        protected BMarkdownEditor bMarkdownEditor;
        protected TopicCategory _TopicType;
        protected List<BZVersionDto> bZVersions;
        internal BSelect<string> bverNoSelect;

        public string SelectValue { get; set; }

        protected async Task Submit()
        {
            if (!form.IsValid())
            {
                return;
            }
            var article = form.GetValue<NewTopicModel>();
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

        private async Task AddTopic(NewTopicModel newTopicModel)
        {
            var User = await GetUser();

            BZTopicDto bZTopicDto = new BZTopicDto()
            {
                Content = newTopicModel.Content,
                Title = newTopicModel.Title,
                Category = (int)newTopicModel.Category,
                CreatorId = User.Id,
                VersionId = bZVersions.FirstOrDefault(p => p.VerNo == newTopicModel.VerNo)?.Id,
                LastModifyDate = DateTime.Now,
                CreateDate = DateTime.Now,
                Good = 0,
                Hot = 0,
                ReplyCount = 0,
                Status = 0,
                Top = 0,
            };

            await WithFullScreenLoading(async () =>
            {
                var result = await NetService.AddTopic(bZTopicDto, newTopicModel.Notice);

                if (result.IsSuccess)
                {
                    ToastSuccess("发布成功");
                    await Task.Delay(100);
                    NavigationManager.NavigateTo($"/topic/{result.Data}");
                }
                else
                {
                    ToastError($"发布失败{result.Message}");
                }
            });
        }

        //protected async Task OnChange(ProjectType value)
        //{
        //    await LoadProjects(value);
        //    form?.Refresh();
        //    RequireRender = true;
        //    bverNoSelect?.Refresh();
        //    MarkAsRequireRender();
        //    StateHasChanged();
        //}

        protected override async Task InitilizePageDataAsync()
        {
            await LoadProjects();
            article = new NewTopicModel() { Title = "", Content = "", VerNo= bZVersions.FirstOrDefault()?.VerNo };
        }

        private async Task LoadProjects()
        {
            var resut = await QueryVersions();
            bZVersions = resut?.ToList();
        }

        protected async Task<List<BZVersionDto>> QueryVersions()
        {
            var result = await NetService.QueryAllVersions();
            if (result.IsSuccess)
                return result.Data;
            else return new List<BZVersionDto>();
        }
    }
}