using Arch.EntityFrameworkCore.UnitOfWork;
using Blazui.Community.DTO;
using Blazui.Community.Model.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Blazui.Community.Repository
{
    public class BZReplyRepository : Repository<BZReplyModel>, IRepository<BZReplyModel>
    {
        public BZReplyRepository(BlazUICommunityContext dbContext) : base(dbContext)
        {

        }

        public async Task<IEnumerable<ReplyDto>> QueryMyReplys(int userId, int pageSize, int pageIndex)
        {
            string sql = @$"select  t1.*,t2.Title,t3.UserName,t3.NickName from reply t1 left join topic t2 on t1.TopicId=t2.Id
                                LEFT JOIN aspnetusers t3 on t2.UserId=t3.Id
                                 where t1.`Status`=0 and t2.`Status`=0 and t1.UserId={userId} limit {pageIndex},{pageSize}";

            return await QueryDataFromSql<ReplyDto>(sql);
        }

        /// <summary>
        /// 软删除
        /// </summary>
        /// <param name="replyId"></param>
        /// <returns></returns>
        public async Task<bool> FakeDelete(int replyId)
        {
            return await DeleteOrActive(replyId, -1);
        }
        /// <summary>
        /// 删除或激活
        /// </summary>
        /// <param name="replyId"></param>
        /// <returns></returns>
        public async Task<bool> DeleteOrActive(int replyId, int status)
        {
            string sql = $" update Reply set status={status} where id={replyId}; ";

            return await ExecuteSqlCmd(sql);
        }

    }
}
