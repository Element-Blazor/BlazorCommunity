using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Blazui.Community.App.Model;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace Blazui.Community.App.Data
{
    [ApiController]
    public class NaviController : ControllerBase
    {
        private readonly IOptionsMonitor<List<HeaderMenu>> _options;
        public NaviController(IOptionsMonitor<List<HeaderMenu>> options)
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
            if(!string.IsNullOrWhiteSpace(menu?.URL))
            return Redirect(menu.URL);
            return Redirect("/");
        }
    }
}