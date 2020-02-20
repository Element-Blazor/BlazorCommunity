using Arch.EntityFrameworkCore.UnitOfWork;
using Blazui.Community.Model.Models;
using System;
using System.Threading.Tasks;

namespace Blazui.Community.Repository
{
    public class BZFollowRepository : Repository<BZFollowModel>, IRepository<BZFollowModel>
    {
        public BZFollowRepository(BlazUICommunityContext dbContext) : base(dbContext)
        {


        }

        /// <summary>
        /// 个人中心取消收藏主贴
        /// </summary>
        /// <param name="TopicId"></param>
        /// <param name="UserId"></param>
        /// <returns></returns>
        public async Task<bool> Cancel(int TopicId,int UserId)
        {
            string sql = $"update follow set `Status`=-1 where TopicId={TopicId} and UserId={UserId}";

            return await ExecuteSqlCmdAsync(sql);
        }
        /// <summary>
        /// 主贴界面切换收藏
        /// </summary>
        /// <param name="followId"></param>
        /// <param name="status"></param>
        /// <returns></returns>
        public async Task<bool> ToggleFollow(int followId,int status)
        {
            string sql = $"update follow set `Status`={status} where Id={followId}";

            return await ExecuteSqlCmdAsync(sql);
        }
        
    }
}
