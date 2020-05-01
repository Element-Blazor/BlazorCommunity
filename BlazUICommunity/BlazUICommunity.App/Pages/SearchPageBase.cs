using Blazui.Community.DTO;
using Microsoft.AspNetCore.Components;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Blazui.Community.App.Pages
{
    public class SearchPageBase : PageBase
    {
        [Parameter]
        public string Text { get; set; }

        internal int currentPage = 1;
        internal int PageSize = 15;

        internal List<SeachTopicDto> Datas { get; set; } = new List<SeachTopicDto>();
        internal int Total = 0;

        protected override void OnInitialized()
        {
            if (string.IsNullOrWhiteSpace(Text))
            {
                NavigationManager.NavigateTo($"/");
            }
        }

        protected override async Task OnInitializedAsync() => await LoadDatas(currentPage);

        internal async Task LoadDatas(int page)
        {
            currentPage = page;
            var searchResult = await NetService.SeachTopicByTitle(Text, currentPage, PageSize);
            if (searchResult.Data != null)
            {
                Datas = searchResult?.Data?.Items?.ToList();
                Total = (int)searchResult?.Data?.TotalCount;
                StateHasChanged();
            }
        }

        internal void OnItemClick(string Id) => NavigationManager.NavigateTo($"/topic/{Id}");

        protected override bool ShouldRender() => true;

        internal void Ask() => NavigationManager.NavigateTo($"/topic/new");
    }
}