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
        /// 根据条件分页查询回帖
        /// </summary>
        /// <returns></returns>
        [HttpGet("Query")]
        public async Task<IActionResult> QueryReplys([FromQuery] ReplyRequestCondition Request = null)
        {
            IPagedList<BZReplyModel> pagedList = null;
            var query = Request.CreateQueryExpression<BZReplyModel, ReplyRequestCondition>();
            IList<BZUserModel> Users = new List<BZUserModel>();
            IList<BZTopicModel> Topics = new List<BZTopicModel>();
            Expression<Func<BZTopicModel, bool>> where = p => true;
            if (!string.IsNullOrWhiteSpace(Request.Title))
            {
                where = where.And(p => p.Title.Contains(Request.Title));
                Topics = await _cacheService.Topics(where);
                if (Topics != null && Topics.Any())
                    query = query.And(p => Topics.Select(x => x.Id).Contains(p.TopicId));
                else
                    return NoContent();
            }
            if (!string.IsNullOrWhiteSpace(Request.UserName))
            {
                Users = await _cacheService.Users(p => 
                p.UserName.ToLower().Contains(Request.UserName.ToLower()) ||
                p.NickName.ToLower().Contains(Request.UserName.ToLower()));
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