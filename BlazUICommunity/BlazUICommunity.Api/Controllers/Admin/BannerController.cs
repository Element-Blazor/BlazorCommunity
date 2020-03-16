using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Arch.EntityFrameworkCore.UnitOfWork.Collections;
using AutoMapper;
using Blazui.Community.Api.Extensions;
using Blazui.Community.Api.Service;
using Blazui.Community.DTO;
using Blazui.Community.Model.Models;
using Blazui.Community.Repository;
using Blazui.Community.Request;
using Blazui.Community.Utility;
using Blazui.Community.Utility.Filter;
using Blazui.Community.Utility.Response;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace Blazui.Community.Api.Controllers
{
    /// <summary>
    /// 
    /// </summary>
    [Route("api/[controller]")]
    [HiddenApi]
    [ApiController]
    [SwaggerTag(description: "banner")]
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



        /// <summary>
        /// 逻辑删除
        /// </summary>
        /// <returns></returns>
        [HttpDelete("Delete/{Id}")]
        public IActionResult Delete([FromRoute] string Id)
        {
            //_BZBannerRepository.LogicDelete(Id, oprationId);
            //return Ok();

            var version = _BZBannerRepository.Find(Id);
            if (version != null && !string.IsNullOrWhiteSpace(version.Id))
            {
                version.Status = version.Status == -1 ? 0 : -1;
            }
            _BZBannerRepository.Update(version);
            _cacheService.Remove(nameof(BzBannerModel));
            return Ok();
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
        /// 根据Id查询
        /// </summary>
        /// <returns></returns>
        [HttpGet("Query/{Id}")]
        public async Task<IActionResult> Query([FromRoute] string Id)
        {
            var res = await _BZBannerRepository.FindAsync(Id);
            if (res is null)
                return NoContent();
            return Ok(res);
        }
        /// <summary>
        /// 根据条件分页查询
        /// </summary>
        /// <returns></returns>
        [HttpPost("Query")]
        public async Task<IActionResult> Query([FromBody] BannerRequestCondition Request = null)
        {
            IPagedList<BzBannerModel> pagedList = null;
            var query = Request.CreateQueryExpression<BzBannerModel, BannerRequestCondition>();
            pagedList = await _BZBannerRepository.GetPagedListAsync(query, o => o.OrderBy(p => p.Id), null, Request.PageIndex - 1, Request.PageSize);
            var pagedatas = pagedList.ConvertToPageData<BzBannerModel, BzBannerDto>();
            pagedatas.Items = _mapper.Map<IList<BzBannerDto>>(pagedList.Items);
            return Ok(pagedatas);
        }

        /// <summary>
        /// 根据条件分页查询
        /// </summary>
        /// <returns></returns>
        [HttpGet("GetPageList/{pageSize}/{pageIndex}")]
        public async Task<IActionResult> Query(int pageSize, int pageIndex)
        {

            var pagedList = await _BZBannerRepository.GetPagedListAsync(p =>true, o => o.OrderBy(p => p.Id), null, pageIndex - 1, pageSize);
            var pagedatas = pagedList.ConvertToPageData<BzBannerModel, BzBannerDto>();
            pagedatas.Items = _mapper.Map<IList<BzBannerDto>>(pagedList.Items);
            return Ok(pagedatas);
        }
        /// <summary>
        /// 根据条件分页查询
        /// </summary>
        /// <returns></returns>
        [HttpGet("QueryAll")]
        public async Task<IActionResult> QueryAll()
        {
            var banners = await _cacheService.Banners(p => p.Show && p.Status == 0);
            return Ok(banners);
        }
    }
}