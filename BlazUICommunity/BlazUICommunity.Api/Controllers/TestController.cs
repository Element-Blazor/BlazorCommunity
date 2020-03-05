﻿using System;
using Arch.EntityFrameworkCore.UnitOfWork;
using Blazui.Community.Repository;
using Blazui.Community.Utility.Filter;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Blazui.Community.Api.Controllers
{
    [HiddenApi]
    [Route("[controller]")]
    [ApiController]
    //[SwaggerTag(description: "测试")]
    public class TestController : ControllerBase
    {
        private readonly ILogger<TestController> _logger;
        private readonly IUnitOfWork _unitOfWork;
        private readonly BZTopicRepository _bZTopicRepository;
        public TestController(IUnitOfWork unitOfWork , ILogger<TestController> logger, BZTopicRepository bZTopicRepository)
        {
            _unitOfWork = unitOfWork;
            _bZTopicRepository = bZTopicRepository;
       }

        [Authorize]
        [HttpGet("Get1")]
        [ResponseCache(VaryByHeader = "Accept-Encoding", Duration = 10)]
        public IActionResult Get()
        {
            return Ok(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
        }

        [HttpGet]
        public IActionResult Get(string repId,[FromServices] ILogger<TestController> logger)
        {
            logger.LogDebug("测试日志");
            //var repo = _unitOfWork.GetRepository<BZTopicModel>(true);
            return Ok(_bZTopicRepository.PageIndexOfReply(Guid.NewGuid().ToString() , repId , 20).Result);
            //communityContext.Database.EnsureCreated();
            //BZUserModel bZUserModel = new BZUserModel()
            //{
            //    Account = "abcdefg123"+new Random().Next(0 , 1000) ,
            //    Avatar = "http://www.baidu.com/image/ssa" ,
            //    Cypher = "31312321" ,
            //    Email = "212718@qq.com" ,
            //    LastLoginAddr = "2131" ,
            //    LastLoginDate = DateTime.Now ,
            //    LastLoginType = 0 ,
            //    Level = 1 ,
            //    Mobile = "12132" ,
            //    NickName = "vicky" ,
            //    Points = 100 ,
            //    RegisterDate = DateTime.Now ,
            //    Sex = 0 ,
            //    Signature = " stay hangur stay foolish" ,
            //    Status = 0
            //};
            //var userRepository = unitOfWork.GetRepository<BZUserModel>(true);
            ////var logRepository = _unitOfWork.GetRepository<SysLog>(true);
            //_unitOfWork.CommitWithTransaction(() =>
            //{
            //    //userRepository.Insert(bZUserModel);
            //    //logRepository.Insert(new SysLog() { CreateTime = DateTime.Now , Detail = "异常，，，，" , CreatorId = 1 , UserName = "321333333333333" , Introduction = "13131" , LastModifierId = 1 , LastModifyTime = DateTime.Now , LogType = 1 });

            //    userRepository.Insert(bZUserModel);
              
            //});
            ////_unitOfWork.SaveChanges();

            //var user = userRepository.GetFirstOrDefault(p=>p.Id==1);
            ////using       ( BlazUICommunityContext  content=new BlazUICommunityContext() )
            ////{
            ////    content.Add(bZUserModel);
            ////    content.SaveChanges();
            ////}
            return Ok();
        } 
    }
}