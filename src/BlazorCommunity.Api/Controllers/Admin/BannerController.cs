using Arch.EntityFrameworkCore.UnitOfWork.Collections;
using AutoMapper;
using BlazorCommunity.Api.Service;
using BlazorCommunity.DTO;
using BlazorCommunity.DTO.Admin;
using BlazorCommunity.Model.Models;
using BlazorCommunity.Repository;
using BlazorCommunity.Request;
using BlazorCommunity.Response;
using BlazorCommunity.SwaggerExtensions;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BlazorCommunity.Api.Controllers
{
    /// <summary>
    ///
    /// </summary>
    [Route("api/[controller]")]
    [HiddenApi]
    [ApiController]
    [SwaggerTag(description: "banner")]
    //[HttpCacheExpiration(MaxAge = 100)]
    public class BannerController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly BZBannerRepository _BZBannerRepository;
        private readonly ICacheService _cacheService;

        /// <summary>
        ///
        /// </summary>
        /// <param name="bZBannerRepository"></param>
        public BannerController(BZBannerRepository bZBannerRepository, IMapper mapper, ICacheService cacheService)
        {
            _BZBannerRepository = bZBannerRepository;
            _mapper = mapper;
            _cacheService = cacheService;
        }

        /// <summary>
        /// 新增
        /// </summary>
        /// <returns></returns>
        [HttpPost("Add")]
        public async Task<IActionResult> Add([FromBody] BzBannerDto dto)
        {
            var banner = _mapper.Map<BzBannerModel>(dto);
            await _BZBannerRepository.InsertAsync(banner);
            _cacheService.Remove(nameof(BzBannerModel));
            return Ok();
        }

        private async Task<IActionResult> DeleteOrResume(string Id, int Status)
        {
            var banner = await _BZBannerRepository.FindAsync(Id);
            if (banner is null)
                return BadRequest();
            if (banner.Status == Status)
                return Ok();
            banner.Status = Status;
            _BZBannerRepository.Update(banner);
            _cacheService.Remove(nameof(BzBannerModel));
            return Ok();
        }

        /// <summary>
        /// 删除
        /// </summary>
        /// <returns></returns>
        [HttpPatch("Delete/{Id}")]
        public async Task<IActionResult> Delete([FromRoute] string Id)
        {
            return await DeleteOrResume(Id, -1);
        }

        /// <summary>
        /// 恢复
        /// </summary>
        /// <returns></returns>
        [HttpPatch("Resume/{Id}")]
        public async Task<IActionResult> Resume([FromRoute] string Id)
        {
            return await DeleteOrResume(Id, 0);
        }

        /// <summary>
        /// 更新
        /// </summary>
        /// <returns></returns>
        [HttpPost("Update")]
        public IActionResult Update([FromBody] BzBannerDto Dto)
        {
            if (!Guid.TryParse(Dto.Id, out _))
                return new BadRequestResponse("id is error");
            var banner = _mapper.Map<BzBannerModel>(Dto);
            _BZBannerRepository.Update(banner);
            _cacheService.Remove(nameof(BzBannerModel));
            return Ok();
        }

        /// <summary>
        /// 根据条件分页查询
        /// </summary>
        /// <returns></returns>
        [HttpGet("Query")]
        public async Task<ActionResult<IPagedList<BannerDisplayDto>>> Query([FromQuery] BannerReuestCondition reuestCondition)
        {
            var pagedList = await _BZBannerRepository.GetPagedListAsync(p => true, o => o.OrderBy(p => p.Id), null, reuestCondition.PageIndex - 1, reuestCondition.PageSize);
            var pagedatas = pagedList.From(result => _mapper.Map<IList<BannerDisplayDto>>(result));
            return Ok(pagedatas);
        }
    }
}