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
            if(versions!=null&&versions.Any())
                return Ok(versions);
            return new NoContentResponse();
        }
    }
}
