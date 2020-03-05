using Arch.EntityFrameworkCore.UnitOfWork;
using Arch.EntityFrameworkCore.UnitOfWork.Collections;
using AutoMapper;
using Blazui.Community.Api.Extensions;
using Blazui.Community.Api.Service;
using Blazui.Community.DTO;
using Blazui.Community.Model.Models;
using Blazui.Community.Utility.Extensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Blazui.Community.Api.Controllers.Client
{   /// <summary>
    /// 
    /// </summary>
    [Route("api/client/[controller]")]
    [ApiController]
    [SwaggerTag(description: "版本信息")]
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
        [Authorize]
        [HttpGet("Delete/{Id}")]
        public IActionResult Delete(string Id)
        {
            var verRepository = _unitOfWork.GetRepository<BZVersionModel>();
            var version = verRepository.Find(Id);
            if (version != null && !string.IsNullOrWhiteSpace(version.Id))
            {
                version.Status = version.Status == -1 ? 0 : -1;
            }
            verRepository.Update(version);
            _cacheService.Remove(nameof(BZVersionModel));
            return Ok();

        }

        /// <summary>
        /// 删除
        /// </summary>
        /// <returns></returns>
        [Authorize]
        [HttpPost("Add")]
        public async Task<IActionResult> Add(BZVersionDto dto)
        {
            var verRepository = _unitOfWork.GetRepository<BZVersionModel>();
            BZVersionModel model = _mapper.Map<BZVersionModel>(dto);
            await verRepository.InsertAsync(model);
            _cacheService.Remove(nameof(BZVersionModel));
            return Ok();

        }


        /// <summary>
        /// 更新
        /// </summary>
        /// <returns></returns>
        [Authorize]
        [HttpPost("Update")]
        public IActionResult Update(BZVersionDto dto)
        {
            var verRepository = _unitOfWork.GetRepository<BZVersionModel>();
            BZVersionModel model = _mapper.Map<BZVersionModel>(dto);
            verRepository.Update(model);
            _cacheService.Remove(nameof(BZVersionModel));
            return Ok();

        }
        /// <summary>
        /// 获取版本数据
        /// </summary>
        /// <returns></returns>
        [HttpGet("Query/{Project}")]
        public async Task<IActionResult> Get([SwaggerParameter(Required = false)]int Project = -1)
        {
            Expression<Func<BZVersionModel, bool>> expression = p => p.Status == 0;
            if (Project != -1)
                expression = expression.And(p => p.Project == Project);
            var verRepository = _unitOfWork.GetRepository<BZVersionModel>();
            var versions = await verRepository.GetAllAsync(expression);

            return Ok(versions);
        }

        /// <summary>
        /// 获取版本数据
        /// </summary>
        /// <returns></returns>
        [HttpGet("GetAll")]
        public async Task<IActionResult> GetAll()
        {
            Expression<Func<BZVersionModel, bool>> expression = p => p.Status == 0;
            var verRepository = _unitOfWork.GetRepository<BZVersionModel>();
            var versions = await verRepository.GetAllAsync(expression);
            return Ok(versions);
        }

        /// <summary>
        /// 获取版本数据
        /// </summary>
        /// <returns></returns>
        [HttpGet("GetPageList/{projectId}/{pageSize}/{pageIndex}")]
        public async Task<IActionResult> GetPageList(int projectId, int pageSize, int pageIndex)
        {
            var verRepository = _unitOfWork.GetRepository<BZVersionModel>();
            IPagedList<BZVersionModel> versions;
            if (projectId < 0)
            {
                versions = await verRepository.GetPagedListAsync(pageIndex - 1, pageSize);

            }
            else
            {
                versions = await verRepository.GetPagedListAsync(p => p.Project == projectId, orderBy: o => o.OrderByDescending(p => p.CreateDate), null, pageIndex - 1, pageSize);

            }
            var pagedatas = versions.ConvertToPageData<BZVersionModel, BZVersionDto>();
            pagedatas.Items = _mapper.Map<IList<BZVersionDto>>(versions.Items);
            return Ok(pagedatas);
        }
    }
}
