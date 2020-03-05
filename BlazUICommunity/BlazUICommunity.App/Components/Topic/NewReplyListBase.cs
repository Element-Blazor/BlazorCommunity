using Blazui.Community.DTO;
using Blazui.Component;
using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Blazui.Community.App.Components.Topic
{
    public class NewReplyListBase : BComponentBase, IContainerComponent
    {

        /// <summary>
        /// 数据源
        /// </summary>
        [Parameter]
        public List<BZReplyDto> DataSource { get; set; } = new List<BZReplyDto>();



        /// <summary>
        /// 当表格无数据时显示的消息
        /// </summary>
        [Parameter]
        public string EmptyMessage { get; set; }
        /// <summary>
        /// 总数据条数
        /// </summary>
        [Parameter]
        public int Total { get; set; }

        /// <summary>
        /// 每页条数
        /// </summary>
        [Parameter]
        public int PageSize { get; set; } = 20;

        /// <summary>
        /// 当前页数
        /// </summary>
        [Parameter]
        public int CurrentPage
        {
            get
            {
                return currentPage;
            }
            set
            {
                currentPage = value;
            }
        }

        private int currentPage = 1;

        /// <summary>
        /// 最大显示的页码数
        /// </summary>
        [Parameter]
        public int ShowPageCount { get; set; } = 7;

        /// <summary>
        /// 当前最大显示的页码数变化时触发
        /// </summary>
        [Parameter]
        public EventCallback<int> ShowPageCountChanged { get; set; }

        /// <summary>
        /// 当页码变化时触发
        /// </summary>
        [Parameter]
        public EventCallback<int> CurrentPageChanged { get; set; }


        /// <summary>
        /// 当只有一页时，不显示分页
        /// </summary>
        [Parameter]
        public bool NoPaginationOnSinglePage { get; set; } = true;

  

        [Parameter]
        public RenderFragment ChildContent { get; set; }

        /// <summary>
        /// 启用分页
        /// </summary>
        [Parameter]
        public bool EnablePagination { get; set; } = true;

       

  

        /// <summary>
        /// 加载中状态背景颜色
        /// </summary>
        [Parameter]
        public string LoadingBackground { get; set; }

        /// <summary>
        /// 加载中状态样式类
        /// </summary>
        [Parameter]
        public string LoadingIconClass { get; set; }

        /// <summary>
        /// 加载中状态文字
        /// </summary>
        public string LoadingText { get; set; }

     
        public ElementReference Container { get; set; }

    

        protected override void OnInitialized()
        {
            base.OnInitialized();
         
            if (DataSource == null)
            {
                return;
            }
        }

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            RequireRender = true;
            StateHasChanged();
        }

        public override async Task SetParametersAsync(ParameterView parameters)
        {
            await base.SetParametersAsync(parameters);
        }

        public void AddChild(NewReplyItem newReplyItem)
        {
        }
    }
}
