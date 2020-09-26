using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Primitives;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BlazorCommunity.App.Service
{
    public class BrowerService
    {
        private readonly IHttpContextAccessor httpContextAccessor;

        public BrowerService(IHttpContextAccessor httpContextAccessor)
        {
            this.httpContextAccessor = httpContextAccessor;
        }
        public bool IsMobile()
        {
            if (httpContextAccessor.HttpContext.Request.Headers.TryGetValue("User-Agent", 
                out StringValues agent))
            {
                var UserAgent = agent.ToString().ToLower();
                return UserAgent.Contains("android") ||
                    UserAgent.Contains("iphone") ||
                    UserAgent.Contains("ipad");
            }
            return false;
        }
    }
}
