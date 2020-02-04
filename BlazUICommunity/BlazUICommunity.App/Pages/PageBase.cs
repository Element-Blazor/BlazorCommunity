﻿using AutoMapper;
using Blazui.Community.App.Service;
using Blazui.Community.App.Shared;
using Blazui.Community.DTO;
using Blazui.Community.Model.Models;
using Blazui.Component;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Caching.Memory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Blazui.Community.App.Pages
{
    public abstract class PageBase : BComponentBase
    {
        [Inject]
        public IMemoryCache memoryCache { get; set; }
        [Inject]
        public ProductService ProductService { get; set; }

        [Inject]
        public MessageService MessageService { get; set; }
        [Inject]
        public MessageBox MessageBox { get; set; }
        public  BZUserModel BzUser { get; set; }

        [Inject]
        public IMapper mapper { get; set; }

        [CascadingParameter]
        public Task<AuthenticationState> authenticationStateTask { get; set; }
        [Inject]
        public UserManager<BZUserModel> userManager { get; set; }
        [Inject]
        public NavigationManager navigationManager { get; set; }

        MainLayoutBase mainLayoutBase;
        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            await base.OnAfterRenderAsync(firstRender);
            if ( !firstRender )
            {
                return;
            }
            try
            {
                LoadingService.Show(new LoadingOption()
                {
                    Background = "rgba(0, 0, 0, 0.1)" ,
                    Text = "拼命加载中" ,
                    IconClass = "el-icon-loading"
                });
                await InitilizePageDataAsync();
                LoadingService.CloseFullScreenLoading();
            }
            catch ( Exception  ex)
            {
                Console.WriteLine(ex.Message);
            }
          
            RequireRender = true;
            StateHasChanged();
            //LoadingService.CloseFullScreenLoading();
        }
     
        protected abstract Task InitilizePageDataAsync();
    }
}
