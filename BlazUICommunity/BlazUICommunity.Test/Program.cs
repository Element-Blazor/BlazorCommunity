using Blazui.Community.Model.Models;
using System;
using System.Collections.Generic;
using System.Linq;

using System.Threading;

namespace Blazui.Community.Test
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World! Start");



            //using (BlazUICommunityContext communityContext = new BlazUICommunityContext())
            //{
            //    communityContext.Database.EnsureDeleted();
            //    communityContext.Database.EnsureCreated();
            //}
            //////string id = Guid.NewGuid().ToString();
            //using (BlazUICommunityContext communityContext = new BlazUICommunityContext())
            //{
            //    //communityContext.BZUser.Add(new BZUserModel() { CreateDate = DateTime.Now, Avator = "", CreatorId = "", Email = "", EmailConfirmed = false, LastLoginAddr = "", LastLoginDate = DateTime.Now, Level = 0, NickName = "", PhoneNumber = "1321321313", Sex = 0, UserName = "admin", Status = 0, Signature = "", Points = 0 });
            //    //communityContext.BZTopic.Add(new BZTopicModel() { Good = 0, CreateDate = DateTime.Now, Title = "", Content = "1312321", CreatorId = Guid.NewGuid().ToString(), LastModifyDate = DateTime.Now, Status = 0, });
            //    //

            //    var set = communityContext.Set<BZTopicModel>();
            //    set.AddAsync(new BZTopicModel() { Good = 0, CreateDate = DateTime.Now, Title = "", Content = "1312321", CreatorId = Guid.NewGuid().ToString(), LastModifyDate = DateTime.Now, Status = 0, }, default);


            //    communityContext.SaveChanges();
            //}
            Console.WriteLine("Hello World! End");
            Console.ReadLine();
        }
    }
}
