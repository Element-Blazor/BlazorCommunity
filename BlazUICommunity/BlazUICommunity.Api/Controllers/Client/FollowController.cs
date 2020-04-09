using Arch.EntityFrameworkCore.UnitOfWork;
using Arch.EntityFrameworkCore.UnitOfWork.Collections;
using AutoMapper;
using Blazui.Community.Api.Service;
using Blazui.Community.DTO;
using Blazui.Community.LinqExtensions;
using Blazui.Community.Model.Models;
using Blazui.Community.Repository;
using Blazui.Community.Request;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

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
        /// <param name="dto"></param>
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
        /// 根据条件分页查询
        /// </summary>
        /// <param name="Request"></param>
        /// <returns></returns>
        [HttpGet("Query")]
        public async Task<IActionResult> Query([FromQuery] FollowRequestCondition Request = null)
        {
            var query = Request.CreateQueryExpression<BZFollowModel, FollowRequestCondition>();
            query = query.And(p => p.Status == 0);
            var followList = await _cacheService.GetFollowsAsync(query);

            if (followList.Any())
            {
                var followTopicIds = followList.Select(p => p.TopicId);
                var topicRepository = _unitOfWork.GetRepository<BZTopicModel>();

                Expression<Func<BZTopicModel, bool>> expression = p => p.Status == 0 && followTopicIds.Contains(p.Id);
                if (!string.IsNullOrWhiteSpace(Request.TopicTitle))
                    expression = p => p.Status == 0 && followTopicIds.Contains(p.Id) && p.Title.Contains(Request.TopicTitle);

                var topicList = await topicRepository.GetPagedListAsync(expression, o => o.OrderByDescending(p => p.CreateDate), null, Request.PageIndex - 1, Request.PageSize);
                var pagedatas = topicList.From(result => _mapper.Map<List<PersonalFollowDisplayDto>>(result));
                var Users = await _cacheService.GetUsersAsync(p => pagedatas.Items.Select(d => d.CreatorId).Contains(p.Id));
                foreach (PersonalFollowDisplayDto topic in pagedatas.Items)
                {
                    var User = Users.FirstOrDefault(p => p.Id == topic.CreatorId);
                    topic.UserName = User?.UserName;
                    topic.NickName = User?.NickName;
                    topic.Avator = User?.Avator;
                    topic.FollowId = followList.FirstOrDefault(p => p.TopicId == topic.Id)?.Id;
                }
                return Ok(pagedatas);
            }
            else
                return NoContent();
        }

        /// <summary>
        /// 用户是否收藏了指定帖子
        /// </summary>
        /// <param name="UserId"></param>
        /// <param name="TopicId"></param>
        /// <returns></returns>
        [HttpGet("IsFollowed/{UserId}/{TopicId}")]
        public async Task<IActionResult> IsFollowed([FromRoute] string UserId, [FromRoute] string TopicId)
        {
            var res = await _followRepository.GetFirstOrDefaultAsync(p => p.CreatorId == UserId && p.TopicId == TopicId);
            if (res is null)
                return NoContent();
            return Ok(_mapper.Map<BZFollowDto>(res));
        }

        /// <summary>
        /// 改变是否收藏状态
        /// </summary>
        /// <param name="Dto"></param>
        /// <returns></returns>
        [Authorize]
        [HttpPatch("Toggle")]
        public async Task<IActionResult> ToggleFollow([FromBody] BZFollowDto Dto)
        {
            if (string.IsNullOrWhiteSpace(Dto.Id))
            {
                return await Add(Dto);
            }
            else
            {
                if (await _bZFollowRepository.ChangeStateByIdAsync(Dto.Id, Dto.Status == 0 ? -1 : 0, ""))
                {
                    _cacheService.Remove(nameof(BZFollowModel));
                    return Ok();
                }
                else
                {
                    return BadRequest();
                }
            }
        }

        /// <summary>
        /// 取消收藏
        /// </summary>
        /// <param name="FollowId"></param>
        /// <returns></returns>
        [Authorize]
        [HttpPatch("Cancel/{FollowId}")]
        public async Task<IActionResult> Cancel([FromRoute] string FollowId)
        {
            if (await _bZFollowRepository.ChangeStateByIdAsync(FollowId, -1, ""))
            {
                _cacheService.Remove(nameof(BZFollowModel));
                return Ok();
            }
            return BadRequest();
        }
    }
}