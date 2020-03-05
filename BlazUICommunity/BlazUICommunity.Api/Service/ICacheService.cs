using Blazui.Community.Model.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Blazui.Community.Api.Service
{
   public interface ICacheService
    {
        /// <summary>
        /// 用户缓存数据
        /// </summary>
        /// <param name="condition"></param>
        /// <returns></returns>
        Task<IList<BZUserModel>> Users(Expression<Func<BZUserModel,bool>> condition);
        /// <summary>
        /// 主贴缓存数据
        /// </summary>
        /// <param name="condition"></param>
        /// <returns></returns>
        Task<IList<BZTopicModel>> Topics(Expression<Func<BZTopicModel, bool>> condition);
        /// <summary>
        /// Banner缓存数据
        /// </summary>
        /// <param name="condition"></param>
        /// <returns></returns>
        Task<IList<BzBannerModel>> Banners(Expression<Func<BzBannerModel, bool>> condition);
        /// <summary>
        /// 回帖缓存数据
        /// </summary>
        /// <param name="condition"></param>
        /// <returns></returns>
        Task<IList<BZReplyModel>> Replys(Expression<Func<BZReplyModel, bool>> condition);
        /// <summary>
        /// 地址信息缓存数据
        /// </summary>
        /// <param name="condition"></param>
        /// <returns></returns>
        Task<IList<BZAddressModel>> Address(Expression<Func<BZAddressModel, bool>> condition);

        /// <summary>
        /// 收藏帖子
        /// </summary>
        /// <param name="condition"></param>
        /// <returns></returns>
        Task<IList<BZFollowModel>> Follows(Expression<Func<BZFollowModel, bool>> condition);

        /// <summary>
        /// 项目版本
        /// </summary>
        /// <param name="condition"></param>
        /// <returns></returns>
        Task<IList<BZVersionModel>> Versions(Expression<Func<BZVersionModel, bool>> condition);
        /// <summary>
        /// 移除缓存
        /// </summary>
        /// <param name="Key"></param>
        void Remove(string Key);
    }
}
