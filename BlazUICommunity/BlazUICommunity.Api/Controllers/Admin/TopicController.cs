using Arch.EntityFrameworkCore.UnitOfWork;
using Arch.EntityFrameworkCore.UnitOfWork.Collections;
using AutoMapper;
using Blazui.Community.Api.Service;
using Blazui.Community.DTO.Admin;
using Blazui.Community.LinqExtensions;
using Blazui.Community.Model.Models;
using Blazui.Community.Repository;
using Blazui.Community.Request;
using Blazui.Community.SwaggerExtensions;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Blazui.Community.Api.Controllers
{
    /// <summary>
    /// 主题帖相关
    /// </summary>
    [HiddenApi]
    [Route("api/[Controller]")]
    [ApiController]
    [SwaggerTag(description: "主贴相关")]
    //[HttpCacheExpiration(MaxAge = 100)]
    public class TopicController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly BZTopicRepository _bZTopicRepository;
        private readonly IRepository<BZFollowModel> _bZFollowRepository;
        private readonly IRepository<BZReplyModel> _bZReplyRepository;
        private readonly ICacheService _cacheService;
        private readonly RoleManager<IdentityRole<string>> roleManager;

        /// <summary>
        ///
        /// </summary>
        /// <param name="unitOfWork"></param>
        /// <param name="mapper"></param>
        /// <param name="bZTopicRepository"></param>
        /// <param name="cacheService"></param>
        public TopicController(IUnitOfWork unitOfWork,
            IMapper mapper, BZTopicRepository bZTopicRepository, ICacheService cacheService, RoleManager<IdentityRole<string>> roleManager)
        {
            _mapper = mapper;
            _unitOfWork = unitOfWork;
            _bZTopicRepository = bZTopicRepository;
            _bZFollowRepository = unitOfWork.GetRepository<BZFollowModel>();
            _bZReplyRepository = unitOfWork.GetRepository<BZReplyModel>();
            _cacheService = cacheService;
            this.roleManager = roleManager;
        }

        /// <summary>
        /// 置顶或取消置顶
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        [HttpPatch("Top/{Id}")]
        public IActionResult TopTopic([FromRoute] string Id)
        {
            var topic = _bZTopicRepository.Find(Id);
            if (topic != null)
            {
                topic.Top = topic.Top == 1 ? 0 : 1;

                topic.LastModifyDate = DateTime.Now;
                topic.LastModifierId = Guid.Empty.ToString();
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
        [HttpPatch("Best/{Id}")]
        public IActionResult BestTopic([FromRoute] string Id)
        {
            var topic = _bZTopicRepository.Find(Id);
            if (topic != null)
            {
                topic.Good = topic.Good == 1 ? 0 : 1;
                topic.LastModifyDate = DateTime.Now;
                topic.LastModifierId = Guid.Empty.ToString();
                _bZTopicRepository.Update(topic);
                _cacheService.Remove(nameof(BZTopicModel));
            }
            return Ok();
        }

        /// <summary>
        /// 设置帖子查看权限
        /// </summary>
        /// <param name="TopicId"></param>
        /// <param name="RoleId"></param>
        /// <returns></returns>
        [HttpPatch("authorize/{TopicId}/{RoleId}")]
        public IActionResult AuthorizeTopic([FromRoute] string TopicId, [FromRoute] string RoleId)
        {
            var topic = _bZTopicRepository.Find(TopicId);
            if (RoleId == "-")
                RoleId = "";
            if (topic != null)
            {
                topic.LastModifyDate = DateTime.Now;
                topic.LastModifierId = Guid.Empty.ToString();
                topic.RoleId = RoleId;
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
        [HttpPatch("End/{Id}")]
        public IActionResult EndTopic([FromRoute] string Id)
        {
            var topic = _bZTopicRepository.Find(Id);
            if (topic != null)
            {
                topic.Status = topic.Status == 1 ? 0 : 1;
                topic.LastModifyDate = DateTime.Now;
                topic.LastModifierId = Guid.Empty.ToString();
                _bZTopicRepository.Update(topic);
                _cacheService.Remove(nameof(BZTopicModel));
            }
            return Ok();
        }

        /// <summary>
        /// 根据ID删除帖子
        /// </summary>
        /// <returns></returns>
        [HttpPatch("Delete/{Id}")]
        public IActionResult Delete([FromRoute] string Id)
        {
            return DeleteOrResume(Id, -1);
        }

        /// <summary>
        /// 恢复帖子
        /// </summary>
        /// <returns></returns>
        [HttpPatch("Resume/{Id}")]
        public IActionResult Active([FromRoute] string Id)
        {
            return DeleteOrResume(Id, 0);
        }

        private IActionResult DeleteOrResume(string Id, int status)
        {
            var topic = _bZTopicRepository.Find(Id);
            if (topic is null)
                return BadRequest();
            return Ok(_unitOfWork.CommitWithTransaction
                (() =>
                {
                    if (topic != null)
                    {
                        topic.Status = status;
                        topic.LastModifyDate = DateTime.Now;
                        topic.LastModifierId = Guid.Empty.ToString();
                        var follows = _cacheService.GetFollowsAsync(p => p.TopicId == Id).Result.ToList();
                        follows.ForEach(p =>
                        {
                            p.Status = status;
                            p.LastModifyDate = DateTime.Now;
                            p.LastModifierId = Guid.Empty.ToString();
                        });
                        _bZFollowRepository.Update(follows);
                        _bZTopicRepository.Update(topic);
                        var replys = _cacheService.GetReplysAsync(p => p.TopicId == Id).Result.ToList();
                        replys.ForEach(p => p.Status = status);
                        _bZReplyRepository.Update(replys);
                    }
                }
                ));
        }

        /// <summary>
        /// 根据条件分页查询帖
        /// </summary>
        /// <returns></returns>
        [HttpGet("Query")]
        public async Task<ActionResult<IPagedList<TopicDisplayDto>>> Query([FromQuery]TopicRequestCondition Request = null)
        {
            IPagedList<BZTopicModel> pagedList = null;
            var query = Request.CreateQueryExpression<BZTopicModel, TopicRequestCondition>();
            if (!string.IsNullOrWhiteSpace(Request.UserName))
            {
                var Users = await _cacheService.GetUsersAsync(p =>
                p.UserName.Contains(Request.UserName) ||
                p.NickName.Contains(Request.UserName));
                if (Users != null)
                    query = query.And(p => Users.Select(x => x.Id).Contains(p.CreatorId));
                else return NoContent();
            }
            pagedList = await _bZTopicRepository.GetPagedListAsync(query, o => o.OrderBy(p => p.Id), null, Request.PageIndex - 1, Request.PageSize);
            if (pagedList.Items.Any())
            {
                var pagedatas = pagedList.From(result => _mapper.Map<List<TopicDisplayDto>>(result));
                var users = await _cacheService.GetUsersAsync(p => pagedList.Items.Select(d => d.CreatorId).Contains(p.Id));

                foreach (TopicDisplayDto topic in pagedatas.Items)
                {
                    topic.UserName = users.FirstOrDefault(p => p.Id == topic.CreatorId)?.UserName;
                    if(!string.IsNullOrWhiteSpace(topic.RoleId))
                    topic.RoleName = roleManager.Roles.FirstOrDefault(p=>p.Id==topic.RoleId)?.Name;
                }
                return Ok(pagedatas);
            }
            return NoContent();
        }
    }
}