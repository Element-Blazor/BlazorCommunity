using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Security.Cryptography;
using System.Threading.Tasks;
using Arch.EntityFrameworkCore.UnitOfWork;
using Arch.EntityFrameworkCore.UnitOfWork.Collections;
using AutoMapper;
using BlazUICommunity.DTO;
using BlazUICommunity.Model.Models;
using BlazUICommunity.Repository;
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
        private readonly BZTopicRepository _bZTopicRepository;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="unitOfWork"></param>
        /// <param name="mapper"></param>
        /// <param name="bZTopicRepository"></param>
        public TopicController(IUnitOfWork unitOfWork ,
            IMapper mapper , BZTopicRepository bZTopicRepository)
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

        /// <summary>
        /// 查询主题帖的回帖
        /// </summary>
        /// <param name="topicId">主题帖Id</param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        [HttpGet("Reply/{topicId}/{pageSize}/{pageIndex}")]
        public async Task<IActionResult> QueryTopicReply(int topicId ,
            [SwaggerParameter(Required = false)] int pageSize = 20 ,
            [SwaggerParameter(Required = false)]int pageIndex = 1)
        {
            if ( topicId < 1 )
                return new BadRequestResponse(" topicId id  error");

            var repo = _unitOfWork.GetRepository<BZReplyModel>(true);

            var listData = await repo.GetPagedListAsync(p => p.TopicId == topicId , o => o.OrderByDescending(p => p.PublishTime) , null , pageIndex - 1 , pageSize);
            if ( listData.TotalCount > 0 )
            {
                var resultData = _mapper.Map<List<BZReplyDto>>(listData?.Items);
                return Ok(resultData);
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
            if ( replyId < 1 )
                return new BadRequestResponse(" replyId id  error");

            var topicRepo = _unitOfWork.GetRepository<BZTopicModel>(true);
            var replyRepo = _unitOfWork.GetRepository<BZReplyModel>(true);
            var reply = await replyRepo.FindAsync(replyId);
            if ( reply is null )
                return new NoContentResponse("找不到回帖");
            var topic = await topicRepo.FindAsync(reply.TopicId);
            if ( topic is null )
            {
                return new NoContentResponse("找不到主题帖");
            }
            else
            {
                var resultData = _mapper.Map<BZTopicDto>(topic);
                var replyPosition = await _bZTopicRepository.PageIndexOfReply(topic.Id , replyId);
                //var replys =await QueryTopicReply(topic.Id , 10 , 1);
                return Ok(new { resultData , replyPosition });
            }
        }


        /// <summary>
        /// 根据类型查询帖子
        /// </summary>
        /// <param name="topicType">帖子类型 0：提问，1：分享，2：讨论，3：建议，4：公告</param>
        /// <param name="pageSize"></param>
        /// <param name="pageIndex"></param>
        /// <returns></returns>
        [HttpGet("{topicType}/{pageSize}/{pageIndex}")]
        public async Task<IActionResult> QueryTopic([SwaggerParameter(Required = false)] int topicType = 0 ,
            [SwaggerParameter(Required = false)] int pageSize = 10 ,
            [SwaggerParameter(Required = false)] int pageIndex = 1)
        {
            if ( topicType < 0 )
                return new BadRequestResponse(" topicType id  error");
            var topicRepo = _unitOfWork.GetRepository<BZTopicModel>(true);
            var Topics = await _topicRepository.GetPagedListAsync(p => p.TopicType == topicType , o => o.OrderBy(o => o.Top == 1).ThenByDescending(o => o.PublishTime) , null , pageIndex , pageSize);
            if ( Topics is null || Topics.TotalCount == 0 )
                return new NoContentResponse();
            var ResultDtos = _mapper.Map<List<BZTopicDto>>(Topics.Items);
            return Ok(ResultDtos);
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
            var Topics = await _topicRepository.GetPagedListAsync(p => p.Good == 1 , o => o.OrderByDescending(o => o.PublishTime) , null , 1 , pageSize);
            if ( Topics is null || Topics.TotalCount == 0 )
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
        public async Task<IActionResult> QueryTop([SwaggerParameter(Required = false)] int pageSize = 5)
        {
            var topicRepo = _unitOfWork.GetRepository<BZTopicModel>(true);
            var Topics = await _topicRepository.GetPagedListAsync(p => p.Top == 1 , o => o.OrderByDescending(o => o.PublishTime) , null , 1 , pageSize);
            if ( Topics is null || Topics.TotalCount == 0 )
                return new NoContentResponse();
            var ResultDtos = _mapper.Map<List<BZTopicDto>>(Topics.Items);
            return Ok(ResultDtos);
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
            Expression<Func<BZTopicModel , bool>> predicate = p => p.PublishTime >= DateTime.Now.AddDays(beforeDays) && p.PublishTime <= DateTime.Now;
            var Topics = await _topicRepository.GetPagedListAsync(predicate , o => o.OrderByDescending(o => o.Hot).ThenByDescending(o => o.ReplyCount) , null , 1 , 10);
            if ( Topics is null || Topics.TotalCount == 0 )
                return new NoContentResponse();
            var ResultDtos = _mapper.Map<List<BZTopicDto>>(Topics.Items);
            return Ok(ResultDtos);
        }



    }
}