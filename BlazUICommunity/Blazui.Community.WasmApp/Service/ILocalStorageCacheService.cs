using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Blazui.Community.WasmApp.Model.Cache;

namespace Blazui.Community.WasmApp.Service
{
   public interface ILocalStorageCacheService
   {
       public Task<T> CreateOrGetCache<T>(string key, Func<Task<T>> func) where T : LocalStorgeCacheBase;
       public Task<T> GetItemAsync<T>(string key);
       public Task ClearAsync();
       public Task RemoveItemAsync(string key);
   }

  
}
