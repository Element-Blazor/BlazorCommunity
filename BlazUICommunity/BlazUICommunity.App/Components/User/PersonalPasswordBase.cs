using Blazui.Community.App.Model;
using Blazui.Community.Model.Models;
using Blazui.Component;
using Blazui.Component.Container;
using Microsoft.AspNetCore.Authorization;
using Newtonsoft.Json;
using System.Threading.Tasks;
using static Blazui.Community.App.ConstantConfiguration;

namespace Blazui.Community.App.Components
{
    [Authorize]
    public class PersonalPasswordBase : PersonalPageBase
    {

        protected override bool ShouldRender() => true;

        protected override void InitTabTitle() => tabTitle = "修改密码";
     
    }
}
