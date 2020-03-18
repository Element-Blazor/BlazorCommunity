using Arch.EntityFrameworkCore.UnitOfWork;
using Arch.EntityFrameworkCore.UnitOfWork.Collections;
using AutoMapper;
using Blazui.Community.Api.Service;
using Blazui.Community.DTO;
using Blazui.Community.LinqExtensions;
using Blazui.Community.Model.Models;
using Blazui.Community.Repository;
using Blazui.Community.Request;
using Blazui.Community.Response;
using Blazui.Community.StringExtensions;
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
    /// 主题帖相关
    /// </summary>
    [Route("api/client/[controller]")]
    [ApiController]
    [SwaggerTag(description: "主贴相关")]
    public class TopicController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly BZTopicRepository _bZTopicRepository;
        private readonly IRepository<BZFollowModel> _bZFollowRepository;
        private readonly IRepository<BZReplyModel> _bZReplyRepository;
        private readonly ICacheService _cacheService;

        /// <summary>
        ///
        /// </summary>
        /// <param name="unitOfWork"></param>
        /// <param name="mapper"></param>
        /// <param name="bZTopicRepository"></param>
        /// <param name="cacheService"></param>
        public TopicController(IUnitOfWork unitOfWork,
            IMapper mapper, BZTopicRepository bZTopicRepository, ICacheService cacheService)
        {
            _mapper = mapper;
            _unitOfWork = unitOfWork;
            _bZTopicRepository = bZTopicRepository;
            _bZFollowRepository = unitOfWork.GetRepository<BZFollowModel>();
            _bZReplyRepository = unitOfWork.GetRepository<BZReplyModel>();
            _cacheService = cacheService;
        }

        /// <summary>
        /// 新增主题帖
        /// </summary>
        /// <returns></returns>
        [Authorize]
        [HttpPost("Add")]
        public async Task<IActionResult> Add([FromBody] BZTopicDto dto)
        {
            var topicModel = _mapper.Map<BZTopicModel>(dto);
            var model = await _bZTopicRepository.InsertAsync(topicModel);
            _cacheService.Remove(nameof(BZTopicModel));
            return Ok(model.Entity.Id);
        }

        /// <summary>
        /// 置顶或取消置顶
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        [HttpGet("TopTopic/{Id}")]
        public IActionResult TopTopic([FromRoute] string Id)
        {
            var topic = _bZTopicRepository.Find(Id);
            if (topic != null)
            {
                topic.Top = topic.Top == 1 ? 0 : 1;
                _bZTopicRepository.Update(topic);
                _cacheService.Remove(nameof(BZTopicModel));
            }
            return Ok();
        }

        /// <summary>
        /// 加精或取消加精
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        [Authorize]
        [HttpGet("BestTopic/{Id}")]
        public IActionResult BestTopic([FromRoute] string Id)
        {
            var topic = _bZTopicRepository.Find(Id);
            if (topic != null)
            {
                topic.Good = topic.Good == 1 ? 0 : 1;
                _bZTopicRepository.Update(topic);
                _cacheService.Remove(nameof(BZTopicModel));
            }
            return Ok();
        }

        /// <summary>
        /// 结贴
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        [Authorize]
        [HttpGet("EndTopic/{Id}")]
        public IActionResult EndTopic([FromRoute] string Id)
        {
            var topic = _bZTopicRepository.Find(Id);
            if (topic != null)
            {
                topic.Status = topic.Status == 1 ? 0 : 1;
                _bZTopicRepository.Update(topic);
                _cacheService.Remove(nameof(BZTopicModel));
            }
            return Ok();
        }

        /// <summary>
        /// 根据ID删除帖子--假删除--前后台均调用
        /// </summary>
        /// <returns></returns>
        [Authorize]
        [HttpDelete("Delete/{Id}")]
        public IActionResult Delete([FromRoute] string Id)
        {
            DeleteOrActiveTopic(Id, -1);
            return Ok();
        }   /// <summary>

            /// 恢复帖子
            /// </summary>
            /// <returns></returns>
        [Authorize]
        [HttpDelete("Active/{Id}")]
        public IActionResult Active([FromRoute] string Id)
        {
            DeleteOrActiveTopic(Id, 0);
            return Ok();
        }

        private bool DeleteOrActiveTopic(string Id, int status)
        {
            return _unitOfWork.CommitWithTransaction
                (() =>
                {
                    var topic = _bZTopicRepository.Find(Id);
                    if (topic != null)
                    {
                        topic.Status = status;
                        var follows = _cacheService.Follows(p => p.TopicId == Id).Result.ToList();
                        follows.ForEach(p => p.Status = status);
                        _bZFollowRepository.Update(follows);
                        _bZTopicRepository.Update(topic);
                        var replys = _cacheService.Replys(p => p.TopicId == Id).Result.ToList();
                        replys.ForEach(p => p.Status = status);
                        _bZReplyRepository.Update(replys);
                    }
                }
                );
        }

        /// <summary>
        /// 更新主题帖
        /// </summary>
        /// <returns></returns>
        [Authorize]
        [HttpPatch("UpdateContent")]
        public IActionResult UpdateContent([FromBody] BZTopicDto Dto)
        {
            if (string.IsNullOrWhiteSpace(Dto.Id))
                return new BadRequestResponse("id is error");
            var topic = _bZTopicRepository.Find(Dto.Id);
            if (topic == null)
                return new BadRequestResponse("no this topic");
            topic.Content = Dto.Content;
            topic.Title = Dto.Title;
            topic.LastModifyDate = DateTime.Now;
            _bZTopicRepository.Update(topic);
            _cacheService.Remove(nameof(BZTopicModel));
            return Ok();
        }

        /// <summary>
        /// 更新主题帖
        /// </summary>
        /// <returns></returns>
        [Authorize]
        [HttpPost("Update")]
        public IActionResult Update([FromBody] BZTopicDto Dto)
        {
            if (string.IsNullOrWhiteSpace(Dto.Id))
                return new BadRequestResponse("id is error");
            var topic = _mapper.Map<BZTopicModel>(Dto);
            _bZTopicRepository.Update(topic);
            _cacheService.Remove(nameof(BZTopicModel));
            return Ok();
        }

        /// <summary>
        /// 根据Id查询主贴
        /// </summary>
        /// <returns></returns>
        [HttpGet("Query/{Id}")]
        public async Task<IActionResult> Query([FromRoute] string Id)
        {
            var topics = await _cacheService.Topics(p => p.Id == Id);
            if (topics.Any())
            {
                var topic = topics.FirstOrDefault();
                var topicDto = _mapper.Map<BZTopicDto>(topic);
                var users = await _cacheService.Users(p => p.Id == topic.CreatorId);
                var version = (await _cacheService.Versions(p => p.Id == topic.Id)).FirstOrDefault();
                if (users.Any())
                {
                    topicDto.UserName = users?.FirstOrDefault()?.UserName;
                    topicDto.NickName = users?.FirstOrDefault()?.NickName;
                    topicDto.Avator = users?.FirstOrDefault()?.Avator;
                    topicDto.VerName = version?.VerName;
                }
                return Ok(topicDto);
            }
            else
            {
                var res = await _bZTopicRepository.QueryTopById(Id);
                if (res is null)
                    return NoContent();

                return Ok(res.FirstOrDefault());
            }
        }

        /// <summary>
        /// 根据条件分页查询帖子
        /// </summary>
        /// <returns></returns>
        [HttpGet("Query")]
        public async Task<IActionResult> Query([FromQuery] TopicRequestCondition Request = null)
        {
            IPagedList<BZTopicModel> pagedList = null;
            var query = Request.CreateQueryExpression<BZTopicModel, TopicRequestCondition>();
            query = query.And(p => p.Status == 0);
            pagedList = await _bZTopicRepository.GetPagedListAsync(query, o => o.OrderBy(p => p.Id), null, Request.PageIndex - 1, Request.PageSize);
            if (pagedList != null && pagedList.Items.Any())
            {
                var pagedatas = pagedList.From(res => _mapper.Map<List<PersonalTopicDisplayDto>>(res));

                var users = await _cacheService.Users(p => pagedList.Items.Select(d => d.CreatorId).Contains(p.Id));
                foreach (PersonalTopicDisplayDto topic in pagedatas.Items)
                {
                    var user = users.FirstOrDefault(p => p.Id == topic.CreatorId);
                    if (user != null)
                    {
                        topic.UserName = user?.UserName;
                        topic.Avator = user?.Avator;
                        topic.NickName = user?.NickName;
                    }
                }
                if (pagedatas is null || pagedatas.TotalCount == 0)
                    return NoContent();
                return Ok(pagedatas);
            }

            return NoContent();
        }

        /// <summary>
        /// 首页查询按钮查询
        /// </summary>
        /// <param name="Title"></param>
        /// <param name="PageIndex"></param>
        /// <param name="PageSize"></param>
        /// <returns></returns>
        [HttpGet("SeachByTitle/{Title}/{PageIndex}/{PageSize}")]
        public async Task<IActionResult> SeachTopicByTitle(string Title, int PageIndex, int PageSize)
        {
            IPagedList<BZTopicModel> pagedList = null;
            Expression<Func<BZTopicModel, bool>> expression = p => p.Status == 0;
            if (string.IsNullOrWhiteSpace(Title))
                return NoContent();
            expression = expression.And(p => p.Title.IfContains(Title));
            pagedList = await _bZTopicRepository.GetPagedListAsync(expression, o => o.OrderBy(p => p.Id), null, PageIndex - 1, PageSize);
            if (pagedList.Items.Any())
            {
                return Ok(pagedList.From(res => _mapper.Map<List<SeachTopicDto>>(res)));
            }
            else
                return NoContent();
        }

        /// <summary>
        /// 查询主题帖的回帖
        /// </summary>
        /// <param name="topicId">主题帖Id</param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        [HttpGet("Reply/{topicId}/{pageSize}/{pageIndex}")]
        public async Task<IActionResult> QueryTopicReply(string topicId,
            [SwaggerParameter(Required = false)] int pageSize = 20,
            [SwaggerParameter(Required = false)]int pageIndex = 1)
        {
            if (string.IsNullOrWhiteSpace(topicId))
                return new BadRequestResponse(" topicId id  error");

            var repo = _unitOfWork.GetRepository<BZReplyModel>(true);

            var pagedList = await repo.GetPagedListAsync(p => p.TopicId == topicId && p.Status == 0, o => o.OrderByDescending(p => p.CreateDate), null, pageIndex - 1, pageSize);
            if (pagedList.TotalCount > 0)
            {
                var topic = await _bZTopicRepository.GetFirstOrDefaultAsync(p => p.Id == topicId);
                var pagedatas = pagedList.From(res => _mapper.Map<List<BZReplyDto>>(res));
                var users = await _cacheService.Users(p => pagedList.Items.Select(d => d.CreatorId).Contains(p.Id));
                foreach (var replyDto in pagedatas.Items)
                {
                    var user = users.FirstOrDefault(p => p.Id == replyDto.CreatorId);
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

        /// <summary>
        /// 根据回帖ID查询所属的主题帖，并定位到回帖所在主贴的页码
        /// </summary>
        /// <param name="replyId">回帖ID</param>
        /// <returns></returns>
        [HttpGet("Reply/{replyId}")]
        public async Task<IActionResult> QueryReply(string replyId)
        {
            if (string.IsNullOrWhiteSpace(replyId))
                return new BadRequestResponse(" replyId id  error");

            var topicRepo = _unitOfWork.GetRepository<BZTopicModel>(true);
            var replyRepo = _unitOfWork.GetRepository<BZReplyModel>(true);
            var reply = await replyRepo.FindAsync(replyId);
            if (reply is null)
                return NoContent();
            var topic = await topicRepo.FindAsync(reply.TopicId);
            if (topic is null)
            {
                return NoContent();
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
        /// 根据类型，排序查询---前端
        /// </summary>
        /// <param name="orderType">排序类型 0：综合，1：人气，2：精华，3：已结帖</param>
        /// <param name="topicType">帖子类型 0：提问，1：分享，2：讨论，3：建议，4：公告</param>
        /// <param name="pageSize"></param>
        /// <param name="pageIndex"></param>
        /// <returns></returns>
        [HttpGet("QueryByOrder/{orderType}/{topicType}/{pageSize}/{pageIndex}")]
        public async Task<IActionResult> QueryTopicsByOrder([SwaggerParameter(Required = false)] int orderType = 0,
            [SwaggerParameter(Required = false)] int topicType = -1,
            [SwaggerParameter(Required = false)] int pageSize = 10,
            [SwaggerParameter(Required = false)] int pageIndex = 1)
        {
            if (orderType < 0)
                return new BadRequestResponse(" topicType id  error");
            var topicRepo = _unitOfWork.GetRepository<BZTopicModel>(true);
            IPagedList<BZTopicModel> pagedList = null;
            Expression<Func<BZTopicModel, bool>> predicate = p => p.Status != -1;//-1 已删除
            if (topicType != -1)
            {
                predicate = predicate.And(p => p.Category == topicType);
            }
            switch (orderType)
            {
                case 0:
                    pagedList = await _bZTopicRepository.GetPagedListAsync(predicate, o => o.OrderBy(o => (o.ReplyCount * 1.5 + o.Hot)).ThenByDescending(o => o.LastModifyDate), null, pageIndex - 1, pageSize);
                    break;

                case 1:
                    pagedList = await _bZTopicRepository.GetPagedListAsync(predicate, o => o.OrderBy(o => (o.Hot)).ThenByDescending(o => o.LastModifyDate), null, pageIndex - 1, pageSize);
                    break;

                case 2:
                    predicate = predicate.And(p => p.Good == 1);//精华帖
                    pagedList = await _bZTopicRepository.GetPagedListAsync(predicate, o => o.OrderByDescending(o => (o.Good)).ThenByDescending(o => o.LastModifyDate), null, pageIndex - 1, pageSize);
                    break;

                case 3:
                    Expression<Func<BZTopicModel, bool>> predicateEnd = p => p.Status == 1;//1 已结帖
                    pagedList = await _bZTopicRepository.GetPagedListAsync(predicateEnd, o => o.OrderByDescending(p => p.LastModifyDate), null, pageIndex - 1, pageSize);
                    break;

                default:
                    break;
            }
            if (pagedList is null || pagedList.TotalCount == 0)
                return NoContent();
            var pagedatas = pagedList.From(res => _mapper.Map<List<BZTopicDto>>(res));
            var userRepository = _unitOfWork.GetRepository<BZUserModel>();
            var users = await _cacheService.Users(p => pagedList.Items.Select(d => d.CreatorId).Contains(p.Id));
            foreach (BZTopicDto topic in pagedatas.Items)
            {
                var user = users.FirstOrDefault(p => p.Id == topic.CreatorId);
                if (user != null)
                {
                    topic.UserName = user.UserName;
                    topic.Avator = user.Avator;
                    topic.NickName = user.NickName;
                }
            }
            if (pagedatas is null || pagedatas.TotalCount == 0)
                return NoContent();
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
            var Topics = await _bZTopicRepository.GetPagedListAsync(p => p.Good == 1, o => o.OrderByDescending(o => o.CreateDate), null, 0, pageSize);
            if (Topics is null || Topics.TotalCount == 0)
                return NoContent();
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
            var Topics = await _bZTopicRepository.QueryTops(pageSize);
            if (Topics is null || Topics.Count() == 0)
                return NoContent();
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
            Expression<Func<BZTopicModel, bool>> predicate = p => p.CreateDate >= DateTime.Now.AddDays(beforeDays) && p.CreateDate <= DateTime.Now;
            var Topics = await _bZTopicRepository.GetPagedListAsync(predicate, o => o.OrderByDescending(o => o.Hot).ThenByDescending(o => o.ReplyCount), null, 1, 10);
            if (Topics is null || Topics.TotalCount == 0)
                return NoContent();
            var ResultDtos = _mapper.Map<List<BZTopicDto>>(Topics.Items);
            return Ok(ResultDtos);
        }
    }
}