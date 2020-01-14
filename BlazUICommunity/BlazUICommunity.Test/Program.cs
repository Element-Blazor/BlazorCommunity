using Arch.EntityFrameworkCore.UnitOfWork;
using BlazUICommunity.Model.Models;
using BlazUICommunity.Repository;
using System;
using System.Security.Cryptography;

namespace BlazUICommunity.Test
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");
            using ( BlazUICommunityContext  communityContext=new BlazUICommunityContext() )
            {
                //communityContext.Database.EnsureCreated();
                BZUserModel bZUserModel = new BZUserModel() { 
                 Account="abcdefg123", Avatar="http://www.baidu.com/image/ssa", Cypher=MD5.Create().ToString(),
                  Email="212718@qq.com",  LastLoginAddr="2131", LastLoginDate=DateTime.Now, LastLoginType=0,
                   Level=1,Mobile ="12131321321312", NickName="vicky", Points=100,  RegisterDate=DateTime.Now, Sex=0, Signature=" stay hangur stay foolish", Status=0
                };
                UnitOfWork<BlazUICommunityContext> UnitOfWork = new UnitOfWork<BlazUICommunityContext>(communityContext);

                var userRepository = UnitOfWork.GetRepository<BZUserModel>(true);
                userRepository.Insert(bZUserModel);
                var user=     userRepository.GetFirstOrDefault();
                //var s=   communityContext.Find<SysUser>(1);
            }
            Console.ReadLine();
        }
    }
}
