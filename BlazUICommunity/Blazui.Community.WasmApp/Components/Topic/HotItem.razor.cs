﻿using Blazui.Community.DTO;
using Blazui.Component;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Blazui.Community.WasmApp.Components.Topic
{
    public partial class HotItem:BComponentBase
    {
        [Parameter]
        public HotTopicDto Hot { get; set; }

        [Parameter]
        public EventCallback<MouseEventArgs> OnClick { get; set; }
       

    }
}