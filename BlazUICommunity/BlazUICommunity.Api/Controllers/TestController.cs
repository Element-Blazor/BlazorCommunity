using Blazui.Community.SwaggerExtensions;
using Microsoft.AspNetCore.Mvc;
using System;

namespace Blazui.Community.Api.Controllers
{
    [HiddenApi]
    [Route("[controller]")]
    [ApiController]
    //[SwaggerTag(description: "测试")]
    //[HttpCacheExpiration(CacheLocation = CacheLocation.Public)]
    //[HttpCacheValidation(MustRevalidate = true)]
    public class TestController : ControllerBase
    {
        [HttpGet("Time")]
        public IActionResult Time(string a)
        {
            return Ok(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
        }

        [HttpPatch("Time")]
        public IActionResult Time2(string a)
        {
            return Ok(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
        }

        [HttpOptions("Time")]
        public IActionResult Time3(string a)
        {
            return Ok(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
        }

        [HttpHead("Time")]
        public IActionResult Time4(string a)
        {
            return Ok(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
        }

        [HttpPut("Time")]
        public IActionResult Time5(string a)
        {
            return Ok(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
        }

        [HttpDelete("Time")]
        public IActionResult Time6(string a)
        {
            return Ok(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
        }

        [HttpPost("Time")]
        public IActionResult Time7(string a)
        {
            return Ok(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
        }
    }
}