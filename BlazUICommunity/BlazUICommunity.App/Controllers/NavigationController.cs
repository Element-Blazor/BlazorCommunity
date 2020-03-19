using Blazui.Community.App.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System.Collections.Generic;
using System.Linq;

namespace Blazui.Community.App.Controllers
{
    [ApiController]
    public class NavigationController : ControllerBase
    {
        private readonly IOptionsMonitor<List<TopNaviHeaderMenuModel>> _options;

        public NavigationController(IOptionsMonitor<List<TopNaviHeaderMenuModel>> options)
        {
            _options = options;
        }

        /// <summary>
        /// 跳转
        /// </summary>
        /// <returns></returns>
        [HttpGet("navto/{Route}")]
        public IActionResult NavTo(string Route)
        {
            var menu = _options.CurrentValue.FirstOrDefault(p => p.Route.TrimStart('/') == Route);
            if (!string.IsNullOrWhiteSpace(menu?.URL))
                return Redirect(menu.URL);
            return Redirect("/");
        }
    }
}