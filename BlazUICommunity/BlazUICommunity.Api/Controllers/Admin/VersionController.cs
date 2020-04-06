using Arch.EntityFrameworkCore.UnitOfWork;
using Arch.EntityFrameworkCore.UnitOfWork.Collections;
using AutoMapper;
using Blazui.Community.Api.Service;
using Blazui.Community.DTO;
using Blazui.Community.DTO.Admin;
using Blazui.Community.LinqExtensions;
using Blazui.Community.Model.Models;
using Blazui.Community.Request;
using Blazui.Community.SwaggerExtensions;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Blazui.Community.Api.Controllers
{   /// <summary>
    ///
    /// </summary>
    [HiddenApi]
    [Route("api/[Controller]")]
    [ApiController]
    [SwaggerTag(description: "版本信息")]
    //[HttpCacheExpiration(MaxAge = 200)]
    public class VersionController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ICacheService _cacheService;

        /// <summary>
        ///
        /// </summary>
        /// <param name="unitOfWork"></param>
        /// <param name="mapper"></param>
        /// <param name="cacheService"></param>
        public VersionController(IUnitOfWork unitOfWork, IMapper mapper, ICacheService cacheService)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _cacheService = cacheService;
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

        private async Task<IActionResult> DeleteOrResume(string Id, int Status)
        {
            var verRepository = _unitOfWork.GetRepository<BZVersionModel>();
            var version = await verRepository.FindAsync(Id);
            if (version is null)
                return BadRequest();
            if (version.Status == Status)
                return Ok();
            version.Status = Status;
            version.LastModifyDate = DateTime.Now;
            version.LastModifierId = Guid.Empty.ToString();
            verRepository.Update(version);
            _cacheService.Remove(nameof(BZVersionModel));
            return Ok();
        }

        /// <summary>
        /// 删除
        /// </summary>
        /// <returns></returns>
        [HttpPost("Add")]
        public async Task<IActionResult> Add(BZVersionDto dto)
        {
            var verRepository = _unitOfWork.GetRepository<BZVersionModel>();
            BZVersionModel model = _mapper.Map<BZVersionModel>(dto);
            model.CreateDate = DateTime.Now;
            model.CreatorId = Guid.Empty.ToString();
            model.LastModifyDate = DateTime.Now;
            model.LastModifierId = Guid.Empty.ToString();
            await verRepository.InsertAsync(model);
            _cacheService.Remove(nameof(BZVersionModel));
            return Ok();
        }

        /// <summary>
        /// 更新
        /// </summary>
        /// <returns></returns>
        [HttpPut("Update")]
        public IActionResult Update(BZVersionDto dto)
        {
            var verRepository = _unitOfWork.GetRepository<BZVersionModel>();
            BZVersionModel model = _mapper.Map<BZVersionModel>(dto);
            model.LastModifyDate = DateTime.Now;
            model.LastModifierId = Guid.Empty.ToString();
            verRepository.Update(model);
            _cacheService.Remove(nameof(BZVersionModel));
            return Ok();
        }

        /// <summary>
        /// 获取版本数据
        /// </summary>
        /// <returns></returns>
        [HttpGet("Query")]
        public async Task<ActionResult<VersionDisplayDto>> GetPageList([FromQuery] VersionRequestCondition requestCondition)
        {
            var verRepository = _unitOfWork.GetRepository<BZVersionModel>();
            IPagedList<BZVersionModel> versions;
            Expression<Func<BZVersionModel, bool>> query = p => true;
            if (requestCondition.ProjectId >= 0)
                query = query.And(p => p.Project == requestCondition.ProjectId);
            versions = await verRepository.GetPagedListAsync(query, o => o.OrderByDescending(p => p.CreateDate), null, requestCondition.PageIndex - 1, requestCondition.PageSize);
            if (versions.Items.Any())
                return Ok(versions.From(result => _mapper.Map<IList<VersionDisplayDto>>(result)));
            return NoContent();
        }
    }
}