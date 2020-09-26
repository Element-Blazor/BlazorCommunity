using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Blazored.LocalStorage;
using Blazui.Community.WasmApp.Model;
using Blazui.Community.WasmApp.Model.Cache;

namespace Blazui.Community.WasmApp.Service
{
    public class LocalStorageCacheService : ILocalStorageCacheService
    {
        private ILocalStorageService LocalStorageService { get; }

        public LocalStorageCacheService(ILocalStorageService localStorageService)
        {
            LocalStorageService = localStorageService;
        }
        public async Task<T> CreateOrGetCache<T>(string key, Func<Task<T>> func) where T :LocalStorgeCacheBase
        {
            var data = await LocalStorageService.GetItemAsync<T>(key);
            if (data != null && data.Expire >= DateTime.Now)
            {
                return data;
            }
            else
            {
                var result = await func();
                if(result.Expire.HasValue)
                await LocalStorageService.SetItemAsync<T>(key, result);
                return result;
            }
        }

        public  Task<T> GetItemAsync<T>(string key)
        {
            return LocalStorageService.GetItemAsync<T>(key);
        }

        public Task ClearAsync()
        {
            return LocalStorageService.ClearAsync();
        }

        public Task RemoveItemAsync(string key)
        {
            return LocalStorageService.RemoveItemAsync(key);
        }

      
    }
}
