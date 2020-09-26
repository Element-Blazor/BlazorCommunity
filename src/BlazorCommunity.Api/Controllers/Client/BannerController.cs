using BlazorCommunity.Api.Service;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System.Threading.Tasks;

namespace BlazorCommunity.Api.Controllers.Client
{
    /// <summary>
    ///
    /// </summary>

    [Route("api/client/[controller]")]
    [ApiController]
    [SwaggerTag(description: "banner")]
    public class BannerController : ControllerBase
    {
        /// <summary>
        /// 根据条件分页查询
        /// </summary>
        /// <returns></returns>
        [HttpGet("QueryAll")]
        public async Task<IActionResult> QueryAll([FromServices] ICacheService cacheService)
        {
            return Ok(await cacheService.GetBannersAsync(p => p.Show && p.Status == 0));
        }
    }
}