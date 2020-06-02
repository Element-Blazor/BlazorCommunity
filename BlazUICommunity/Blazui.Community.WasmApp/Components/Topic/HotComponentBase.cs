using Blazui.Community.WasmApp.Service;
using Blazui.Community.DTO;
using Blazui.Community.Response;
using Blazui.Component;
using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Blazui.Community.WasmApp.Components.Topic
{
    public abstract class HotComponentBase : BComponentBase
    {
        [Inject]
       public ILocalStorageCacheService localStorage { get; set; }
        [Inject]
        public NetworkService Service { get; set; }
        [Parameter]
        public virtual string Title { get; set; }
        [Parameter]
        public virtual List<HotTopicDto> Datas { get; set; } = new List<HotTopicDto>();

        protected void NavigateToTopic(string TopicId)
        {
                NavigationManager.NavigateTo($"/topic/{TopicId}",true);
        }

        protected override bool ShouldRender() => true;



        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            if (firstRender)
            {
                var Result = await InitDatas();
                    Datas = Result;
                    MarkAsRequireRender();
                    StateHasChanged();
            }
        }


        protected abstract Task<List<HotTopicDto>> InitDatas();

    }
}
