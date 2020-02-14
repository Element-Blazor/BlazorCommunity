using Blazui.Community.Model.Models;
using Blazui.Community.Utility;
using Blazui.Component;
using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Blazui.Community.App.Components
{
    public class ReplyItemBase:BComponentBase
    {
        [Parameter]
        public BZReplyDtoWithUser ReplyModel { get; set; }

        //protected override async Task OnInitializedAsync()
        //{
        //    await base.OnInitializedAsync();
        //    ReplyModel = new BZReplyDtoWithUser() {

        //        Content = "测试一下",
        //        NickName = "Blazor",
        //        LastModifyTime = DateTime.Now.AddDays(new Random().Next(0, 10)).ConvertToDateDiffStr()
        //    };

        //}
    }
}
