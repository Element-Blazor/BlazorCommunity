﻿using Blazui.Component;
using Microsoft.AspNetCore.Components;

namespace Blazui.Community.WasmApp.Components.Topic
{
    public partial class MainContent : BComponentBase
    {
        [Parameter]
        public string Title { get; set; }

        [Parameter]
        public int TopicType { get; set; }
    }
}