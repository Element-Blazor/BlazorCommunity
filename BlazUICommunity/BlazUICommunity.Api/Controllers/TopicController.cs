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
    /// 主题帖相关
    /// </summary>
    [Route("api/[Controller]")]
    [ApiController]
    [SwaggerTag(description: "主贴相关")]
    public class TopicController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IRepository<BZTopicModel> _topicRepository;
        private readonly IMapper _mapper;
        /// <summary>
        /// 
        /// </summary>
        /// <param name="unitOfWork"></param>
        /// <param name="mapper"></param>
        public TopicController(IUnitOfWork unitOfWork ,
            IMapper mapper)
        {
            _mapper = mapper;
            _unitOfWork = unitOfWork;
            _topicRepository = unitOfWork.GetRepository<BZTopicModel>(true);
        }


        /// <summary>
        /// 新增主题帖
        /// </summary>
        /// <returns></returns>
        [HttpPost("Add")]
        public async Task<IActionResult> Add([FromBody] BZTopicDto dto)
        {
            var user = _mapper.Map<BZTopicModel>(dto);
             await _topicRepository.InsertAsync(user);
            return Ok();
        }



        /// <summary>
        /// 根据ID删除帖子
        /// </summary>
        /// <returns></returns>
        [HttpDelete("Delete/{Id}")]
        public IActionResult Delete([FromRoute] int Id)
        {
            _topicRepository.Delete(Id);
            return Ok();
        }

        /// <summary>
        /// 更新主题帖
        /// </summary>
        /// <returns></returns>
        [HttpPut("Update/{Id}")]
        public IActionResult Update([FromBody] BZTopicDto Dto , [FromRoute] int Id)
        {
            if ( Id < 1 )
                return new BadRequestResponse("id is error");
            var user = _mapper.Map<BZTopicModel>(Dto);
            user.Id = Id;

            _topicRepository.Update(user);
            return Ok();
        }

        /// <summary>
        /// 根据Id查询主贴
        /// </summary>
        /// <returns></returns>
        [HttpGet("Query/{Id}")]
        public async Task<IActionResult> Query([FromRoute] int Id)
        {
            var res = await _topicRepository.FindAsync(Id);
            if ( res is null )
                return new NoContentResponse();
            return Ok(res);
        }
        /// <summary>
        /// 根据条件分页查询帖子
        /// </summary>
        /// <returns></returns>
        [HttpPost("Query")]
        public async Task<IActionResult> Query([FromBody] TopicRequest Request = null)
        {
            IPagedList<BZTopicModel> pagedList = null;
            var query = Request.CreateQueryExpression<BZTopicModel , TopicRequest>();
            pagedList = query == null ? await _topicRepository.GetPagedListAsync(Request.pageInfo.PageIndex - 1 , Request.pageInfo.PageSize) :
                       await _topicRepository.GetPagedListAsync(query , o => o.OrderBy(p => p.Id) , null , Request.pageInfo.PageIndex - 1 , Request.pageInfo.PageSize);
            return Ok(pagedList);
        }
    }
}