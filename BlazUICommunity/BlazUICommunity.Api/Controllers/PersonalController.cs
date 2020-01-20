using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Security.Cryptography;
using System.Threading.Tasks;
using Arch.EntityFrameworkCore.UnitOfWork;
using AutoMapper;
using BlazUICommunity.Model.Models;
using BlazUICommunity.Utility.Extensions;
using BlazUICommunity.Utility.Filter;
using BlazUICommunity.Utility.Response;
using log4net.Repository.Hierarchy;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Swashbuckle.AspNetCore.Annotations;

namespace BlazUICommunity.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [SwaggerTag("个人中心")]
    public class PersonalController : ControllerBase
    {
        private readonly ILogger<TestController> _logger;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public PersonalController(IUnitOfWork unitOfWork , ILogger<TestController> logger , IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;

        }

        /// <summary>
        /// 我的主贴
        /// </summary>
        /// <param name="userId">用户ID</param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="title"></param>
        /// <returns></returns>
        [HttpGet("Topic/{userId}/{pageSize}/{pageIndex}/{title}")]
        public async Task<IActionResult> QueryTopic(int userId ,
            [SwaggerParameter(Required = false)] int pageSize = 20 ,
            [SwaggerParameter(Required = false)]int pageIndex = 1,
            [SwaggerParameter(Required = false)]string title = "")
        {
            if ( userId < 1 )
                return new BadRequestResponse(" user id  error");

            var repo = _unitOfWork.GetRepository<BZTopicModel>(true);
            Expression<Func<BZTopicModel , bool>> predicate = p => p.UserId == userId;
            if ( !string.IsNullOrWhiteSpace(title) )
                predicate.And(p => p.Title == title);
            var listData = await repo.GetPagedListAsync(predicate , o => o.OrderByDescending(p => p.PublishTime) , null , pageIndex - 1 , pageSize);
            if ( listData.TotalCount > 0 )
            {
                var resultData = _mapper.Map<List<BZTopicDto>>(listData?.Items);
                return Ok(resultData);
            }
            else
            {
                
                return new NoContentResponse();
            }
        }

        /// <summary>
        /// 我的回帖
        /// </summary>
        /// <param name="userId">用户ID</param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        [HttpGet("Reply/{userId}/{pageSize}/{pageIndex}")]
        public async Task<IActionResult> QueryReply(int userId ,
            [SwaggerParameter(Required = false)] int pageSize = 20 ,
            [SwaggerParameter(Required = false)]int pageIndex = 1 )
        {
            if ( userId < 1 )
                return new BadRequestResponse(" user id  error");

            var repo = _unitOfWork.GetRepository<BZReplyModel>(true);
        
            var listData = await repo.GetPagedListAsync(p => p.UserId == userId , o => o.OrderByDescending(p => p.PublishTime) , null , pageIndex - 1 , pageSize);
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
        /// 我的关注
        /// </summary>
        /// <param name="userId">用户ID</param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        [HttpGet("Follow/{userId}/{pageSize}/{pageIndex}")]
        public async Task<IActionResult> QueryFollow(int userId ,
            [SwaggerParameter(Required = false)] int pageSize = 20 ,
            [SwaggerParameter(Required = false)]int pageIndex = 1)
        {
            if ( userId < 1 )
                return new BadRequestResponse(" user id  error");

            var repo = _unitOfWork.GetRepository<BZFollowModel>(true);

            var listData = await repo.GetPagedListAsync(p => p.UserId == userId , o => o.OrderByDescending(p => p.FollowTime) , null , pageIndex - 1 , pageSize);
            if ( listData.TotalCount > 0 )
            {
                var resultData = _mapper.Map<List<BZFollowDto>>(listData?.Items);
                return Ok(resultData);
            }
            else
            {
                return new NoContentResponse();
            }
        }

    }
}