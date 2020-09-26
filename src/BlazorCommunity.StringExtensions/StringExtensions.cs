using System;
using System.Text.RegularExpressions;

namespace BlazorCommunity.StringExtensions
{
    public static class StringExtensions
    {

        /// 表达式目录树生成sql解析不了
        ///// <summary>
        ///// 处理空格与忽略大小写
        ///// </summary>
        ///// <param name="source"></param>
        ///// <param name="toCompareString"></param>
        ///// <returns></returns>
        //public static bool Contains(this string source, string toCompareString)
        //{
        //    if (string.IsNullOrWhiteSpace(source))
        //        throw new ArgumentNullException(nameof(source));
        //    if (string.IsNullOrWhiteSpace(toCompareString))
        //        return false;
        //    return source.Contains(Regex.Replace(toCompareString, @"\s", ""), StringComparison.OrdinalIgnoreCase);
        //}
    }
}