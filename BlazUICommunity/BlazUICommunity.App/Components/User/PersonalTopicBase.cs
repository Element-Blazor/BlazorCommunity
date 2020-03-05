using Microsoft.AspNetCore.Authorization;
using System.Threading.Tasks;

namespace Blazui.Community.App.Components
{
    [Authorize]
    public class PersonalTopicBase: PersonalPageBase
    {
    
        protected override void InitTabTitle()
        {
            tabTitle = "我的帖子";
        }
    }
}
