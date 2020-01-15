using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Threading.Tasks;
using Arch.EntityFrameworkCore.UnitOfWork;
using BlazUICommunity.Model.Models;
using BlazUICommunity.Utility.Filter;
using log4net.Repository.Hierarchy;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace BlazUICommunity.Api.Controllers
{
    [HiddenApi]
    [Route("[controller]")]
    [ApiController]
    public class FirstController : ControllerBase
    {
        private readonly ILogger<FirstController> _logger;
        private readonly IUnitOfWork _unitOfWork;

        public FirstController(IUnitOfWork unitOfWork , ILogger<FirstController> logger)
        {
            _unitOfWork = unitOfWork;
        
       }

        [HttpGet]
        public IActionResult Get([FromServices] IUnitOfWork unitOfWork,[FromServices] ILogger<FirstController> logger)
        {
            logger.LogDebug("测试日志");
            //communityContext.Database.EnsureCreated();
            //BZUserModel bZUserModel = new BZUserModel()
            //{
            //    Account = "abcdefg123" ,
            //    Avatar = "http://www.baidu.com/image/ssa" ,
            //    Cypher = "31312321",
            //    Email = "212718@qq.com" ,
            //    LastLoginAddr = "2131" ,
            //    LastLoginDate = DateTime.Now ,
            //    LastLoginType = 0 ,
            //    Level = 1 ,
            //    Mobile = "12131321321312" ,
            //    NickName = "vicky" ,
            //    Points = 100 ,
            //    RegisterDate = DateTime.Now ,
            //    Sex = 0 ,
            //    Signature = " stay hangur stay foolish" ,
            //    Status = 0
            //};
            var userRepository = unitOfWork.GetRepository<BZUserModel>(true);
            //var logRepository = _unitOfWork.GetRepository<SysLog>(true);
            //_unitOfWork.CommitWithTransaction(() =>
            //{
            //    userRepository.Insert(bZUserModel);
            //    logRepository.Insert(new SysLog() { CreateTime = DateTime.Now , Detail = "异常，，，，" , CreatorId = 1 , UserName = "321333333333333", Introduction="13131", LastModifierId=1, LastModifyTime=DateTime.Now, LogType=1 });
            //});
            //_unitOfWork.SaveChanges();
            var user = userRepository.GetFirstOrDefault(p=>p.Id==1);
            //using       ( BlazUICommunityContext  content=new BlazUICommunityContext() )
            //{
            //    content.Add(bZUserModel);
            //    content.SaveChanges();
            //}
            return Ok(user);
        } 
    }
}