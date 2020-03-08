using Blazui.Community.App.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static Blazui.Community.App.ConstantConfiguration;
namespace Blazui.Community.App.Configuration
{
    public class HeaderMenuConfiguration
    {
        public HeaderMenuConfiguration( )
        {

        }
        private static List<HeaderMenu> headerMenus = new List<HeaderMenu>();
        public static List<HeaderMenu> Headers()
        {
            headerMenus.Add(new HeaderMenu() { Title = "交流", Route = "/", URL = "" });
            headerMenus.Add(new HeaderMenu() { Title = "组件", Route = "/Component", URL = ComponentUrl });
            headerMenus.Add(new HeaderMenu() { Title = "文档", Route = "/Document", URL = DocsUrl });
            headerMenus.Add(new HeaderMenu() { Title = "示例", Route = "/Demo", URL = DemoUrl });
            return headerMenus;
        }
    }
}
