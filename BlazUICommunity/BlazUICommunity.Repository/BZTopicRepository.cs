using Arch.EntityFrameworkCore.UnitOfWork;
using Blazui.Community.DTO;
using Blazui.Community.Model.Models;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Data.SqlClient;
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
        public async Task<int> PageIndexOfReply(int topicId , int replyId , int pageSize = 20)
        {
            string exeSql = $"select CAST(tt.rn AS CHAR   ) AS rn from (select t.topicid,t.id,row_number() over (order by id) rn  from `reply` t where t.topicid=@topicId) tt    where tt.id =@replyId";
            DbParameter sqlTopicIdParameter = new MySqlParameter("topicId" , topicId);
            DbParameter sqlReplyIdParameter = new MySqlParameter("replyId" , replyId);
            var result = await ExecuteScalarAsync<string>(exeSql , sqlTopicIdParameter , sqlReplyIdParameter);
            if( result  is null )
            {
                throw new InvalidCastException($"result is null");
            }
            if ( int.TryParse(result , out int rowNumber) )
            {
                var Remainder = rowNumber % pageSize;
                var row= Remainder == 0? +rowNumber / pageSize: (rowNumber- Remainder + pageSize ) / pageSize;
                return row;
            }
            else throw new InvalidCastException($"{result} 转换成int 失败");
        }


    }
}
