using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
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
    /// 回帖相关
    /// </summary>
    [Route("api/[Controller]")]
    [ApiController]
    [SwaggerTag(description: "回帖相关")]
    public class ReplyController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        //private readonly IRepository<BZReplyModel> _replyRepository;
        private readonly BZReplyRepository _replyRepository;
        private readonly IMapper _mapper;


        /// <summary>
        /// 
        /// </summary>
        /// <param name="unitOfWork"></param>
        /// <param name="bZReplyRepository"></param>
        /// <param name="mapper"></param>
        public ReplyController(IUnitOfWork unitOfWork, BZReplyRepository bZReplyRepository,
            IMapper mapper)
        {
            _mapper = mapper;
            _unitOfWork = unitOfWork;
            _replyRepository = bZReplyRepository;
            //_replyRepository = unitOfWork.GetRepository<BZReplyModel>(true);
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
            var addResult = await _unitOfWork.CommitWithTransactionAsync(async () =>
               {
                   var result = await replyRepository.InsertAsync(reply);
                   if (result.Entity.Id > 0)
                   {
                       var topic = topicRepostory.GetFirstOrDefault(p => p.Id == dto.TopicId);
                       topic.ReplyCount++;
                       topicRepostory.Update(topic);
                   }
               });
            if (addResult)
                return Ok();
            return new BadRequestResponse("回复帖子失败");

        }


        /// <summary>
        /// 根据ID删除帖子
        /// </summary>
        /// <returns></returns>
        [HttpGet("GetByUserId/{UserId}/{PageSize}/{PageIndex}")]
        public async Task<IActionResult> GetByUserId([FromRoute] int UserId, int PageSize, int PageIndex)
        {
            var res = await _replyRepository.QueryMyReplys(UserId, PageSize, PageIndex - 1);
            return Ok(res);
        }

        /// <summary>
        /// 根据ID删除帖子
        /// </summary>
        /// <returns></returns>
        [HttpDelete("Delete/{Id}")]
        public async Task<IActionResult> Delete([FromRoute] int Id)
        {

            await _unitOfWork.CommitWithTransactionAsync(() =>
            {
                var reply = _replyRepository.Find(Id);
                var delete = _replyRepository.FakeDelete(Id);
                var topicRepo = _unitOfWork.GetRepository<BZTopicModel>();
                var topic = topicRepo.GetFirstOrDefault(p => p.Id == reply.TopicId);
                if (topic != null)
                {
                    topic.ReplyCount--;
                    topicRepo.Update(topic);
                }
            });
            return Ok();
        }

        /// <summary>
        /// 删除或激活帖子
        /// </summary>
        /// <returns></returns>
        [HttpDelete("DeleteOrActive/{Id}")]
        public async Task<IActionResult> DeleteOrActive([FromRoute] int Id)
        {
            var topicRepo = _unitOfWork.GetRepository<BZTopicModel>();
            var result = await _unitOfWork.CommitWithTransactionAsync( () =>
            {
                var reply =  _replyRepository.Find(Id);

                 _replyRepository.DeleteOrActive(Id, reply.Status == 0 ? -1 : 0);

                var topic =  topicRepo.GetFirstOrDefault(p => p.Id == reply.TopicId);
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
                }
            });
            return Ok();
        }

        /// <summary>
        /// 更新回帖
        /// </summary>
        /// <returns></returns>
        [HttpPut("Update/{Id}")]
        public IActionResult Update([FromBody] BZReplyDto Dto, [FromRoute] int Id)
        {
            if (Id < 1)
                return new BadRequestResponse("id is error");
            var reply = _mapper.Map<BZReplyModel>(Dto);
            reply.Id = Id;

            _replyRepository.Update(reply);
            return Ok();
        }

        /// <summary>
        /// 更新主题帖
        /// </summary>
        /// <returns></returns>
        [HttpPost("UpdateContent")]
        public IActionResult UpdateContent([FromBody] BZTopicDto Dto)
        {
            if (Dto.Id < 1)
                return new BadRequestResponse("id is error");
            var reply = _replyRepository.Find(Dto.Id);
            if (reply == null)
                return new BadRequestResponse("no this error");
            reply.Content = Dto.Content;
            reply.ModifyTime = DateTime.Now;
            _replyRepository.Update(reply);
            return Ok();
        }
        /// <summary>
        /// 根据Id查询回帖
        /// </summary>
        /// <returns></returns>
        [HttpGet("Query/{Id}")]
        public async Task<IActionResult> Query([FromRoute] int Id)
        {
            var res = await _replyRepository.FindAsync(Id);
            if (res is null)
                return new NoContentResponse();
            return Ok(res);
        }
        /// <summary>
        /// 根据条件分页查询回帖
        /// </summary>
        /// <returns></returns>
        [HttpPost("Query")]
        public async Task<IActionResult> Query([FromBody] ReplyRequest Request = null)
        {
            IPagedList<BZReplyModel> pagedList = null;
            var query = Request.CreateQueryExpression<BZReplyModel, ReplyRequest>();
            query ??= p => true;
            query = query.And(p => p.Status == 0);
            pagedList = await _replyRepository.GetPagedListAsync(query, o => o.OrderBy(p => p.Id), null, Request.pageInfo.PageIndex - 1, Request.pageInfo.PageSize);


            if (pagedList.TotalCount > 0)
            {
                var pagedatas = pagedList.ConvertToPageData<BZReplyModel, BZReplyDtoWithUser>();
                var userRepository = _unitOfWork.GetRepository<BZUserModel>();
                var topicRepository = _unitOfWork.GetRepository<BZTopicModel>();
                var users = await userRepository.GetAllAsync(p => pagedList.Items.Select(d => d.UserId).Contains(p.Id));
                var topics = await topicRepository.GetAllAsync(p => pagedList.Items.Select(p => p.TopicId).Contains(p.Id));
                foreach (var replyDto in pagedList.Items)
                {
                    var user = users.FirstOrDefault(p => p.Id == replyDto.UserId);
                    var topic = topics.FirstOrDefault(p => p.Id == replyDto.TopicId);
                    var replywithuser = _mapper.Map<BZReplyDtoWithUser>(replyDto);
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
                pagedatas.Items = pagedatas.Items.OrderBy(p => p.PublishTime).ToList();
                return Ok(pagedatas);
            }
            else
            {
                return new NoContentResponse();
            }
        }


        /// <summary>
        /// 根据条件分页查询回帖
        /// </summary>
        /// <returns></returns>
        [HttpPost("QueryReplys")]
        public async Task<IActionResult> QueryReplys([FromBody] ReplyRequest Request = null
            , [SwaggerParameter(Required = false)] string userName = "",
            [SwaggerParameter(Required = false)] string topicTitle = "")
        {
            IPagedList<BZReplyModel> pagedList = null;
            var query = Request.CreateQueryExpression<BZReplyModel, ReplyRequest>();
            var userRepository = _unitOfWork.GetRepository<BZUserModel>();
            var topicRepository = _unitOfWork.GetRepository<BZTopicModel>();


            IList<BZUserModel> Users = new List<BZUserModel>();
            IList<BZTopicModel> Topics = new List<BZTopicModel>();
            Expression<Func<BZTopicModel, bool>> where = p => true;
            if (!string.IsNullOrWhiteSpace(topicTitle))
            {
                where = where.And(p => p.Title.Contains(topicTitle));
                Topics = await topicRepository.GetAllAsync(where);
                if (Topics != null && Topics.Any())
                    query = query.And(p => Topics.Select(x => x.Id).Contains(p.TopicId));
                else
                    return new NoContentResponse();
            }
            if (!string.IsNullOrWhiteSpace(userName))
            {
                Users = await userRepository.GetAllAsync(p => p.UserName.Contains(userName));
                if (Users.Any())
                    query = query.And(p => Users.Select(x => x.Id).Contains(p.UserId));
                else
                    return new NoContentResponse();
            }
            pagedList = await _replyRepository.GetPagedListAsync(query, o => o.OrderBy(p => p.Id), null, Request.pageInfo.PageIndex - 1, Request.pageInfo.PageSize);
            if (pagedList.TotalCount > 0)
            {
                var pagedatas = pagedList.ConvertToPageData<BZReplyModel, BZReplyDtoWithUser>();
                var users = await userRepository.GetAllAsync(p => pagedList.Items.Select(d => d.UserId).Contains(p.Id));
                var topics = await topicRepository.GetAllAsync(p => pagedList.Items.Select(p => p.TopicId).Contains(p.Id));
                foreach (var replyDto in pagedList.Items)
                {
                    var user = users.FirstOrDefault(p => p.Id == replyDto.UserId);
                    var topic = topics.FirstOrDefault(p => p.Id == replyDto.TopicId);
                    var replywithuser = _mapper.Map<BZReplyDtoWithUser>(replyDto);
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
                pagedatas.Items = pagedatas.Items.OrderBy(p => p.PublishTime).ToList();
                return Ok(pagedatas);
            }
            else
            {
                return new NoContentResponse();
            }
        }
    }
}