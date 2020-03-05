using Microsoft.AspNetCore.Authorization;
using System.Threading.Tasks;

namespace Blazui.Community.App.Components
{
    [Authorize]
    public class PersonalBindBase : PersonalPageBase
    {
     
        protected override void InitTabTitle()
        {
            tabTitle = "我的绑定";
        }
    }
}
