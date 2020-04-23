using Blazui.Community.DTO;
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
        Task<IList<BZUserModel>> GetUsersAsync(Expression<Func<BZUserModel, bool>> condition = null);

        /// <summary>
        /// 主贴缓存数据
        /// </summary>
        /// <param name="condition"></param>
        /// <returns></returns>
        Task<IList<BZTopicModel>> GetTopicsAsync(Expression<Func<BZTopicModel, bool>> condition = null);

        /// <summary>
        /// Banner缓存数据
        /// </summary>
        /// <param name="condition"></param>
        /// <returns></returns>
        Task<IList<BzBannerModel>> GetBannersAsync(Expression<Func<BzBannerModel, bool>> condition = null);

        /// <summary>
        /// 回帖缓存数据
        /// </summary>
        /// <param name="condition"></param>
        /// <returns></returns>
        Task<IList<BZReplyModel>> GetReplysAsync(Expression<Func<BZReplyModel, bool>> condition = null);

        /// <summary>
        /// 地址信息缓存数据
        /// </summary>
        /// <param name="condition"></param>
        /// <returns></returns>
        Task<IList<BZAddressModel>> GetAddressAsync(Expression<Func<BZAddressModel, bool>> condition = null);

        /// <summary>
        /// 收藏帖子
        /// </summary>
        /// <param name="condition"></param>
        /// <returns></returns>
        Task<IList<BZFollowModel>> GetFollowsAsync(Expression<Func<BZFollowModel, bool>> condition = null);

        /// <summary>
        /// 项目版本
        /// </summary>
        /// <param name="condition"></param>
        /// <returns></returns>
        Task<IList<BZVersionModel>> GetVersionsAsync(Expression<Func<BZVersionModel, bool>> condition = null);
        /// <summary>
        /// 缓存活跃用户
        /// </summary>
        /// <returns></returns>
        Task<IList<HotUserDto>> GetHotUsersAsync();
        /// <summary>
        /// 缓存热点分享主题
        /// </summary>
        /// <returns></returns>
        Task<IList<HotTopicDto>> GetShareHotsAsync();
        /// <summary>
        /// 缓存热点问题主题
        /// </summary>
        /// <returns></returns>
        Task<IList<HotTopicDto>> GetAskHotsAsync();

        /// <summary>
        /// 移除缓存
        /// </summary>
        /// <param name="Key"></param>
        void Remove(string Key);
    }
}