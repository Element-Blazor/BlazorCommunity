﻿using BlazorCommunity.DTO;
using Element;
using Microsoft.AspNetCore.Components;
using System.Threading.Tasks;

namespace BlazorCommunity.WasmApp.Components.Topic
{
    public partial class TopicItem : ElementComponentBase
    {
        [Parameter]
        public BZTopicDto Topic { get; set; }

        [Inject]
        public NavigationManager navigationManager { get; set; }

      

        protected void NavigationToReply(string topicId)
        {
            if (topicId == null || string.IsNullOrWhiteSpace(topicId))
                return;
            NavigationManager.NavigateTo($"/topic/{topicId}");
        }
    }
}