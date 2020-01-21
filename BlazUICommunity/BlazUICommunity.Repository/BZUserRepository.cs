using Arch.EntityFrameworkCore.UnitOfWork;
using BlazUICommunity.DTO;
using BlazUICommunity.Model.Models;
using BlazUICommunity.Utility;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Caching.Memory;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Threading.Tasks;

namespace BlazUICommunity.Repository
{
    public class BZUserRepository : Repository<BZUserModel>, IRepository<BZUserModel>
    {
        public BZUserRepository(BlazUICommunityContext dbContext) : base(dbContext)
        {

        }
        [Inject]
        private IMemoryCache memoryCache { get; set; }

        public (bool success, string message) ChangePwd(string Account , string OldPwd , string NewPwd)
        {
            if ( string.IsNullOrWhiteSpace(Account) )
            {
                throw new ArgumentException("message" , nameof(Account));
            }

            if ( string.IsNullOrWhiteSpace(OldPwd) )
            {
                throw new ArgumentException("message" , nameof(OldPwd));
            }

            if ( string.IsNullOrWhiteSpace(NewPwd) )
            {
                throw new ArgumentException("message" , nameof(NewPwd));
            }

            var checkUser = CheckAndGetUser(Account , OldPwd , out BZUserModel user);
            if ( checkUser )
            {
                user.Cypher = MD5Encrypt.Encrypt(NewPwd);
                Commit();
                return (true, "修改成功");
            }
            else
            {
                return (false, "用户名或旧密码错误");
            }
        }


        private BZUserModel GetUserFromCache(string Account)
        {
            var users = memoryCache.Get<List<BZUserModel>>(nameof(BZUserModel));
            if ( users != null )
            {
                return users.Single(p => p.Account == Account);
            }
            return null;
        }

        /// <summary>
        /// 检查用户是否存在并且返回用户
        /// </summary>
        /// <param name="Account"></param>
        /// <param name="Password"></param>
        /// <returns>BZUserModel</returns>
        private bool CheckAndGetUser(string Account , string Password , out BZUserModel bZUser)
        {
            if ( string.IsNullOrWhiteSpace(Account) )
            {
                throw new ArgumentException("message" , nameof(Account));
            }

            if ( string.IsNullOrWhiteSpace(Password) )
            {
                throw new ArgumentException("message" , nameof(Password));
            }

            bZUser = GetUserFromCache(Account);
            return bZUser == null? false:MD5Encrypt.Encrypt(Password) == bZUser.Cypher;
         
        }

        /// <summary>
        /// 登录
        /// </summary>
        /// <param name="Account"></param>
        /// <param name="Pwd"></param>
        /// <returns></returns>
        public (bool success, string message, BZUserModel user) Login(string Account , string Pwd)
        {
            var checkUser = CheckAndGetUser(Account , Pwd , out BZUserModel user);

            return checkUser ? (true, "登录成功", user) : (false, "登录失败", null);

        }

        /// <summary>
        /// 查询指定时间用户发表的主题帖数量
        /// </summary>
        /// <param name="start"></param>
        /// <param name="end"></param>
        /// <returns></returns>
        private async Task<IEnumerable<UserActiveDto>> UserTopActives(DateTime start , DateTime end)
        {
            if ( start.ToString("yyyy-MM-dd") == end.ToString("yyyy-MM-dd") )
                end = end.AddDays(1);
            string exeSql = $"select  t.*,u.NickName as `Name` from (select `UserId`,count(`Id`) as count from `topic` where PublishTime>=@start and PublishTime<=@end GROUP BY `UserId`) t left JOIN `user` u on t.UserId=u.Id order BY t.count desc limit 0,20;";
            DbParameter sqlTopicIdParameter = new MySqlParameter("start" , start);
            DbParameter sqlReplyIdParameter = new MySqlParameter("end" , end);
            var topActive = await QueryDataFromSql<UserActiveDto>(exeSql , sqlTopicIdParameter , sqlReplyIdParameter);
            return topActive;
        }

        /// <summary>
        /// 查询指定时间用户发表的回帖数量
        /// </summary>
        /// <param name="start"></param>
        /// <param name="end"></param>
        /// <returns></returns>
        private async Task<IEnumerable<UserActiveDto>> UserReplyActives(DateTime start , DateTime end)
        {
            if ( start.ToString("yyyy-MM-dd") == end.ToString("yyyy-MM-dd") )
                end = end.AddDays(1);
            string exeSql = $"select  t.*,u.NickName as `Name` from (select `UserId`,count(`Id`) as count from `reply` where PublishTime>=@start and PublishTime<=@end GROUP BY `UserId`) t left JOIN `user` u on t.UserId=u.Id order BY t.count desc limit 0,20;";
            DbParameter sqlTopicIdParameter = new MySqlParameter("start" , start);
            DbParameter sqlReplyIdParameter = new MySqlParameter("end" , end);
            var replyActive = await QueryDataFromSql<UserActiveDto>(exeSql , sqlTopicIdParameter , sqlReplyIdParameter);
            return replyActive;
        }

        /// <summary>
        /// 统计用户活跃度
        /// </summary>
        /// <param name="start"></param>
        /// <param name="end"></param>
        /// <returns></returns>
        public async Task<IEnumerable<UserActiveDto>> UserActive(DateTime start , DateTime end)
        {
            var tops = await UserTopActives(start , end);
            var replys = await UserReplyActives(start , end);
            tops = tops.Select(t => new UserActiveDto() { Count = t.Count * 2 , UserId = t.UserId , Name = t.Name });
            var actives = tops.Union(replys);
            var group = actives.GroupBy(p => p.UserId);
            IEnumerable<UserActiveDto> dtos = new List<UserActiveDto>();
            dtos = group.Select(p => new UserActiveDto() { Count = p.Sum(c => c.Count) , Name = p.FirstOrDefault()?.Name , UserId = p.Key });
            //foreach ( var item in group )
            //{
            //    dtos.Add(new UserActiveDto() { UserId = item.Key , Name = item.FirstOrDefault()?.Name , Count = item.Sum(d => d.Count) });
            //}
            return dtos;
        }
    }
}
