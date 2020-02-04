using Blazui.Community.Model.Models;
using System;

namespace Blazui.Community.Test
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");

            //Guid s1 = Guid.Parse("0edf5111-204c-404f-99fa-aeaf592884e7");
            using ( BlazUICommunityContext communityContext =new BlazUICommunityContext() )
            {
                communityContext.Database.EnsureCreated();
                //BZUserModel bZUserModel = new BZUserModel() { 
                // Account="abcdefg123", Avatar="http://www.baidu.com/image/ssa", Cypher="321312213",
                //  Email="212718@qq.com",  LastLoginAddr="2131", LastLoginDate=DateTime.Now, LastLoginType=0,
                //   Level=1,Mobile ="12131321321312", NickName="vicky", Points=100,  RegisterDate=DateTime.Now, Sex=0, Signature=" stay hangur stay foolish", Status=0
                //};

                var s = communityContext.Find<BZUserModel>(1);
                //communityContext.SaveChanges();
                //UnitOfWork<BlazUICommunityContext> UnitOfWork = new UnitOfWork<BlazUICommunityContext>(communityContext);

                //var userRepository = UnitOfWork.GetRepository<BZUserModel>(true);
                //userRepository.Insert(bZUserModel);
                //var user=     userRepository.GetFirstOrDefault();

            }
            Console.ReadLine();
        }
    }
}
