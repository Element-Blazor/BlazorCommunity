using Arch.EntityFrameworkCore.UnitOfWork;
using Blazui.Community.DTO;
using Blazui.Community.Model.Models;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Threading.Tasks;

namespace Blazui.Community.Repository
{
    public class BZUserRepository : Repository<BZUserModel>, IRepository<BZUserModel>
    {
        public BZUserRepository(BlazUICommunityContext dbContext) : base(dbContext)
        {
        }

        /// <summary>
        /// 查询指定时间用户发表的主题帖数量
        /// </summary>
        /// <param name="start"></param>
        /// <param name="end"></param>
        /// <returns></returns>
        private async Task<IEnumerable<UserActiveDto>> UserTopActives(DateTime start, DateTime end)
        {
            if (start.ToString("yyyy-MM-dd") == end.ToString("yyyy-MM-dd"))
                end = end.AddDays(1);
            string exeSql = $"select  t.*,u.NickName as `Name` from (select `UserId`,count(`Id`) as count from `bztopic` where CreateDate>=@start and CreateDate<=@end GROUP BY `UserId`) t left JOIN `bzuser` u on t.UserId=u.Id order BY t.count desc limit 0,20;";
            DbParameter sqlTopicIdParameter = new MySqlParameter("start", start);
            DbParameter sqlReplyIdParameter = new MySqlParameter("end", end);
            var topActive = await QueryDataFromSql<UserActiveDto>(exeSql, sqlTopicIdParameter, sqlReplyIdParameter);
            return topActive;
        }

        /// <summary>
        /// 查询指定时间用户发表的回帖数量
        /// </summary>
        /// <param name="start"></param>
        /// <param name="end"></param>
        /// <returns></returns>
        private async Task<IEnumerable<UserActiveDto>> UserReplyActives(DateTime start, DateTime end)
        {
            if (start.ToString("yyyy-MM-dd") == end.ToString("yyyy-MM-dd"))
                end = end.AddDays(1);
            string exeSql = $"select  t.*,u.NickName as `Name` from (select `UserId`,count(`Id`) as count from `bzreply` where CreateDate>=@start and CreateDate<=@end GROUP BY `UserId`) t left JOIN `bzuser` u on t.UserId=u.Id order BY t.count desc limit 0,20;";
            DbParameter sqlTopicIdParameter = new MySqlParameter("start", start);
            DbParameter sqlReplyIdParameter = new MySqlParameter("end", end);
            var replyActive = await QueryDataFromSql<UserActiveDto>(exeSql, sqlTopicIdParameter, sqlReplyIdParameter);
            return replyActive;
        }

        /// <summary>
        /// 统计用户活跃度
        /// </summary>
        /// <param name="start"></param>
        /// <param name="end"></param>
        /// <returns></returns>
        public async Task<IEnumerable<UserActiveDto>> UserActive(DateTime start, DateTime end)
        {
            var tops = await UserTopActives(start, end);
            var replys = await UserReplyActives(start, end);
            tops = tops.Select(t => new UserActiveDto() { Count = t.Count * 2, UserId = t.UserId, Name = t.Name });
            var actives = tops.Union(replys);
            var group = actives.GroupBy(p => p.UserId);
            IEnumerable<UserActiveDto> dtos = new List<UserActiveDto>();
            dtos = group.Select(p => new UserActiveDto() { Count = p.Sum(c => c.Count), Name = p.FirstOrDefault()?.Name, UserId = p.Key });
       
            return dtos;
        }



        public async Task<IEnumerable<HotUserDto>> QueryHotUsers(int MonStart,int MonEnd)
        {
            string sql = $"select t1.id,t1.username,t1.nickname,t1.lastlogindate,t2.ctopic as TopicCount,t3.creply as ReplyCount from bzuser t1 left join (select CreatorId,count(id) as ctopic from bztopic GROUP BY CreatorId)  t2 on t1.Id=t2.CreatorId left join (select CreatorId,count(id) as creply from bzreply GROUP BY CreatorId)  t3 on t1.Id=t3.CreatorId where   DATE_FORMAT(lastlogindate,'%Y%m') BETWEEN '{MonStart}' and '{MonEnd}' limit 0,10 ";

           return await QueryDataFromSql<HotUserDto>(sql);
        }


        public async Task<HotUserDto> QueryTopicUser(string UserId)
        {
            string sql = $"select t1.id,t1.username,t1.nickname,t1.lastlogindate,t2.ctopic as TopicCount,t3.creply as ReplyCount from bzuser t1 left join (select CreatorId,count(id) as ctopic from bztopic GROUP BY CreatorId)  t2 on t1.Id=t2.CreatorId left join (select CreatorId,count(id) as creply from bzreply GROUP BY CreatorId)  t3 on t1.Id=t3.CreatorId where   t1.id='{UserId}'";

            return (await QueryDataFromSql<HotUserDto>(sql))?.FirstOrDefault();
        }
    }
}