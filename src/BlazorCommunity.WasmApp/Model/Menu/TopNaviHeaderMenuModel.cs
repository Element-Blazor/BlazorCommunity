﻿using System.Collections.Generic;

namespace BlazorCommunity.WasmApp.Model
{
    public class TopNaviHeaderMenuModel
    {
        public string Title { get; set; }
        public string URL { get; set; }
        public string Icon { get; set; }
    }

    public class TopNavMenuOption
    {
        public List<TopNaviHeaderMenuModel> HeaderMenus { get; set; }
    }
}