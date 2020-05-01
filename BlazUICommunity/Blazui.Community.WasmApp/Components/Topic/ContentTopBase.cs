using Blazui.Community.WasmApp.Pages;
using Blazui.Community.DTO;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Blazui.Community.WasmApp.Components
{
    public class ContentTopBase : PageBase
    {
        protected List<BZTopicDto> Topics = new List<BZTopicDto>();

        protected override async Task InitilizePageDataAsync()
        {
            var result = await NetService.QueryTopdTopics();
            if (result.IsSuccess)
            {
                if (result.Data.Any())
                {
                    Topics = result.Data;
                }
            }
        }
    }
}