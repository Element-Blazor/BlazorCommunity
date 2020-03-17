using Arch.EntityFrameworkCore.UnitOfWork;
using Blazui.Community.DTO;
using Blazui.Community.Model.Models;
using MySql.Data.MySqlClient;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Blazui.Community.Repository
{
    public class BZReplyRepository : Repository<BZReplyModel>, IRepository<BZReplyModel>
    {
        public BZReplyRepository(BlazUICommunityContext dbContext) : base(dbContext)
        {
        }

        public async Task<IEnumerable<BZReplyDto>> QueryMyReplys(string userId, int pageSize, int pageIndex, string TopicTitle = null)
        {
            var whereTitle = string.Empty;
            if (!string.IsNullOrWhiteSpace(TopicTitle))
                whereTitle = $" and t2.Title like @TopicTitle";
            string sql = @$"select  t1.*,t2.Title,t3.UserName,t3.NickName,t3.avator,t2.CreatorId as UserId from bzreply t1 left join  bztopic t2 on t1.TopicId=t2.Id
                                LEFT JOIN BZuser t3 on t2.CreatorId=t3.Id
                                 where t1.`Status`=0 and t2.`Status`=0 and t1.CreatorId=@userId  {whereTitle}  limit {pageIndex * pageSize},{pageSize}";
            List<MySqlParameter> parameters = new List<MySqlParameter>
            {
                new MySqlParameter("@userId", userId)
            };
            if (!string.IsNullOrWhiteSpace(whereTitle))
                parameters.Add(new MySqlParameter("@TopicTitle", $"%{TopicTitle}%"));
            return await QueryDataFromSql<BZReplyDto>(sql, parameters.ToArray());
        }

        public async Task<long> QueryMyReplysCount(string userId, string TopicTitle = null)
        {
            var whereTitle = string.Empty;
            if (!string.IsNullOrWhiteSpace(TopicTitle))
                whereTitle = $" and t2.Title like @TopicTitle";
            string sql = @$"select  count(t1.id) from bzreply t1 left join  bztopic t2 on t1.TopicId=t2.Id
                                LEFT JOIN BZuser t3 on t2.CreatorId=t3.Id
                                 where t1.`Status`=0 and t2.`Status`=0 and t1.CreatorId=@userId  {whereTitle}";
            List<MySqlParameter> parameters = new List<MySqlParameter>
            {
                new MySqlParameter("@userId", userId)
            };
            if (!string.IsNullOrWhiteSpace(whereTitle))
                parameters.Add(new MySqlParameter("@TopicTitle", $"%{TopicTitle}%"));
            return await ExecuteScalarAsync<long>(sql, parameters.ToArray());
        }
    }
}