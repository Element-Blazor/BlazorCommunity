using System;
using System.Text.RegularExpressions;

namespace Blazui.Community.StringExtensions
{
    public static class StringExtensions
    {
        /// <summary>
        /// 处理空格与忽略大小写
        /// </summary>
        /// <param name="source"></param>
        /// <param name="toCompareString"></param>
        /// <returns></returns>
        public static bool IfContains(this string source, string toCompareString)
        {
            if (string.IsNullOrWhiteSpace(source))
                throw new ArgumentNullException(nameof(source));
            if (string.IsNullOrWhiteSpace(toCompareString))
                return false;
            return source.Contains(Regex.Replace(toCompareString, @"\s", ""), StringComparison.OrdinalIgnoreCase);
        }
    }
}