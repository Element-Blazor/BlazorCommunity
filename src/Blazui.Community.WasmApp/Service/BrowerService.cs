using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Primitives;
using Microsoft.JSInterop;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Blazui.Community.WasmApp.Service
{
    public class BrowerService
    {
        private readonly IJSRuntime jSRuntime;
        public BrowerService(IJSRuntime jSRuntime)
        {
            this.jSRuntime = jSRuntime;
        }
        public async Task<bool> IsMobile()
        {
            var ismobile = await jSRuntime.InvokeAsync<bool>("isMobile");
            Console.WriteLine($"ismobile -{ismobile}");
            return ismobile;
        }
    }
}
