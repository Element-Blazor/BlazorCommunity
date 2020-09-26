using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Blazored.LocalStorage;
using BlazorCommunity.WasmApp.Model;
using BlazorCommunity.WasmApp.Model.Cache;

namespace BlazorCommunity.WasmApp.Service
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

        public async Task<T> GetItemAsync<T>(string key)
        {
            return await LocalStorageService.GetItemAsync<T>(key);
        }

        public async Task ClearAsync()
        {
             await  LocalStorageService.ClearAsync();
        }

        public async Task RemoveItemAsync(string key)
        {
             await LocalStorageService. RemoveItemAsync(key);
        }

      
    }
}
