using BlazorCommunity.Api.Service;
using BlazorCommunity.SwaggerExtensions;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace BlazorCommunity.Api.Controllers
{
    [HiddenApi]
    [Route("[controller]")]
    [ApiController]
    //[SwaggerTag(description: "测试")]
    //[HttpCacheExpiration(CacheLocation = CacheLocation.Public)]
    //[HttpCacheValidation(MustRevalidate = true)]
    public class TestController : ControllerBase
    {
        private readonly ISmtpClientService smtpClientService;

        public TestController(ISmtpClientService smtpClientService)
        {
            this.smtpClientService = smtpClientService;
        }
        [HttpGet("SendATestEmail")]
        public async Task<IActionResult> SendATestEmail(string email)
        {
            if (!string.IsNullOrWhiteSpace(email))
            {
                return Ok(await smtpClientService.SendAsync(email, "这是一封测试邮件", "邮件测试"));
            }
            return BadRequest();
        }
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