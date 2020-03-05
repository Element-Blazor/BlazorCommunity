﻿using Arch.EntityFrameworkCore.UnitOfWork;
using Blazui.Community.Model.Models;
using Microsoft.Extensions.Caching.Memory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Blazui.Community.Api.Service
{
    public class CacheService : ICacheService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMemoryCache _memoryCache;
        public CacheService(IMemoryCache memoryCache, IUnitOfWork unitOfWork)
        {
            _memoryCache = memoryCache;
            _unitOfWork = unitOfWork;
        }

        public async Task<IList<BZAddressModel>> Address(Expression<Func<BZAddressModel, bool>> condition)
        {
            return await GetOrCreateAsync(nameof(BZAddressModel), TimeSpan.FromSeconds(60), condition);

        }

        public  async Task<IList<BzBannerModel>> Banners(Expression<Func<BzBannerModel, bool>> condition)
        {
            return await GetOrCreateAsync(nameof(BzBannerModel), TimeSpan.FromSeconds(60), condition);
        }

        public async Task<IList<BZFollowModel>> Follows(Expression<Func<BZFollowModel, bool>> condition)
        {
            return await GetOrCreateAsync(nameof(BZFollowModel), TimeSpan.FromSeconds(30), condition);
        }

     

        public async Task<IList<BZReplyModel>> Replys(Expression<Func<BZReplyModel, bool>> condition)
        {
            return await GetOrCreateAsync(nameof(BZReplyModel), TimeSpan.FromSeconds(30), condition);
        }

        public async Task<IList<BZTopicModel>> Topics(Expression<Func<BZTopicModel, bool>> condition)
        {
            return await GetOrCreateAsync(nameof(BZTopicModel), TimeSpan.FromSeconds(30), condition);
        }

        public async  Task<IList<BZUserModel>> Users(Expression<Func<BZUserModel, bool>> condition)
        {
            return await GetOrCreateAsync(nameof(BZUserModel), TimeSpan.FromSeconds(30), condition);
        }
        public async Task<IList<BZVersionModel>> Versions(Expression<Func<BZVersionModel, bool>> condition)
        {
            return await GetOrCreateAsync(nameof(BZVersionModel), TimeSpan.FromSeconds(60 * 5), condition);
        }
        private async Task<IList<T>> GetOrCreateAsync<T>(string key, TimeSpan Expiration ,Expression <Func<T, bool>> condition=null) where T :class
        {
            condition ??= p => true;
            var Repo = _unitOfWork.GetRepository<T>();
            var datas = await _memoryCache.GetOrCreateAsync(key, async p =>
            {
                p.SetSlidingExpiration(Expiration);
                return await Repo.GetAllAsync();
            });
            if (datas.Any())
                return datas.AsQueryable().Where(condition).ToList();
            else
                return new List<T>();
        }

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="Key"></param>
        public void Remove(string Key)
        {
            if(_memoryCache.TryGetValue(Key,out object _))
            _memoryCache.Remove(Key);
        }

     
    }
}
