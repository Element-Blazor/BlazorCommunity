﻿using Blazui.Community.DTO;
using Blazui.Component;
using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Blazui.Community.App.Components.Topic
{
    public class NewReplyItemBase: BComponentBase
    {


        [CascadingParameter]
        public NewReplyList ReplyList { get; set; }

        [Parameter]
        public virtual RenderFragment<object> ChildContent { get; set; }

        [Parameter]
        public BZReplyDto reply { get; set; }


        protected override void OnInitialized()
        {
            base.OnInitialized();
        }
    }
}