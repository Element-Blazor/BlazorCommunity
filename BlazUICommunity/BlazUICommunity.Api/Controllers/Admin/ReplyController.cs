using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
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
using Blazui.Community.Utility.Filter;
using Blazui.Community.Utility.Response;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace Blazui.Community.Api.Controllers
{
    /// <summary>
    /// 回帖相关
    /// </summary>
    [HiddenApi]
    [Route("api/[Controller]")]
    [ApiController]
    [SwaggerTag(description: "回帖相关")]
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
        /// 新增回帖
        /// </summary>
        /// <returns></returns>
        [HttpPost("Add")]
        public async Task<IActionResult> Add([FromBody] BZReplyDto dto)
        {
            var reply = _mapper.Map<BZReplyModel>(dto);
            var topicRepostory = _unitOfWork.GetRepository<BZTopicModel>(true);
            var replyRepository = _unitOfWork.GetRepository<BZReplyModel>();
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
                    return Ok();
                return new BadRequestResponse("回复帖子失败");
            }
            return new BadRequestResponse("帖子不存在");
        }


        /// <summary>
        /// 根据用户查询回复帖子
        /// </summary>
        /// <returns></returns>
        [HttpGet("GetByUserId/{UserId}/{PageSize}/{PageIndex}")]
        public async Task<IActionResult> GetByUserId(string UserId, int PageSize, int PageIndex)
        {
            var res = await _replyRepository.QueryMyReplys(UserId, PageSize, PageIndex - 1);
            return Ok(res);
        }

        /// <summary>
        /// 根据ID删除帖子
        /// </summary>
        /// <returns></returns>
        [HttpDelete("Delete/{Id}")]
        public async Task<IActionResult> Delete([FromRoute] string Id)
        {

            await _unitOfWork.CommitWithTransactionAsync(() =>
            {

                var delete = _replyRepository.LogicDelete(Id);
                if (delete)
                {
                    _cacheService.Remove(nameof(BZReplyModel));
                    var reply = _replyRepository.Find(Id);
                    var topicRepo = _unitOfWork.GetRepository<BZTopicModel>();
                    var topic = topicRepo.GetFirstOrDefault(p => p.Id == reply.TopicId);
                    if (topic != null)
                    {
                        topic.ReplyCount--;
                        topicRepo.Update(topic);
                        _cacheService.Remove(nameof(BZTopicModel));
                    }
                }
            });
            return Ok();
        }

        /// <summary>
        /// 删除或激活帖子
        /// </summary>
        /// <returns></returns>
        [HttpDelete("DeleteOrActive/{Id}")]
        public async Task<IActionResult> DeleteOrActive([FromRoute] string Id)
        {
            var topicRepo = _unitOfWork.GetRepository<BZTopicModel>();
            var result = await _unitOfWork.CommitWithTransactionAsync(() =>
           {
               var reply = _replyRepository.Find(Id);
               var delete = false;
               if (reply != null && reply.Status == 0)
               {
                   delete = _replyRepository.LogicDelete(reply.Id);
               }
               else
               {
                   delete = _replyRepository.LogicRecovery(reply.Id);
               }

               if (delete)
               {
                   _cacheService.Remove(nameof(BZReplyModel));
                   var topic = topicRepo.GetFirstOrDefault(p => p.Id == reply.TopicId);
                   if (topic != null)
                   {
                       if (reply.Status == 0)
                       {
                           topic.ReplyCount--;
                       }
                       else
                       {
                           topic.ReplyCount++;
                       }
                       topicRepo.Update(topic);
                       _cacheService.Remove(nameof(BZTopicModel));
                   }
               }
           });
            return Ok();
        }

        /// <summary>
        /// 更新回帖
        /// </summary>
        /// <returns></returns>
        [HttpPut("Update/{Id}")]
        public IActionResult Update([FromBody] BZReplyDto Dto, [FromRoute] string Id)
        {
            if (string.IsNullOrWhiteSpace(Id))
                return new BadRequestResponse("id is error");
            var reply = _mapper.Map<BZReplyModel>(Dto);
            reply.Id = Id;

            _replyRepository.Update(reply);
            _cacheService.Remove(nameof(BZReplyModel));
            return Ok();
        }

        /// <summary>
        /// 更新回帖
        /// </summary>
        /// <returns></returns>
        [HttpPost("UpdateContent")]
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
        /// 根据Id查询回帖
        /// </summary>
        /// <returns></returns>
        [HttpGet("Query/{Id}")]
        public async Task<IActionResult> Query([FromRoute] string Id)
        {
            var replys = await _cacheService.Replys(p => p.Id == Id);
            if (replys.Any())
                return Ok(_mapper.Map<BZReplyDto>(replys.FirstOrDefault()));
            var res = await _replyRepository.FindAsync(Id);
            if (res is null)
                return NoContent();
            return Ok(res);
        }
        /// <summary>
        /// 根据条件分页查询回帖
        /// </summary>
        /// <returns></returns>
        [HttpPost("Query")]
        public async Task<IActionResult> Query([FromBody] ReplyRequestCondition Request = null)
        {
            IPagedList<BZReplyModel> pagedList = null;
            var query = Request.CreateQueryExpression<BZReplyModel, ReplyRequestCondition>();
            query ??= p => true;
            query = query.And(p => p.Status == 0);
            pagedList = await _replyRepository.GetPagedListAsync(query, o => o.OrderBy(p => p.Id), null, Request.PageInfo.PageIndex - 1, Request.PageInfo.PageSize);
            if (pagedList.TotalCount > 0)
            {
                var pagedatas = pagedList.ConvertToPageData<BZReplyModel, BZReplyDto>();
                var topics = await _cacheService.Topics(p => pagedList.Items.Select(d => d.TopicId).Contains(p.Id));
                var Users = await _cacheService.Users(p => pagedList.Items.Select(d => d.CreatorId).Contains(p.Id));
                foreach (var replyDto in pagedList.Items)
                {
                    var user = Users.FirstOrDefault(p => p.Id == replyDto.CreatorId);
                    var topic = topics.FirstOrDefault(p => p.Id == replyDto.TopicId);
                    var replywithuser = _mapper.Map<BZReplyDto>(replyDto);
                    pagedatas.Items.Add(replywithuser);
                    if (user != null)
                    {
                        replywithuser.NickName = user?.NickName;
                        replywithuser.Avator = user?.Avator;
                        replywithuser.UserName = user?.UserName;
                        replywithuser.UserId = user.Id;
                        replywithuser.Title = topic?.Title;
                    }
                }
                pagedatas.Items = pagedatas.Items.OrderBy(p => p.CreateDate).ToList();
                return Ok(pagedatas);
            }
            else
            {
                return NoContent();
            }
        }


        /// <summary>
        /// 根据条件分页查询回帖
        /// </summary>
        /// <returns></returns>
        [HttpPost("QueryReplys")]
        public async Task<IActionResult> QueryReplys([FromBody] ReplyRequestCondition Request = null
            , [SwaggerParameter(Required = false)] string userName = "",
            [SwaggerParameter(Required = false)] string topicTitle = "")
        {
            IPagedList<BZReplyModel> pagedList = null;
            var query = Request.CreateQueryExpression<BZReplyModel, ReplyRequestCondition>();
            IList<BZUserModel> Users = new List<BZUserModel>();
            IList<BZTopicModel> Topics = new List<BZTopicModel>();
            Expression<Func<BZTopicModel, bool>> where = p => true;
            if (!string.IsNullOrWhiteSpace(topicTitle))
            {
                where = where.And(p => p.Title.Contains(topicTitle));
                Topics = await _cacheService.Topics(where);
                if (Topics != null && Topics.Any())
                    query = query.And(p => Topics.Select(x => x.Id).Contains(p.TopicId));
                else
                    return NoContent();
            }
            if (!string.IsNullOrWhiteSpace(userName))
            {
                Users = await _cacheService.Users(p => p.UserName.Contains(userName) || p.NickName.Contains(userName));
                if (Users.Any())
                    query = query.And(p => Users.Select(x => x.Id).Contains(p.CreatorId));
                else
                    return NoContent();
            }
            pagedList = await _replyRepository.GetPagedListAsync(query, o => o.OrderBy(p => p.Id), null, Request.PageInfo.PageIndex - 1, Request.PageInfo.PageSize);
            if (pagedList.TotalCount > 0)
            {
                var pagedatas = pagedList.ConvertToPageData<BZReplyModel, BZReplyDto>();
                var topics = await _cacheService.Topics(p => pagedList.Items.Select(d => d.TopicId).Contains(p.Id));
                var users = await _cacheService.Users(p => pagedList.Items.Select(d => d.CreatorId).Contains(p.Id));
                foreach (var replyDto in pagedList.Items)
                {
                    var user = users.FirstOrDefault(p => p.Id == replyDto.CreatorId);
                    var topic = topics.FirstOrDefault(p => p.Id == replyDto.TopicId);
                    var replywithuser = _mapper.Map<BZReplyDto>(replyDto);
                    pagedatas.Items.Add(replywithuser);
                    if (user != null)
                    {
                        replywithuser.NickName = user?.NickName;
                        replywithuser.Avator = user?.Avator;
                        replywithuser.UserName = user?.UserName;
                        replywithuser.UserId = user.Id;
                        replywithuser.Title = topic?.Title;
                    }
                }
                pagedatas.Items = pagedatas.Items.OrderByDescending(p => p.CreateDate).ToList();
                return Ok(pagedatas);
            }
            else
            {
                return NoContent();
            }
        }
    }
}