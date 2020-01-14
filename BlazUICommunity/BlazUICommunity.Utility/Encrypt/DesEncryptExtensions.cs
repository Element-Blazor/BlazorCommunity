using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlazUICommunity.Utility
{
    /// <summary>
    /// 
    /// </summary>
    public static class DesEncryptExtensions
    {
        public static string DesKey = "";
        private static readonly string defaultValue = "deathvicky";
        public static IApplicationBuilder ConfigureDesEncrypt(this IApplicationBuilder builder , Func<string> Key = null)
        {
            var v = Key?.Invoke();
            DesKey = string.IsNullOrEmpty(v) ? defaultValue : ( string ) Convert.ChangeType(v , typeof(string));

            return builder;
        }
    }
}
