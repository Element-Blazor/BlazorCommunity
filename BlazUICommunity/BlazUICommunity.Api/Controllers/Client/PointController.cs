using System;
using System.Linq;
using System.Threading.Tasks;
using Arch.EntityFrameworkCore.UnitOfWork;
using Arch.EntityFrameworkCore.UnitOfWork.Collections;
using AutoMapper;
using Blazui.Community.DTO;
using Blazui.Community.Model.Models;
using Blazui.Community.Request;
using Blazui.Community.Utility.Extensions;
using Blazui.Community.Utility.Response;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace Blazui.Community.Api.Controllers.Client
{
    /// <summary>
    /// 积分
    /// </summary>
    [Route("api/client/[controller]")]
    [ApiController]
    [SwaggerTag(description: "积分相关")]
    public class PointController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IRepository<BZPointModel> _pointRepository;
        private readonly IMapper _mapper;
        /// <summary>
        /// 
        /// </summary>
        /// <param name="unitOfWork"></param>
        /// <param name="mapper"></param>
        public PointController(IUnitOfWork unitOfWork ,
            IMapper mapper)
        {
            _mapper = mapper;
            _unitOfWork = unitOfWork;
            _pointRepository = unitOfWork.GetRepository<BZPointModel>(true);
        }


        /// <summary>
        /// 新增
        /// </summary>
        /// <returns></returns>
        [Authorize]
        [HttpPost("Add")]
        public async Task<IActionResult> Add([FromBody] BZPointDto dto)
        {
            var pointModel = _mapper.Map<BZPointModel>(dto);
             await _pointRepository.InsertAsync(pointModel);
            return Ok();
        }



        /// <summary>
        /// 逻辑删除
        /// </summary>
        /// <returns></returns>
        [Authorize]
        [HttpDelete("Delete/{Id}/{oprationId}")]
        public IActionResult Delete([FromRoute] string Id,string oprationId)
        {
            _pointRepository.LogicDelete(Id, oprationId);
            return Ok();
        }

        /// <summary>
        /// 更新
        /// </summary>
        /// <returns></returns>
        [Authorize]
        [HttpPost("Update/{Id}")]
        public IActionResult Update([FromBody] BZPointDto Dto)
        {
            if (!Guid.TryParse(Dto.Id,out _))
                return new BadRequestResponse("id is error");
            var point = _mapper.Map<BZPointModel>(Dto);
            _pointRepository.Update(point);
            return Ok();
        }

        /// <summary>
        /// 根据Id查询
        /// </summary>
        /// <returns></returns>
        [HttpGet("Query/{Id}")]
        public async Task<IActionResult> Query([FromRoute] string Id)
        {
            var res = await _pointRepository.FindAsync(Id);
            if ( res is null )
                    return NoContent( );
            return Ok(res);
        }
        /// <summary>
        /// 根据条件分页查询
        /// </summary>
        /// <returns></returns>
        [HttpPost("Query")]
        public async Task<IActionResult> Query([FromBody] PointRequestCondition Request = null)
        {
            IPagedList<BZPointModel> pagedList = null;
            var query = Request.CreateQueryExpression<BZPointModel , PointRequestCondition>();
            pagedList = query == null ? await _pointRepository.GetPagedListAsync(Request.PageInfo.PageIndex - 1 , Request.PageInfo.PageSize) :
                       await _pointRepository.GetPagedListAsync(query , o => o.OrderBy(p => p.Id) , null , Request.PageInfo.PageIndex - 1 , Request.PageInfo.PageSize);
            return Ok(pagedList);
        }
    }
}