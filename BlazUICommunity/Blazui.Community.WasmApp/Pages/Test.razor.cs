using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Rendering;

namespace Blazui.Community.WasmApp.Pages
{
    public partial class Test1:ComponentBase

    {
        protected override void BuildRenderTree(RenderTreeBuilder builder)
        {
            builder.AddMarkupContent(0, "<span> BuildRenderTree  使用 AddMarkupContent 输出 Html 。</span>");
            // base.BuildRenderTree(builder);
            
        }

    }
}
