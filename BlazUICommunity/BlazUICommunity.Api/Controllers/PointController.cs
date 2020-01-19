using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Threading.Tasks;
using Arch.EntityFrameworkCore.UnitOfWork;
using Arch.EntityFrameworkCore.UnitOfWork.Collections;
using AutoMapper;
using BlazUICommunity.DTO;
using BlazUICommunity.Model.Models;
using BlazUICommunity.Request;
using BlazUICommunity.Utility.Extensions;
using BlazUICommunity.Utility.Response;
using log4net.Repository.Hierarchy;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Swashbuckle.AspNetCore.Annotations;

namespace BlazUICommunity.Api.Controllers
{
    /// <summary>
    /// 积分
    /// </summary>
    [Route("api/[Controller]")]
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
        [HttpPost("Add")]
        public async Task<IActionResult> Add([FromBody] BZPointDto dto)
        {
            var user = _mapper.Map<BZPointModel>(dto);
             await _pointRepository.InsertAsync(user);
            return Ok();
        }



        /// <summary>
        /// 取消
        /// </summary>
        /// <returns></returns>
        [HttpDelete("Delete/{Id}")]
        public IActionResult Delete([FromRoute] int Id)
        {
            _pointRepository.Delete(Id);
            return Ok();
        }

        /// <summary>
        /// 更新
        /// </summary>
        /// <returns></returns>
        [HttpPut("Update/{Id}")]
        public IActionResult Update([FromBody] BZPointDto Dto , [FromRoute] int Id)
        {
            if ( Id < 1 )
                return new BadRequestResponse("id is error");
            var user = _mapper.Map<BZPointModel>(Dto);
            user.Id = Id;

            _pointRepository.Update(user);
            return Ok();
        }

        /// <summary>
        /// 根据Id查询
        /// </summary>
        /// <returns></returns>
        [HttpGet("Query/{Id}")]
        public async Task<IActionResult> Query([FromRoute] int Id)
        {
            var res = await _pointRepository.FindAsync(Id);
            if ( res is null )
                return new NoContentResponse();
            return Ok(res);
        }
        /// <summary>
        /// 根据条件分页查询
        /// </summary>
        /// <returns></returns>
        [HttpPost("Query")]
        public async Task<IActionResult> Query([FromBody] PointRequest Request = null)
        {
            IPagedList<BZPointModel> pagedList = null;
            var query = Request.CreateQueryExpression<BZPointModel , PointRequest>();
            pagedList = query == null ? await _pointRepository.GetPagedListAsync(Request.pageInfo.PageIndex - 1 , Request.pageInfo.PageSize) :
                       await _pointRepository.GetPagedListAsync(query , o => o.OrderBy(p => p.Id) , null , Request.pageInfo.PageIndex - 1 , Request.pageInfo.PageSize);
            return Ok(pagedList);
        }
    }
}