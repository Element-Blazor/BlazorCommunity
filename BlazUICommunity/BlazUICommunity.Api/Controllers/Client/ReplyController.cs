using Arch.EntityFrameworkCore.UnitOfWork;
using Arch.EntityFrameworkCore.UnitOfWork.Collections;
using AutoMapper;
using Blazui.Community.Api.Options;
using Blazui.Community.Api.Service;
using Blazui.Community.DTO;
using Blazui.Community.LinqExtensions;
using Blazui.Community.Model.Models;
using Blazui.Community.Repository;
using Blazui.Community.Response;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Swashbuckle.AspNetCore.Annotations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Blazui.Community.Api.Controllers.Client
{
    /// <summary>
    /// 回帖相关
    /// </summary>
    [Route("api/client/[controller]")]
    [ApiController]
    [SwaggerTag(description: "回帖相关")]
    public class ReplyController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly BZReplyRepository _replyRepository;
        private readonly IMapper _mapper;
        private readonly ICacheService _cacheService;
        private readonly IMessageService messageService;
        private readonly IOptions<BaseDomainOptions> domainOption;

        /// <summary>
        ///
        /// </summary>
        /// <param name="unitOfWork"></param>
        /// <param name="bZReplyRepository"></param>
        /// <param name="mapper"></param>
        /// <param name="cacheService"></param>
        /// <param name="messageService"></param>
        /// <param name="EmailNoticeOptions"></param>
        public ReplyController(IUnitOfWork unitOfWork, BZReplyRepository bZReplyRepository,
            IMapper mapper, ICacheService cacheService, IMessageService messageService, IOptions<BaseDomainOptions> domainOption)
        {
            _mapper = mapper;
            _unitOfWork = unitOfWork;
            _replyRepository = bZReplyRepository;
            _cacheService = cacheService;
            this.messageService = messageService;
            this.domainOption = domainOption;
        }

        /// <summary>
        /// 新增回帖
        /// </summary>
        /// <returns></returns>
        [Authorize]
        [HttpPost("Add")]
        public async Task<IActionResult> Add([FromBody] BZReplyDto dto)
        {
            var reply = _mapper.Map<BZReplyModel>(dto);
            var topicRepostory = _unitOfWork.GetRepository<BZTopicModel>(true);
            var replyRepository = _unitOfWork.GetRepository<BZReplyModel>();
            var userRepository = _unitOfWork.GetRepository<BZUserModel>();
            var topic = topicRepostory.GetFirstOrDefault(p => p.Id == dto.TopicId);
            if (topic != null)
            {
                var addResult = await _unitOfWork.CommitWithTransactionAsync(async () =>
                {
                    var result = await replyRepository.InsertAsync(reply);
                    if (!string.IsNullOrWhiteSpace(result.Entity.Id))
                    {
                        _cacheService.Remove(nameof(BZReplyModel));
                        topic.ReplyCount++;
                        topicRepostory.Update(topic);
                        _cacheService.Remove(nameof(BZTopicModel));

                    }
                });
                if (addResult)
                {
                    var user = await userRepository.FindAsync(topic.CreatorId);
                    var content = $"您的帖子—{topic.Title}，有新的回复，查看链接：{domainOption.Value.BaseDomain}topic/{topic.Id}";
                    messageService.SendEmailToTopicCreatorAsync(user?.Email, content);
                    return Ok();
                }
                return new BadRequestResponse("回复帖子失败");
            }
            return new BadRequestResponse("帖子不存在");
        }

        /// <summary>
        /// 根据ID删除帖子
        /// </summary>
        /// <returns></returns>
        [Authorize]
        [HttpDelete("Delete/{Id}")]
        public async Task<IActionResult> Delete([FromRoute] string Id)
        {
            var reply = await _replyRepository.FindAsync(Id);
            if (reply is null)
                return BadRequest();
            var delete = await _replyRepository.ChangeStateByIdAsync(Id, -1, "");
            if (delete)
            {
                _cacheService.Remove(nameof(BZReplyModel));
                var topicRepo = _unitOfWork.GetRepository<BZTopicModel>();
                var topic = topicRepo.GetFirstOrDefault(p => p.Id == reply.TopicId);
                if (topic != null)
                {
                    topic.ReplyCount--;
                    topicRepo.Update(topic);
                    _cacheService.Remove(nameof(BZTopicModel));
                }
            }
            return Ok();
        }

        /// <summary>
        /// 更新回帖
        /// </summary>
        /// <returns></returns>
        [Authorize]
        [HttpPatch("UpdateContent")]
        public IActionResult UpdateContent([FromBody] BZTopicDto Dto)
        {
            if (string.IsNullOrWhiteSpace(Dto.Id))
                return new BadRequestResponse("id is error");
            var reply = _replyRepository.Find(Dto.Id);
            if (reply == null)
                return new BadRequestResponse("no this error");
            reply.Content = Dto.Content;
            reply.LastModifyDate = DateTime.Now;
            reply.LastModifierId = Dto.LastModifierId;
            _replyRepository.Update(reply);
            _cacheService.Remove(nameof(BZReplyModel));
            return Ok();
        }

        /// <summary>
        /// 根据用户查询回复帖子
        /// </summary>
        /// <returns></returns>
        [HttpGet("GetByUserId/{UserId}/{PageSize}/{PageIndex}")]
        [HttpGet("GetByUserId/{UserId}/{PageSize}/{PageIndex}/{Title}")]
        public async Task<IActionResult> GetByUserId(string UserId, int PageSize, int PageIndex, string Title = null)
        {
            if (string.IsNullOrWhiteSpace(UserId)) return BadRequest(nameof(UserId));

            Expression<Func<BZReplyModel, bool>> query = p => p.CreatorId == UserId && p.Status == 0;
            if (!string.IsNullOrWhiteSpace(Title))
            {
                var TopicList = await _unitOfWork.GetRepository<BZTopicModel>().GetAllAsync(p => p.Status == 0 && p.Title.Contains(Title));
                if (TopicList != null && TopicList.Any())
                    query = query.And(p => TopicList.Select(x => x.Id).Contains(p.TopicId));
                else return NoContent();
            }

            var replyList = await _replyRepository.GetPagedListAsync(query, o => o.OrderByDescending(p => p.CreateDate), null, PageIndex - 1, PageSize);
            if (replyList.Items.Any())
            {
                var replysDatas = replyList.From(result => _mapper.Map<List<PersonalReplyDisplayDto>>(result));
                var topics = await _cacheService.GetTopicsAsync(p => replyList.Items.Select(x => x.TopicId).Contains(p.Id));
                var users = await _cacheService.GetUsersAsync(p => topics.Select(x => x.CreatorId).Contains(p.Id));
                foreach (var reply in replysDatas.Items)
                {
                    var topic = topics?.FirstOrDefault(p => p.Id == reply.TopicId);
                    var user = users?.FirstOrDefault(p => p.Id == topic?.CreatorId);
                    reply.Author = user?.NickName;
                    reply.Title = topic.Title;
                }
                return Ok(replysDatas);
            }
            else
            {
                return NoContent();
            }
        }
    }
}