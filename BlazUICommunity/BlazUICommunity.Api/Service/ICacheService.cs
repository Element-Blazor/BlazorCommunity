using Blazui.Community.Model.Models;
using System;
using System.Collections.Generic;
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
        Task<IList<BZUserModel>> Users(Expression<Func<BZUserModel, bool>> condition = null);

        /// <summary>
        /// 主贴缓存数据
        /// </summary>
        /// <param name="condition"></param>
        /// <returns></returns>
        Task<IList<BZTopicModel>> Topics(Expression<Func<BZTopicModel, bool>> condition = null);

        /// <summary>
        /// Banner缓存数据
        /// </summary>
        /// <param name="condition"></param>
        /// <returns></returns>
        Task<IList<BzBannerModel>> Banners(Expression<Func<BzBannerModel, bool>> condition = null);

        /// <summary>
        /// 回帖缓存数据
        /// </summary>
        /// <param name="condition"></param>
        /// <returns></returns>
        Task<IList<BZReplyModel>> Replys(Expression<Func<BZReplyModel, bool>> condition = null);

        /// <summary>
        /// 地址信息缓存数据
        /// </summary>
        /// <param name="condition"></param>
        /// <returns></returns>
        Task<IList<BZAddressModel>> Address(Expression<Func<BZAddressModel, bool>> condition = null);

        /// <summary>
        /// 收藏帖子
        /// </summary>
        /// <param name="condition"></param>
        /// <returns></returns>
        Task<IList<BZFollowModel>> Follows(Expression<Func<BZFollowModel, bool>> condition = null);

        /// <summary>
        /// 项目版本
        /// </summary>
        /// <param name="condition"></param>
        /// <returns></returns>
        Task<IList<BZVersionModel>> Versions(Expression<Func<BZVersionModel, bool>> condition = null);

        /// <summary>
        /// 移除缓存
        /// </summary>
        /// <param name="Key"></param>
        void Remove(string Key);
    }
}