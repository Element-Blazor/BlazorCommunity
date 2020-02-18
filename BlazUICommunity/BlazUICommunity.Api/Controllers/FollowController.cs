using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Threading.Tasks;
using Arch.EntityFrameworkCore.UnitOfWork;
using Arch.EntityFrameworkCore.UnitOfWork.Collections;
using AutoMapper;
using Blazui.Community.Api.Extensions;
using Blazui.Community.DTO;
using Blazui.Community.Model.Models;
using Blazui.Community.Repository;
using Blazui.Community.Request;
using Blazui.Community.Utility.Extensions;
using Blazui.Community.Utility.Response;
using log4net.Repository.Hierarchy;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Swashbuckle.AspNetCore.Annotations;

namespace Blazui.Community.Api.Controllers
{
    /// <summary>
    /// 关注
    /// </summary>
    [Route("api/[Controller]")]
    [ApiController]
    [SwaggerTag(description: "收藏帖子")]
    public class FollowController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IRepository<BZFollowModel> _followRepository;
        private readonly BZFollowRepository _bZFollowRepository;
        private readonly IMapper _mapper;
        /// <summary>
        /// 
        /// </summary>
        /// <param name="unitOfWork"></param>
        /// <param name="mapper"></param>
        public FollowController(IUnitOfWork unitOfWork,
            IMapper mapper, BZFollowRepository bZFollowRepository)
        {
            _mapper = mapper;
            _unitOfWork = unitOfWork;
            _followRepository = unitOfWork.GetRepository<BZFollowModel>(true);
            _bZFollowRepository = bZFollowRepository;
        }


        /// <summary>
        /// 新增关注
        /// </summary>
        /// <returns></returns>
        [HttpPost("Add")]
        public async Task<IActionResult> Add([FromBody] BZFollowDto dto)
        {
            var user = _mapper.Map<BZFollowModel>(dto);
            await _followRepository.InsertAsync(user);
            return Ok();
        }



        /// <summary>
        /// 取消关注
        /// </summary>
        /// <returns></returns>
        [HttpDelete("Delete/{Id}")]
        public IActionResult Delete([FromRoute] int Id)
        {
            _followRepository.Delete(Id);
            return Ok();
        }

        /// <summary>
        /// 更新关注
        /// </summary>
        /// <returns></returns>
        [HttpPut("Update/{Id}")]
        public IActionResult Update([FromBody] BZFollowDto Dto, [FromRoute] int Id)
        {
            if (Id < 1)
                return new BadRequestResponse("id is error");
            var user = _mapper.Map<BZFollowModel>(Dto);
            user.Id = Id;

            _followRepository.Update(user);
            return Ok();
        }

        /// <summary>
        /// 根据Id查询
        /// </summary>
        /// <returns></returns>
        [HttpGet("Query/{Id}")]
        public async Task<IActionResult> Query([FromRoute] int Id)
        {
            var res = await _followRepository.FindAsync(Id);
            if (res is null)
                return new NoContentResponse();
            return Ok(res);
        }
        /// <summary>
        /// 根据条件分页查询
        /// </summary>
        /// <returns></returns>
        [HttpPost("Query")]
        public async Task<IActionResult> Query([FromBody] FollowRequest Request = null)
        {
            IPagedList<BZFollowModel> pagedList = null;
            var query = Request.CreateQueryExpression<BZFollowModel, FollowRequest>();
            query = query.And(p => p.Status == 0);
            pagedList = query == null ? await _followRepository.GetPagedListAsync(Request.pageInfo.PageIndex - 1, Request.pageInfo.PageSize) :
                       await _followRepository.GetPagedListAsync(query, o => o.OrderBy(p => p.Id), null, Request.pageInfo.PageIndex - 1, Request.pageInfo.PageSize);
            if (pagedList.TotalCount > 0)
            {
                var pagedatas = pagedList.ConvertToPageData<BZFollowModel, BZTopicModel>();
                var topicRepository = _unitOfWork.GetRepository<BZTopicModel>();
                var topics = await topicRepository.GetAllAsync(p => pagedList.Items.Select(d => d.TopicId).Contains(p.Id));
                foreach (BZFollowModel follow in pagedList.Items)
                {
                    var topic = topics.FirstOrDefault(p => p.Id == follow.TopicId);
                    topic.PublishTime = follow.FollowTime;
                    pagedatas.Items.Add(topic);
                }
                return Ok(pagedatas);
            }
            else
                return NoContent();
        }

        /// <summary>
        /// 查询指定用户是否收藏了指定帖子
        /// </summary>
        /// <returns></returns>
        [HttpGet("IsStar/{UserId}/{TopicId}")]
        public async Task<IActionResult> IsStar([FromRoute] int UserId, [FromRoute] int TopicId)
        {
            var res = await _followRepository.GetFirstOrDefaultAsync(p => p.UserId == UserId && p.TopicId == TopicId);
            if (res is null)
                return new NoContentResponse();
            return Ok(res);
        }

        /// <summary>
        /// 改变是否收藏状态
        /// </summary>
        /// <returns></returns>
        [HttpPost("Toggle")]
        public async Task<IActionResult> ToggleFollow([FromBody] BZFollowDto Dto)
        {
            if (Dto.Id == 0)
            {
                return await Add(Dto);
            }
            else
            {
                return Update(Dto, Dto.Id);
            }
        }


        /// <summary>
        /// 改变是否收藏状态
        /// </summary>
        /// <returns></returns>
        [HttpDelete("Cancel/{TopicId}/{UserId}")]
        public async Task<IActionResult> CancelFollow([FromRoute] int TopicId, [FromRoute] int UserId)
        {
            var result = await _bZFollowRepository.Cancel(TopicId, UserId);
            if (result)
            {
                return Ok();
            }
            else
                return new BadRequestResponse(" CancelFollow error");
        }
    }
}