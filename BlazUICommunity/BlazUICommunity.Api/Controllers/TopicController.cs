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
        private readonly BZTopicRepository _bZTopicRepository;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="unitOfWork"></param>
        /// <param name="mapper"></param>
        /// <param name="bZTopicRepository"></param>
        public TopicController(IUnitOfWork unitOfWork,
            IMapper mapper, BZTopicRepository bZTopicRepository)
        {
            _mapper = mapper;
            _unitOfWork = unitOfWork;
            _bZTopicRepository = bZTopicRepository;
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
            try
            {
                await _topicRepository.InsertAsync(user);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
            return Ok();
        }



        /// <summary>
        /// 根据ID删除帖子
        /// </summary>
        /// <returns></returns>
        [HttpDelete("Delete/{Id}")]
        public IActionResult Delete([FromRoute] int Id)
        {
            try
            {
                //_topicRepository.Delete(Id);
                var topic = _topicRepository.Find(Id);
                topic.Status = -1;
                _topicRepository.Update(topic);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        /// <summary>
        /// 更新主题帖
        /// </summary>
        /// <returns></returns>
        [HttpPut("Update/{Id}")]
        public IActionResult Update([FromBody] BZTopicDto Dto, [FromRoute] int Id)
        {
            if (Id < 1)
                return new BadRequestResponse("id is error");
            var user = _mapper.Map<BZTopicModel>(Dto);
            user.Id = Id;

            try
            {
                _topicRepository.Update(user);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        /// <summary>
        /// 根据Id查询主贴
        /// </summary>
        /// <returns></returns>
        [HttpGet("Query/{Id}")]
        public async Task<IActionResult> Query([FromRoute] int Id)
        {
            //var res = await _topicRepository.FindAsync(Id);
            var res = await _bZTopicRepository.QueryTopById(Id);
            if (res is null)
                return new NoContentResponse();
     
            return Ok(res.FirstOrDefault());
        }
        /// <summary>
        /// 根据条件分页查询帖子
        /// </summary>
        /// <returns></returns>
        [HttpPost("Query")]
        public async Task<IActionResult> Query([FromBody] TopicRequest Request = null)
        {

            IPagedList<BZTopicModel> pagedList = null;
            var query = Request.CreateQueryExpression<BZTopicModel, TopicRequest>();
            query = query.And(p => p.Status == 0);
            pagedList = query == null ? await _topicRepository.GetPagedListAsync(Request.pageInfo.PageIndex - 1, Request.pageInfo.PageSize) :
                       await _topicRepository.GetPagedListAsync(query, o => o.OrderBy(p => p.Id), null, Request.pageInfo.PageIndex - 1, Request.pageInfo.PageSize);
            return Ok(pagedList);
        }

        /// <summary>
        /// 查询主题帖的回帖
        /// </summary>
        /// <param name="topicId">主题帖Id</param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        [HttpGet("Reply/{topicId}/{pageSize}/{pageIndex}")]
        public async Task<IActionResult> QueryTopicReply(int topicId,
            [SwaggerParameter(Required = false)] int pageSize = 20,
            [SwaggerParameter(Required = false)]int pageIndex = 1)
        {
            if (topicId < 1)
                return new BadRequestResponse(" topicId id  error");

            var repo = _unitOfWork.GetRepository<BZReplyModel>(true);

            var pagedList = await repo.GetPagedListAsync(p => p.TopicId == topicId, o => o.OrderBy(p => p.PublishTime), null, pageIndex - 1, pageSize);
            if (pagedList.TotalCount > 0)
            {
                var pagedatas = pagedList.ConvertToPageData<BZReplyModel, BZReplyDtoWithUser>();
                var userRepository = _unitOfWork.GetRepository<BZUserModel>();
                var users = await userRepository.GetAllAsync(p => pagedList.Items.Select(d => d.UserId).Contains(p.Id));
                foreach (var replyDtoWithUser in pagedList.Items)
                {
                    var user = users.FirstOrDefault(p => p.Id == replyDtoWithUser.UserId);
                    var replywithuser = _mapper.Map<BZReplyDtoWithUser>(replyDtoWithUser);
                    pagedatas.Items.Add(replywithuser);
                    if(user !=null)
                    {
                        replywithuser.NickName = user?.NickName;
                        replywithuser.Avator = user?.Avator;
                        replywithuser.UserName = user?.UserName;
                        replywithuser.UserId = user.Id;
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
        /// 根据回帖ID查询所属的主题帖，并定位到回帖所在主贴的页码
        /// </summary>
        /// <param name="replyId">回帖ID</param>
        /// <returns></returns>
        [HttpGet("Reply/{replyId}")]
        public async Task<IActionResult> QueryReply(int replyId)
        {
            if (replyId < 1)
                return new BadRequestResponse(" replyId id  error");

            var topicRepo = _unitOfWork.GetRepository<BZTopicModel>(true);
            var replyRepo = _unitOfWork.GetRepository<BZReplyModel>(true);
            var reply = await replyRepo.FindAsync(replyId);
            if (reply is null)
                return new NoContentResponse("找不到回帖");
            var topic = await topicRepo.FindAsync(reply.TopicId);
            if (topic is null)
            {
                return new NoContentResponse("找不到主题帖");
            }
            else
            {
                var resultData = _mapper.Map<BZTopicDto>(topic);
                var replyPosition = await _bZTopicRepository.PageIndexOfReply(topic.Id, replyId);
                //var replys =await QueryTopicReply(topic.Id , 10 , 1);
                return Ok(new { topic = resultData, position = replyPosition });
            }
        }


        /// <summary>
        /// 根据类型查询帖子
        /// </summary>
        /// <param name="topicType">帖子类型 0：提问，1：分享，2：讨论，3：建议，4：公告</param>
        /// <param name="pageSize"></param>
        /// <param name="pageIndex"></param>
        /// <returns></returns>
        [HttpGet("/QueryTopicsByType{topicType}/{pageSize}/{pageIndex}")]
        public async Task<IActionResult> QueryTopic([SwaggerParameter(Required = false)] int topicType = 0,
            [SwaggerParameter(Required = false)] int pageSize = 10,
            [SwaggerParameter(Required = false)] int pageIndex = 1)
        {
            if (topicType < 0)
                return new BadRequestResponse(" topicType id  error");
            var topicRepo = _unitOfWork.GetRepository<BZTopicModel>(true);
            var Topics = await _topicRepository.GetPagedListAsync(p => p.TopicType == topicType, o => o.OrderBy(o => o.Top == 1).ThenByDescending(o => o.PublishTime), null, pageIndex-1, pageSize);
            if (Topics is null || Topics.TotalCount == 0)
                return new NoContentResponse();
            var ResultDtos = _mapper.Map<List<BZTopicDto>>(Topics.Items);
            return Ok(ResultDtos);
        }

        /// <summary>
        /// 根据类型，排序查询
        /// </summary>
        /// <param name="orderType">排序类型 0：综合，1：人气，2：精华，3：已结帖</param>
        /// <param name="topicType">帖子类型 0：提问，1：分享，2：讨论，3：建议，4：公告</param>
        /// <param name="pageSize"></param>
        /// <param name="pageIndex"></param>
        /// <returns></returns>
        [HttpGet("QueryTopicsByOrder/{orderType}/{topicType}/{pageSize}/{pageIndex}")]
        public async Task<IActionResult> QueryTopicsByOrder([SwaggerParameter(Required = false)] int orderType = 0,
            [SwaggerParameter(Required = false)] int topicType = -1,
            [SwaggerParameter(Required = false)] int pageSize = 10,
            [SwaggerParameter(Required = false)] int pageIndex = 1)
        {
            if (orderType < 0)
                return new BadRequestResponse(" topicType id  error");
            var topicRepo = _unitOfWork.GetRepository<BZTopicModel>(true);
            IPagedList<BZTopicModel> pagedList=null;
            Expression<Func<BZTopicModel, bool>> predicate = p => p.Status!=-1;//-1 已删除
            if (topicType != -1)
            {
                predicate= predicate.And(p => p.TopicType == topicType);
            }
            switch (orderType)
            {
                case 0:
                     pagedList = await _topicRepository.GetPagedListAsync(predicate, o => o.OrderBy(o => (o.ReplyCount*1.5+o.Hot) ).ThenByDescending(o => o.ModifyTime), null, pageIndex - 1, pageSize);
                    break;
                case 1:
                    pagedList = await _topicRepository.GetPagedListAsync(predicate, o => o.OrderBy(o => ( o.Hot)).ThenByDescending(o => o.ModifyTime), null, pageIndex - 1, pageSize);
                    break;
                case 2:
                    predicate = predicate.And(p => p.Good == 1);//精华帖
                    pagedList = await _topicRepository.GetPagedListAsync(predicate, o => o.OrderByDescending(o => (o.Good)).ThenByDescending(o => o.ModifyTime), null, pageIndex - 1, pageSize);
                    break;
                case 3:
                    Expression<Func<BZTopicModel, bool>> predicateEnd = p => p.Status==1;//1 已结帖
                    pagedList = await _topicRepository.GetPagedListAsync(predicateEnd, o=>o.OrderByDescending(p=>p.ModifyTime), null, pageIndex - 1, pageSize);
                    break;
                default:
                    break;
            }
            if (pagedList is null || pagedList.TotalCount == 0)
                return new NoContentResponse();
            var pagedatas = pagedList.ConvertToPageData<BZTopicModel, BZTopicDtoWithUser>();
            var userRepository = _unitOfWork.GetRepository<BZUserModel>();
            var users = await userRepository.GetAllAsync(p => pagedList.Items.Select(d => d.UserId).Contains(p.Id));
            foreach (BZTopicModel topic in pagedList.Items)
            {
                var topicwithuser = _mapper.Map<BZTopicDtoWithUser>(topic);
                var user = users.FirstOrDefault(p => p.Id == topic.UserId);
                topicwithuser.UserName = user.UserName;
                topicwithuser.Avator = user.Avator;
                topicwithuser.NickName = user.NickName;
                pagedatas.Items.Add(topicwithuser);
            }
            if (pagedatas is null || pagedatas.TotalCount == 0)
                return new NoContentResponse();
            return Ok(pagedatas);
        }

        /// <summary>
        /// 查询精华帖子
        /// </summary>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        [HttpGet("Best/{pageSize}")]
        public async Task<IActionResult> QueryBest([SwaggerParameter(Required = false)] int pageSize = 5)
        {
            var topicRepo = _unitOfWork.GetRepository<BZTopicModel>(true);
            var Topics = await _topicRepository.GetPagedListAsync(p => p.Good == 1, o => o.OrderByDescending(o => o.PublishTime),null, 0, pageSize);
            if (Topics is null || Topics.TotalCount == 0)
                return new NoContentResponse();
            var ResultDtos = _mapper.Map<List<BZTopicDto>>(Topics.Items);
            return Ok(ResultDtos);
        }

        /// <summary>
        /// 查询置顶帖子
        /// </summary>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        [HttpGet("Top/{pageSize}")]
        public async Task<IActionResult> QueryTop([SwaggerParameter(Required = false)] int pageSize = 3)
        {
            var topicRepo = _unitOfWork.GetRepository<BZTopicModel>(true);
            //var Topics = await _topicRepository.GetPagedListAsync(p => p.Top == 1, o => o.OrderByDescending(o => o.PublishTime), null, 0, pageSize);

            var Topics = await _bZTopicRepository.QueryTops(pageSize);
            if (Topics is null || Topics.Count() == 0)
                return new NoContentResponse();
            //var ResultDtos = _mapper.Map<List<BZTopicDto>>(Topics);
            return Ok(Topics);
        }


        /// <summary>
        /// 帖子热度
        /// </summary>
        /// <param name="HotType">1：月榜，2：周榜</param>
        /// <returns></returns>
        [HttpGet("Hot/{HotType}")]
        public async Task<IActionResult> TopHot(int HotType = 1)
        {
            int beforeDays = HotType switch
            {
                1 => -30,
                2 => -7,
                _ => -7
            };
            var topicRepo = _unitOfWork.GetRepository<BZTopicModel>(true);
            Expression<Func<BZTopicModel, bool>> predicate = p => p.PublishTime >= DateTime.Now.AddDays(beforeDays) && p.PublishTime <= DateTime.Now;
            var Topics = await _topicRepository.GetPagedListAsync(predicate, o => o.OrderByDescending(o => o.Hot).ThenByDescending(o => o.ReplyCount), null, 1, 10);
            if (Topics is null || Topics.TotalCount == 0)
                return new NoContentResponse();
            var ResultDtos = _mapper.Map<List<BZTopicDto>>(Topics.Items);
            return Ok(ResultDtos);
        }



    }
}