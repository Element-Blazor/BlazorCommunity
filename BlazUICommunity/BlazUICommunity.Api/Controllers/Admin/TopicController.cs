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
    /// 主题帖相关
    /// </summary>
    [HiddenApi]
    [Route("api/[Controller]")]
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
        /// 根据ID删除帖子
        /// </summary>
        /// <returns></returns>
        [HttpDelete("Delete/{Id}")]
        public IActionResult Delete([FromRoute] string Id)
        {
            return Ok(DeleteOrActiveTopic(Id, -1));
        }

        /// <summary>
        /// 恢复帖子
        /// </summary>
        /// <returns></returns>
        [HttpDelete("Active/{Id}")]
        public IActionResult Active([FromRoute] string Id)
        {
            return Ok(DeleteOrActiveTopic(Id, 0));
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
        /// 根据条件分页查询帖
        /// </summary>
        /// <returns></returns>
        [HttpGet("Query")]
        public async Task<IActionResult> Query([FromQuery]TopicRequestCondition Request = null)
        {
            IPagedList<BZTopicModel> pagedList = null;
            var query = Request.CreateQueryExpression<BZTopicModel, TopicRequestCondition>();
            if (!string.IsNullOrWhiteSpace(Request.UserName))
            {
                var Users = await _cacheService.Users(p => 
                p.UserName.ToLower().Contains(Request.UserName.ToLower()) ||
                p.NickName.ToLower().Contains(Request.UserName.ToLower()));
                if (Users != null)
                    query = query.And(p => Users.Select(x => x.Id).Contains(p.CreatorId));
                else return NoContent();
            }
            pagedList = await _bZTopicRepository.GetPagedListAsync(query, o => o.OrderBy(p => p.Id), null, Request.PageIndex - 1, Request.PageSize);
            if (pagedList.Items.Any())
            {
                var pagedatas = pagedList.From(result => _mapper.Map<List<BZTopicDto>>(result));
                var users = await _cacheService.Users(p => pagedList.Items.Select(d => d.CreatorId).Contains(p.Id));
                foreach (BZTopicDto topic in pagedatas.Items)
                {
                    var user = users.FirstOrDefault(p => p.Id == topic.CreatorId);
                    topic.UserName = user.UserName;
                    topic.Avator = user.Avator;
                    topic.NickName = user.NickName;
                }
                return Ok(pagedatas);
            }
            return NoContent();
        }

    }
}