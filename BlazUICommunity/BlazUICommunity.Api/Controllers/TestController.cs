using System;
using Arch.EntityFrameworkCore.UnitOfWork;
using Blazui.Community.Repository;
using Blazui.Community.Utility.Filter;
using Marvin.Cache.Headers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Blazui.Community.Api.Controllers
{
    [HiddenApi]
    [Route("[controller]")]
    [ApiController]
    //[SwaggerTag(description: "测试")]
    [HttpCacheExpiration(CacheLocation = CacheLocation.Public)]
    [HttpCacheValidation(MustRevalidate = true)]
    public class TestController : ControllerBase
    {
       
        [HttpGet("Time")]
        [ResponseCache(Duration = 100)]
        public IActionResult Time()
        {
            return Ok(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
        }

    }
}