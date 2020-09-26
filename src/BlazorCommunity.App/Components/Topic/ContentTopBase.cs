using BlazorCommunity.App.Pages;
using BlazorCommunity.DTO;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BlazorCommunity.App.Components
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