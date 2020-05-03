using Arch.EntityFrameworkCore.UnitOfWork;
using Arch.EntityFrameworkCore.UnitOfWork.Collections;
using Blazui.Community.AppDbContext;
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
    public class BZTopicRepository : Repository<BZTopicModel>, IRepository<BZTopicModel>
    {
        public BZTopicRepository(BlazUICommunityContext dbContext) : base(dbContext)
        {
        }

        /// <summary>
        /// 查询回帖所在主贴的页码
        /// </summary>
        /// <param name="topicId"></param>
        /// <param name="replyId"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        public async Task<int> PageIndexOfReply(string topicId, string replyId, int pageSize = 20)
        {
            string exeSql = $"select CAST(tt.rn AS CHAR   ) AS rn from (select t.topicid,t.id,row_number() over (order by id) rn  from `reply` t where t.topicid=@topicId) tt    where tt.id =@replyId";
            DbParameter sqlTopicIdParameter = new MySqlParameter("topicId", topicId);
            DbParameter sqlReplyIdParameter = new MySqlParameter("replyId", replyId);
            var result = await ExecuteScalarAsync<string>(exeSql, sqlTopicIdParameter, sqlReplyIdParameter);
            if (result is null)
            {
                throw new InvalidCastException($"result is null");
            }
            if (int.TryParse(result, out int rowNumber))
            {
                var Remainder = rowNumber % pageSize;
                var row = Remainder == 0 ? +rowNumber / pageSize : (rowNumber - Remainder + pageSize) / pageSize;
                return row;
            }
            else throw new InvalidCastException($"{result} 转换成int 失败");
        }

        /// <summary>
        /// 前端查询置顶帖子
        /// </summary>
        /// <param name="topSize"></param>
        /// <returns></returns>
        public async Task<IEnumerable<BZTopicDto>> QueryTops(int topSize)
        {
            string sql = $"select t1.*,t2.UserName,t2.NickName,t2.Avator,t2.Signature from bztopic t1 left join bzuser t2 on t1.CreatorId=t2.id where t1.Top=1 and t1.`Status`=0 order by t1.CreateDate desc limit {topSize}";
            return await QueryDataFromSql<BZTopicDto>(sql);
        }

        /// <summary>
        /// 根据ID查询帖子
        /// </summary>
        /// <param name="topicId"></param>
        /// <returns></returns>
        public async Task<IEnumerable<BZTopicDto>> QueryTopById(string topicId)
        {
            string sql = $"select t1.*,t2.UserName,t2.NickName,t2.Avator,t2.Signature from bztopic t1 left join bzuser t2 on t1.CreatorId=t2.id where t1.Id=@topicId and t1.`Status`=0 ";

            MySqlParameter mySqlParameter = new MySqlParameter("topicId", topicId);

            return await QueryDataFromSql<BZTopicDto>(sql, mySqlParameter);
        }

        public async Task<List<BZTopicModel>> QueryHotTopics(int Category)
        {

            IPagedList<BZTopicModel> PageTopics = await GetPagedListAsync(p => p.Status != -1 && p.Category == Category, o => o.OrderByDescending(o => o.Hot).ThenByDescending(o => o.ReplyCount), null, 0, 10);
            if (PageTopics is null || PageTopics.TotalCount == 0)
                return new List<BZTopicModel>();
            return PageTopics.Items.ToList();
         
        }
    }
}