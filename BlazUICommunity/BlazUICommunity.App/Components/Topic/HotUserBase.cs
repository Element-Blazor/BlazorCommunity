using Blazui.Community.App.Service;
using Blazui.Community.DTO;
using Blazui.Component;
using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Blazui.Community.App.Components.Topic
{
    public class HotUserBase : BComponentBase
    {

        protected string Title { get; set; }
      
        protected  List<HotUserDto> Datas { get; set; } = new List<HotUserDto>();
        [Inject]
         NetworkService Service { get; set; }
        public static string[] TagClasses =   { "","el-tag--success", "el-tag--info", "el-tag--warning", "el-tag--danger" };
        protected override bool ShouldRender() => true;

        protected async override Task OnAfterRenderAsync(bool firstRender)
        {
            await base.OnAfterRenderAsync(firstRender);
            if (firstRender)
            {
                Title = "活跃用户";
                var UserResult = await Service.QueryHotUsers();
                Datas = UserResult.IsSuccess? UserResult.Data:new List<HotUserDto>();
                MarkAsRequireRender();
                StateHasChanged();
            }
        }
    }
}
