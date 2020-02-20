using Arch.EntityFrameworkCore.UnitOfWork;
using AutoMapper;
using Blazui.Community.Model.Models;
using Blazui.Community.Utility.Extensions;
using Blazui.Community.Utility.Response;
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
    [Route("api/[Controller]")]
    [ApiController]
    [SwaggerTag(description: "版本信息")]
    public class VersionController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="unitOfWork"></param>
        /// <param name="mapper"></param>
        public VersionController(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }
        /// <summary>
        /// 删除
        /// </summary>
        /// <returns></returns>
        [HttpGet("Delete/{Id}")]
        public  IActionResult Delete(int Id)
        {
            var verRepository = _unitOfWork.GetRepository<BZVersionModel>();
            var version = verRepository.Find(Id);
            if (version != null && version.Id > 0)
            {
                version.Status = version.Status == -1 ? 0 : -1;
            }
            verRepository.Update(version);
            return Ok();

        }
        /// <summary>
        /// 获取版本数据
        /// </summary>
        /// <returns></returns>
        [HttpGet("Query/{Project}")]
        public async Task<IActionResult> Get([SwaggerParameter(Required = false)]int Project = -1)
        {
            Expression<Func<BZVersionModel, bool>> expression = p => p.Status==0;
            if(Project!=-1)
                expression = expression.And(p => p.Project == Project);
            var verRepository = _unitOfWork.GetRepository<BZVersionModel>();
            var versions =await verRepository.GetAllAsync(expression);
 
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
        public async Task<IActionResult> GetPageList(int projectId,int pageSize,int pageIndex)
        {
            var verRepository = _unitOfWork.GetRepository<BZVersionModel>();
            if (projectId<0)
            {
                var versions = await verRepository.GetPagedListAsync(pageIndex-1, pageSize);
                return Ok(versions);
            }
            else
            {
                var versions = await verRepository.GetPagedListAsync(p => p.Project == projectId, orderBy: o => o.OrderByDescending(p => p.VerDate), null, pageIndex-1, pageSize) ;
                return Ok(versions);
            }
        }
    }
}
