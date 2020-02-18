﻿using Blazui.Community.App.Model;
using Blazui.Community.App.Service;
using Blazui.Community.DTO;
using Blazui.Community.Model.Models;
using Blazui.Component;
using Blazui.Component.EventArgs;
using Blazui.Component.Select;
using Blazui.Markdown;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
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
        protected List<BZVersionModel> bZVersions;
        protected BSelect<int> bSelect;
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
            var User = await GetUser();
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
                UserId = User.Id,
                versionId = 0
            };
            var result = await NetService.AddTopic(bZTopicDto);
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
            article = new ArticleModel() { Title = "", Content = "" };

            await LoadProjects(ProjectType.Blazui);
            await Task.CompletedTask;
        }

        private async Task LoadProjects(ProjectType type)
        {
            var resut = await NetService.GetVersions((int)type);
            if (resut.IsSuccess)
                bZVersions = resut.Data;
            else
                bZVersions = new List<BZVersionModel>();
        }

        internal void GoHome()
        {
            navigationManager.NavigateTo("/");
        }
    }
}
