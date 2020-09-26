using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Blazui.Community.Common
{
    public class RegexHelper
    {

        private static readonly string CheckChinaPattern = @"[\u4e00-\u9fa5]";//检查汉字的正则表达式
        private static readonly string CheckEmailPattern = "^[a-z0-9]+([._\\-]*[a-z0-9])*@([a-z0-9]+[-a-z0-9]*[a-z0-9]+.){1,63}[a-z0-9]+$";
        public static bool IsEmail( string email)
        {
            return Regex.Match(email.ToLower(), CheckEmailPattern).Success;
        }

        public static bool ContainsChineseCharacters(string input)
        {
            return Regex.Matches(input, CheckChinaPattern)?.Count > 0;
        }
    }
}
