using Blazui.Community.App.Pages;
using Blazui.Community.DTO;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Blazui.Community.App.Components
{
    public class ContentTopBase : PageBase
    {
        protected List<BZTopicDto> Topics = new List<BZTopicDto>();
        protected override async Task InitilizePageDataAsync()
        {
            var result = await NetService.GetTopdTopics();
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
