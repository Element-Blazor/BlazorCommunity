using BlazorCommunity.App.Service;
using BlazorCommunity.DTO;
using Element;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.JSInterop;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BlazorCommunity.App.Pages.Mobile
{
    public class TopListBase : PageBase
    {
        protected static List<BZTopicDto> Datas = new List<BZTopicDto>();
        protected int PageSize { get; set; } = 10;
        protected int currentPage = 1;
        public int Total { get; set; } = 0;
        [Inject]
        private IJSRuntime JSRuntime { get; set; }
        protected ElementReference ElementReference;

        [Inject]
        private BrowerService browerService { get; set; }
        private double StartX, StartY, EndX, EndY;
        protected async Task OnTouchStart(TouchEventArgs args)
        {
            StartX = args.Touches[0].ClientX;
            StartY = args.Touches[0].ClientY;
        }
        protected bool IsBottom = false;
        protected async Task OnTouchEnd(TouchEventArgs args)
        {
            EndX = args.ChangedTouches[0].ClientX;
            EndY = args.ChangedTouches[0].ClientY;

            if (StartX == EndX && StartY == EndY)
            {
                Console.WriteLine("点击事件");
                return;//点击事件
            }
            var ScrollResult = await JSRuntime.InvokeAsync<ScrollResult>("isScrollToBottom", ElementReference);
            if (ScrollResult.isScrollToBottom)
            {
             
                await LoadPage(ScrollResult);
            }
            else
            {
               await DropDownRefresh();
            }
        }

        private async Task DropDownRefresh()
        {
            if (StartY < EndY)
            {
                var isScrollToTop = await JSRuntime.InvokeAsync<bool>("isScrollToTop", ElementReference);
                if (isScrollToTop)
                {
                    currentPage = 1;
                    Datas.Clear();
                    await AppendDatas();
                    IsBottom = false;
                }
            }
        }

        private async Task LoadPage(ScrollResult ScrollResult)
        {
            if (Datas.Count < Total)
            {
                currentPage++;
                IsBottom = false;
                await WithFullScreenLoading(async () => await Task.Delay(1000));
                await AppendDatas();
                await JSRuntime.InvokeVoidAsync("scroll", ElementReference, ScrollResult.ScrollHeight - 10);
            }
            else
            {
                IsBottom = true;
            }
        }

        /// <summary>
        /// 当页码变化时触发
        /// </summary>
        public EventCallback<int> CurrentPageChanged { get; set; }
        /// <summary>
        /// 当表格无数据时显示的消息
        /// </summary>

        public string EmptyMessage { get; set; } = "没有查询到数据";

        public string BottomMessage { get; set; } = "我也是有底线的";

        private async Task AppendDatas()
        {
            var Response = await NetService.MobileQuery(currentPage, PageSize);
            if (Response.IsSuccess && Response.Data != null
                && Response.Data.Items != null && Response.Data.Items.Any())
            {
                Datas.AddRange(Response.Data.Items);
                Total = Response.Data.TotalCount;
            }
            StateHasChanged();
        }

        protected override async Task InitilizePageDataAsync()
        {
            if (!browerService.IsMobile())
            {
                NavigationManager.NavigateTo("/",true);
            }
            currentPage = 1;
            Datas.Clear();
            await AppendDatas();
        }
      


    }
    public class ScrollResult
    {
        public bool isScrollToBottom { get; set; }
        public double ScrollHeight { get; set; }
    }
}
