using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;

namespace Ruanmou.NetCore2.MVC6.Unitility.Filters
{
    /// <summary>
    /// 自定义的资源Filter
    /// </summary>
    public class CustomResourceFilterAttribute : Attribute, IResourceFilter
    {
        private static readonly ConcurrentDictionary<string, object> _Cache = new ConcurrentDictionary<string, object>();
        private string _cacheKey;

        /// <summary>
        /// 控制器实例化之前
        /// </summary>
        /// <param name="context"></param>
        public void OnResourceExecuting(ResourceExecutingContext context)
        {
            _cacheKey = context.HttpContext.Request.Path.ToString();
            if (_Cache.ContainsKey(_cacheKey))
            {
                var cachedValue = _Cache[_cacheKey] as ViewResult;
                if (cachedValue != null)
                {
                    context.Result = cachedValue;
                }
            }
        }

        /// <summary>
        /// 把请求都处理完的
        /// </summary>
        /// <param name="context"></param>
        public void OnResourceExecuted(ResourceExecutedContext context)
        {
            if (!String.IsNullOrEmpty(_cacheKey) &&
                !_Cache.ContainsKey(_cacheKey))
            {
                var result = context.Result as ViewResult;
                if (result != null)
                {
                    _Cache.TryAdd(_cacheKey, result);
                }
            }
        }
    }
}