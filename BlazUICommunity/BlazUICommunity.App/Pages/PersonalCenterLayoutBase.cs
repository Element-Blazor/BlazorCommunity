using Blazui.Community.App.Model;
using Blazui.Component.Container;
using Blazui.Component.NavMenu;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Blazui.Community.App.Pages
{
    [Authorize]
    public class PersonalCenterLayoutBase : LayoutComponentBase
    {
        protected List<PersonalCenterMenuModel> personalCenterMenuModels;
        protected BLayout bLayout
        {
            get; set;
        }
        protected BTab bTab
        {
            get; set;
        }
        protected BMenu bmenu
        {
            get; set;
        }
        protected BTabPanel bTabPanel { get; set; }
        protected BCard bCard { get; set; }
        protected override async Task OnInitializedAsync()
        {
            await base.OnInitializedAsync();
            InitTabs();
        }
        private void InitTabs()
        {
            personalCenterMenuModels ??= new List<PersonalCenterMenuModel>()
            {
                new PersonalCenterMenuModel()
                {
                    Icon    = "el-icon-user",
                    Title   = "基本信息",
                    Index   = 0,
                    Route="user/base"
                },
                new PersonalCenterMenuModel()
                {
                    Icon    = "el-icon-camera",
                    Title   = "我的头像",
                    Index   = 1,
                       Route="user/avator"
                },
                new PersonalCenterMenuModel()
                {
                    Icon    = "el-icon-lock",
                    Title   = "我的密码",
                    Index   = 2,
                       Route="user/pwd"
                },
                //      new PersonalCenterMenuModel()
                //{
                //    Icon    = "el-icon-mobile",
                //    Title   = "我的绑定",
                //    Index   = 3,
                //       Route="user/bind"
                //},
                new PersonalCenterMenuModel()
                {
                    Icon    = "el-icon-star-on",
                    Title   = "我的帖子",
                    Index   = 4,
                        Route="user/topic"
                }
            };
        }

    }
}
