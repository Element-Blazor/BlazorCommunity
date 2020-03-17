using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Threading.Tasks;
using Arch.EntityFrameworkCore.UnitOfWork;
using Arch.EntityFrameworkCore.UnitOfWork.Collections;
using AutoMapper;
using Blazui.Community.Api.Extensions;
using Blazui.Community.Api.Service;
using Blazui.Community.DTO;
using Blazui.Community.Model.Models;
using Blazui.Community.Repository;
using Blazui.Community.Request;
using Blazui.Community.Utility.Extensions;
using Blazui.Community.Utility.Response;
using log4net.Repository.Hierarchy;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using Swashbuckle.AspNetCore.Annotations;

namespace Blazui.Community.Api.Controllers.Client
{
    /// <summary>
    /// 关注
    /// </summary>
    [Route("api/client/[controller]")]
    [ApiController]
    [SwaggerTag(description: "收藏帖子")]
    public class FollowController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IRepository<BZFollowModel> _followRepository;
        private readonly BZFollowRepository _bZFollowRepository;
        private readonly IMapper _mapper;
        private readonly ICacheService _cacheService;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="unitOfWork"></param>
        /// <param name="mapper"></param>
        /// <param name="bZFollowRepository"></param>
        /// <param name="cacheService"></param>
        public FollowController(IUnitOfWork unitOfWork,
            IMapper mapper, BZFollowRepository bZFollowRepository, ICacheService cacheService)
        {
            _mapper = mapper;
            _unitOfWork = unitOfWork;
            _followRepository = unitOfWork.GetRepository<BZFollowModel>(true);
            _bZFollowRepository = bZFollowRepository;
            this._cacheService = cacheService;
        }


        /// <summary>
        /// 新增关注
        /// </summary>
        /// <returns></returns>
        [Authorize]
        [HttpPost("Add")]
        public async Task<IActionResult> Add([FromBody] BZFollowDto dto)
        {
            var follow = _mapper.Map<BZFollowModel>(dto);
            var result = await _followRepository.InsertAsync(follow);
            var data = result?.Entity == null ? false : true;
            if (data)
                _cacheService.Remove(nameof(BZFollowModel));
            return Ok(data);
        }



        /// <summary>
        /// 取消关注
        /// </summary>
        /// <returns></returns>
        [Authorize]
        [HttpDelete("Delete/{Id}/{oprationId?}")]
        public async Task<IActionResult> Delete(string Id, string oprationId)
        {
            var result = await _followRepository.ChangeStateByIdAsync(Id,-1, oprationId);
            _cacheService.Remove(nameof(BZFollowModel));
            return Ok(result);
        }



        /// <summary>
        /// 根据Id查询
        /// </summary>
        /// <returns></returns>
        [HttpGet("Query/{Id}")]
        public async Task<IActionResult> Query([FromRoute] string Id)
        {
            var follows = await _cacheService.Follows(p => p.Id == Id);
            if (follows.Any())
                return Ok(_mapper.Map<BZFollowDto>(follows.FirstOrDefault()));
            var res = await _followRepository.FindAsync(Id);
            if (res != null)
                return Ok(_mapper.Map<BZFollowDto>(res));
            return NoContent();
        }
        /// <summary>
        /// 根据条件分页查询
        /// </summary>
        /// <returns></returns>
        [HttpPost("Query")]
        public async Task<IActionResult> Query([FromBody] FollowRequestCondition Request = null)
        {
            IPagedList<BZFollowModel> pagedList = null;
            var query = Request.CreateQueryExpression<BZFollowModel, FollowRequestCondition>();
            query = query.And(p => p.Status == 0);
            if(!string.IsNullOrWhiteSpace(Request.TopicTitle))
            {
                var topicscontaintitle = await _cacheService.Topics(p => p.Title.IfContains(Request.TopicTitle));
                if (topicscontaintitle?.Count > 0)
                {
                    query = query.And(p => topicscontaintitle.Select(x => x.Id).Contains(p.TopicId));
                }
                else
                    return NoContent();
            }
            pagedList = await _followRepository.GetPagedListAsync(query, o => o.OrderBy(p => p.Id), null, Request.PageIndex - 1, Request.PageSize);
            if (pagedList.TotalCount > 0)
            {
                var pagedatas = pagedList.ConvertToPageData<BZFollowModel, BZTopicDto>();
                var topics = await _cacheService.Topics(p => pagedList.Items.Select(d => d.TopicId).Contains(p.Id));
                var Users = await _cacheService.Users(p => topics.Select(d => d.CreatorId).Contains(p.Id));
                foreach (BZFollowModel follow in pagedList.Items)
                {
                    var topic = topics.FirstOrDefault(p => p.Id == follow.TopicId);
                    if (topic != null)
                    {
                        var topicdto = _mapper.Map<BZTopicDto>(topic);
                        var User = Users.FirstOrDefault(p => p.Id == topic.CreatorId);
                        if (User != null)
                        {
                            topicdto.UserName = User.UserName;
                            topicdto.NickName = User.NickName;
                            topicdto.Avator = User.Avator;
                        }
                        pagedatas.Items.Add(topicdto);
                    }
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
        public async Task<IActionResult> IsStar([FromRoute] string UserId, [FromRoute] string TopicId)
        {
            var res = await _followRepository.GetFirstOrDefaultAsync(p => p.CreatorId == UserId && p.TopicId == TopicId);
            if (res is null)
                return NoContent();
            return Ok(_mapper.Map<BZFollowDto>(res));
        }

        /// <summary>
        /// 改变是否收藏状态
        /// </summary>
        /// <returns></returns>
        [Authorize]
        [HttpPost("Toggle")]
        public async Task<IActionResult> ToggleFollow([FromBody] BZFollowDto Dto)
        {

            if (string.IsNullOrWhiteSpace(Dto.Id))
                return await Add(Dto);
            else
            {
                await _bZFollowRepository.ChangeStateByIdAsync(Dto.Id,Dto.Status==0?-1:0,"");
            }
            _cacheService.Remove(nameof(BZFollowModel));
            return Ok();
        }

        /// <summary>
        /// 更新关注
        /// </summary>
        /// <returns></returns>
        [Authorize]
        [HttpPut("Update")]
        public IActionResult Update([FromBody] BZFollowDto Dto)
        {
            var follow = _mapper.Map<BZFollowModel>(Dto);
            _followRepository.Update(follow);
            _cacheService.Remove(nameof(BZFollowModel));
            return Ok();
        }
        /// <summary>
        /// 改变是否收藏状态
        /// </summary>
        /// <returns></returns>
        [Authorize]
        [HttpDelete("Cancel/{TopicId}/{UserId}")]
        public async Task<IActionResult> CancelFollow([FromRoute] string TopicId, [FromRoute] string UserId)
        {
            if (await _bZFollowRepository.Cancel(TopicId, UserId))
            {
                _cacheService.Remove(nameof(BZFollowModel));
                return Ok();
            }
            else
                return new BadRequestResponse("  error");
        }
    }
}