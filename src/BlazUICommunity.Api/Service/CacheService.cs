using Arch.EntityFrameworkCore.UnitOfWork;
using AutoMapper;
using Blazui.Community.DTO;
using Blazui.Community.Model.Models;
using Blazui.Community.Repository;
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
        private readonly IMapper mapper;
        private readonly BZTopicRepository bZTopicRepository;
        private readonly BZUserRepository bZUserRepository;
        private readonly IMemoryCache _memoryCache;

        public CacheService(IMemoryCache memoryCache, IUnitOfWork unitOfWork, IMapper mapper, BZTopicRepository bZTopicRepository,BZUserRepository bZUserRepository)
        {
            _memoryCache = memoryCache;
            _unitOfWork = unitOfWork;
            this.mapper = mapper;
            this.bZTopicRepository = bZTopicRepository;
            this.bZUserRepository = bZUserRepository;
        }

        public async Task<IList<BZAddressModel>> GetAddressAsync(Expression<Func<BZAddressModel, bool>> condition)
        {
            return await GetOrCreateAsync(nameof(BZAddressModel), TimeSpan.FromSeconds(new Random(DateTime.Now.Second).Next(30,100)), condition);
        }

        public async Task<IList<BzBannerModel>> GetBannersAsync(Expression<Func<BzBannerModel, bool>> condition)
        {
            return await GetOrCreateAsync(nameof(BzBannerModel), TimeSpan.FromSeconds(new Random(DateTime.Now.Second).Next(30, 100)), condition);
        }

        public async Task<IList<BZFollowModel>> GetFollowsAsync(Expression<Func<BZFollowModel, bool>> condition)
        {
            return await GetOrCreateAsync(nameof(BZFollowModel), TimeSpan.FromSeconds(new Random(DateTime.Now.Second).Next(30, 100)), condition);
        }

        public async Task<IList<BZReplyModel>> GetReplysAsync(Expression<Func<BZReplyModel, bool>> condition)
        {
            return await GetOrCreateAsync(nameof(BZReplyModel), TimeSpan.FromSeconds(new Random(DateTime.Now.Second).Next(30, 100)), condition);
        }

        public async Task<IList<BZTopicModel>> GetTopicsAsync(Expression<Func<BZTopicModel, bool>> condition)
        {
            return await GetOrCreateAsync(nameof(BZTopicModel), TimeSpan.FromSeconds(new Random(DateTime.Now.Second).Next(30, 100)), condition);
        }

        public async Task<IList<BZUserModel>> GetUsersAsync(Expression<Func<BZUserModel, bool>> condition)
        {
            return await GetOrCreateAsync(nameof(BZUserModel), TimeSpan.FromSeconds(new Random(DateTime.Now.Second).Next(30, 100)), condition);
        }

        public async Task<IList<BZVersionModel>> GetVersionsAsync(Expression<Func<BZVersionModel, bool>> condition)
        {
            return await GetOrCreateAsync(nameof(BZVersionModel), TimeSpan.FromSeconds(new Random(DateTime.Now.Second).Next(30, 100)), condition);
        }

        private async Task<IList<T>> GetOrCreateAsync<T>(string key, TimeSpan Expiration, Expression<Func<T, bool>> condition = null) where T : class
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
            if (_memoryCache.TryGetValue(Key, out object _))
                _memoryCache.Remove(Key);
        }

        public async Task<IList<HotUserDto>> GetHotUsersAsync()
        {
            return await _memoryCache.GetOrCreateAsync("HotUsers", async p =>
            {
                p.SetSlidingExpiration(TimeSpan.FromSeconds(new Random(DateTime.Now.Second).Next(30, 100)));
                var now = DateTime.Now;
                var dateStart = Convert.ToInt32(now.AddMonths(-1).ToString("yyyyMM"));
                var dateEnd = Convert.ToInt32(now.ToString("yyyyMM"));
                var hotusers = await bZUserRepository.QueryHotUsers(dateStart, dateEnd);

                if (hotusers != null)
                {
                    hotusers = hotusers.OrderByDescending(p => p.ReplyCount + p.TopicCount * 2).ThenByDescending(p => p.LastLoginDate);
                }
                else
                {
                    hotusers = new List<HotUserDto>();
                }

                return hotusers.ToList();

            });
        }

        public async Task<IList<HotTopicDto>> GetShareHotsAsync()
        {
            return  await  _memoryCache.GetOrCreateAsync("HotShares", async p =>
            {
                p.SetSlidingExpiration(TimeSpan.FromSeconds(new Random(DateTime.Now.Second).Next(30, 100)));
                var Topics = await bZTopicRepository.QueryHotTopics(1);
                var ResultDtos = mapper.Map<List<HotTopicDto>>(Topics);
                return ResultDtos;

            });
        }

        public async Task<IList<HotTopicDto>> GetAskHotsAsync()
        {
            return await _memoryCache.GetOrCreateAsync("HotAsks", async p =>
            {
                p.SetSlidingExpiration(TimeSpan.FromSeconds(new Random(DateTime.Now.Second).Next(30, 100)));
                var Topics = await bZTopicRepository.QueryHotTopics(0);
                var ResultDtos = mapper.Map<List<HotTopicDto>>(Topics);
                return ResultDtos;

            });
        }

        public async Task<IList<HotTopicDto>> GetTopicsByAuthor(string TopicId)
        {
            return await _memoryCache.GetOrCreateAsync($"GetTopicsByAuthor-{TopicId}", async p =>
            {
                p.SetSlidingExpiration(TimeSpan.FromSeconds(new Random(DateTime.Now.Second).Next(30, 100)));
                var Topics = await bZTopicRepository.GetTopicsByAuthor(TopicId);
                var ResultDtos = mapper.Map<List<HotTopicDto>>(Topics);
                return ResultDtos;

            });
        }
    }
}