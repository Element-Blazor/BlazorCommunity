using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Reflection;
using System.Text;

namespace Blazui.Community.Utility.Extensions
{
    public static class EnumExtensions
    {

        /// <summary>
        /// 获取枚举标记的第一个Description或继承Description的特性描述
        /// </summary>
        /// <param name="e"></param>
        /// <returns></returns>
        public static string Description(this Enum e)
        {
            var attribute = e?.GetType().GetField(e?.ToString()).GetCustomAttribute<DescriptionAttribute>(true);

            return (attribute == null || attribute.Description == null) ? e.ToString() : attribute.Description;
        }

        public static Dictionary<int, string> KeyDescriptions(this Type t)
        {
            if (!t.IsEnum)
                throw new NotSupportedException("只对枚举类型有效");
            Dictionary<int, string> keyValuePairs = new Dictionary<int, string>();
            foreach (Enum v in Enum.GetValues(t))
            {
                keyValuePairs.Add(Convert.ToInt32(v), v.Description());
            }
            return keyValuePairs;
        }
        /// <summary>
        /// 获取枚举
        /// </summary>
        /// <param name="t"></param>
        /// <returns></returns>
        public static Dictionary<int, string> KeyNames(this Type t)
        {
            if (!t.IsEnum)
                throw new NotSupportedException("只对枚举类型有效");
            Dictionary<int, string> keyValuePairs = new Dictionary<int, string>();
            foreach (Enum v in Enum.GetValues(t))
            {
                keyValuePairs.Add(Convert.ToInt32(v), v.ToString());
            }
            return keyValuePairs;
        }
    }
}
