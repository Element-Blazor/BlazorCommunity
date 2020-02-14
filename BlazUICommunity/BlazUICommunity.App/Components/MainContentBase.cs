using Blazui.Component;
using Blazui.Component.Container;
using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Blazui.Community.App.Components
{
    public class MainContentBase:BComponentBase
    {
        protected BLayout contentlayout;
        protected BLayout blayout_north;
        protected BCard bcard;
        protected ContentCenter contentCenter;
        [Parameter]
        public string Title { get; set; }

        private int topictype;
        [Parameter]
        public int TopicType { get; set; }
        //{
        //    get
        //    {
        //        return topictype;
        //    }
        //    set
        //    {
        //        topictype = value;
        //        RequireRender = true;
        //        contentlayout?.Refresh();
        //        blayout_north?.Refresh();
        //        bcard?.Refresh();
        //        contentCenter?.Refresh();
        //        MarkAsRequireRender();
        //        StateHasChanged();
        //        Console.WriteLine(topictype);
        //        Task.CompletedTask.Wait();
        //    }
        //}


    }
}
