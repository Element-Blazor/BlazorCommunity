using Arch.EntityFrameworkCore.UnitOfWork;
using Blazui.Community.Model.Models;
using MySql.Data.MySqlClient;
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
        public async Task<bool> Cancel(string TopicId, string UserId)
        {
            string sql = $"update bzfollow set `Status`=-1 where TopicId=@TopicId and UserId=@UserId";
            MySqlParameter[] paras = new MySqlParameter[]
                  {
                      new MySqlParameter("@TopicId",TopicId),new MySqlParameter("@UserId",UserId)
                  };

            return await ExecuteSqlCmdAsync(sql, paras);
        }
        /// <summary>
        /// 主贴界面切换收藏
        /// </summary>
        /// <param name="followId"></param>
        /// <param name="status"></param>
        /// <returns></returns>
        public async Task<bool> ToggleFollow(string followId,int status)
        {
            string sql = $"update bzfollow set `Status`=@Status where Id=@followId";
            MySqlParameter[] paras = new MySqlParameter[]
                 {
                      new MySqlParameter("@Status",status),new MySqlParameter("@followId",followId)
                 };
            return await ExecuteSqlCmdAsync(sql, paras);
        }
        
    }
}
