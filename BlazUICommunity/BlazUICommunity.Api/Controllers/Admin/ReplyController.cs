using Arch.EntityFrameworkCore.UnitOfWork;
using Arch.EntityFrameworkCore.UnitOfWork.Collections;
using AutoMapper;
using Blazui.Community.Api.Service;
using Blazui.Community.DTO;
using Blazui.Community.LinqExtensions;
using Blazui.Community.Model.Models;
using Blazui.Community.Repository;
using Blazui.Community.Request;
using Blazui.Community.StringExtensions;
using Blazui.Community.SwaggerExtensions;
using Marvin.Cache.Headers;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Blazui.Community.Api.Controllers
{
    /// <summary>
    /// 回帖相关
    /// </summary>
    [HiddenApi]
    [Route("api/[Controller]")]
    [ApiController]
    [SwaggerTag(description: "回帖相关")]
    //[HttpCacheExpiration(MaxAge = 100)]
    public class ReplyController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly BZReplyRepository _replyRepository;
        private readonly IMapper _mapper;
        private readonly ICacheService _cacheService;

        /// <summary>
        ///
        /// </summary>
        /// <param name="unitOfWork"></param>
        /// <param name="bZReplyRepository"></param>
        /// <param name="mapper"></param>
        /// <param name="cacheService"></param>
        public ReplyController(IUnitOfWork unitOfWork, BZReplyRepository bZReplyRepository,
            IMapper mapper, ICacheService cacheService)
        {
            _mapper = mapper;
            _unitOfWork = unitOfWork;
            _replyRepository = bZReplyRepository;
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
            var reply = await _replyRepository.FindAsync(Id);
            if (reply is null)
                return BadRequest();
            if (reply.Status == Status)
                return Ok();
            reply.Status = Status;
            _replyRepository.Update(reply);
            _cacheService.Remove(nameof(BZReplyModel));
            return Ok();
        }

        /// <summary>
        /// 根据条件分页查询回帖
        /// </summary>
        /// <returns></returns>
        [HttpGet("Query")]
        public async Task<IActionResult> QueryReplys([FromQuery] ReplyRequestCondition Request = null)
        {
            IPagedList<BZReplyModel> pagedList = null;
            var query = Request.CreateQueryExpression<BZReplyModel, ReplyRequestCondition>();

            Expression<Func<BZTopicModel, bool>> where = p => true;
            if (!string.IsNullOrWhiteSpace(Request.Title))
            {
                where = where.And(p => p.Title.Contains(Request.Title));
                var Topics = await _cacheService.Topics(where);
                if (Topics != null && Topics.Any())
                    query = query.And(p => Topics.Select(x => x.Id).Contains(p.TopicId));
                else
                    return NoContent();
            }
            if (!string.IsNullOrWhiteSpace(Request.UserName))
            {
                var Users = await _cacheService.Users(p =>
               p.UserName.Contains(Request.UserName) ||
               p.NickName.Contains(Request.UserName));
                if (Users.Any())
                    query = query.And(p => Users.Select(x => x.Id).Contains(p.CreatorId));
                else
                    return NoContent();
            }
            pagedList = await _replyRepository.GetPagedListAsync(query, o => o.OrderByDescending(p => p.CreateDate), null, Request.PageIndex - 1, Request.PageSize);
            if (pagedList.Items.Any())
            {
                var pagedatas = pagedList.From(result => _mapper.Map<List<BZReplyDto>>(result));
                var topics = await _cacheService.Topics(p => pagedList.Items.Select(d => d.TopicId).Contains(p.Id));
                var users = await _cacheService.Users(p => pagedList.Items.Select(d => d.CreatorId).Contains(p.Id));
                foreach (var replyDto in pagedatas.Items)
                {
                    var user = users.FirstOrDefault(p => p.Id == replyDto.CreatorId);
                    var topic = topics.FirstOrDefault(p => p.Id == replyDto.TopicId);
                    if (user != null)
                    {
                        replyDto.NickName = user?.NickName;
                        replyDto.Avator = user?.Avator;
                        replyDto.UserName = user?.UserName;
                        replyDto.UserId = user.Id;
                        replyDto.Title = topic?.Title;
                    }
                }
                return Ok(pagedatas);
            }
            else
            {
                return NoContent();
            }
        }
    }
}