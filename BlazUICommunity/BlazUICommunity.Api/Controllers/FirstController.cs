using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Threading.Tasks;
using Arch.EntityFrameworkCore.UnitOfWork;
using BlazUICommunity.Model.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BlazUICommunity.Api.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class FirstController : ControllerBase
    {

        private readonly IUnitOfWork _unitOfWork;

        public FirstController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public IActionResult Get()
        {

            //communityContext.Database.EnsureCreated();
            BZUserModel bZUserModel = new BZUserModel()
            {
                Account = "abcdefg123" ,
                Avatar = "http://www.baidu.com/image/ssa" ,
                Cypher = "31312321",
                Email = "212718@qq.com" ,
                LastLoginAddr = "2131" ,
                LastLoginDate = DateTime.Now ,
                LastLoginType = 0 ,
                Level = 1 ,
                Mobile = "12131321321312" ,
                NickName = "vicky" ,
                Points = 100 ,
                RegisterDate = DateTime.Now ,
                Sex = 0 ,
                Signature = " stay hangur stay foolish" ,
                Status = 0
            };
            var userRepository = _unitOfWork.GetRepository<BZUserModel>(true);
            var logRepository = _unitOfWork.GetRepository<SysLog>(true);
            //_unitOfWork.CommitWithTransaction(() =>
            //{
            //    userRepository.Insert(bZUserModel);
            //    logRepository.Insert(new SysLog() { CreateTime = DateTime.Now , Detail = "异常，，，，" , CreatorId = 1 , UserName = "3213333333333333333333333333333333333321321333333333333333333333333333333333332132133333333333333333333333333333333333213213333333333333333333333333333333333321" });
            //});
            //_unitOfWork.SaveChanges();
               var user = userRepository.GetFirstOrDefault(p=>p.Id==1);
            //using       ( BlazUICommunityContext  content=new BlazUICommunityContext() )
            //{
            //    content.Add(bZUserModel);
            //    content.SaveChanges();
            //}
            return Ok();
        } 
    }
}