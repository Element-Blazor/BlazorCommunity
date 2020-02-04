using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Blazui.Community.Utility.Extensions
{
    public static class PropertyInfoExtensions
    {
        private static PropertyInfo[] Properties;


        /// <summary>
        /// 判断对象是否所有字段都为空
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="t"></param>
        /// <returns></returns>
        public static bool IsAllPropertyNull<T>(this T t)
        {
            if (t == null)
                return true;
            if (Properties == null)
                Properties = typeof(T).GetProperties();
            return !Properties.Any(p => p.GetValue(t) != null);
        }

        /// <summary>
        /// 找出需要更新的属性
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="tOld"></param>
        /// <param name="tNew"></param>
        /// <returns></returns>
        public static PropertyInfo[] GetShouldUpdateProperties<T>(this T tNew, T tOld)
        {
            if (Properties == null)
                Properties = typeof(T).GetProperties();
            if (tNew == null)
                return null;
            if (tOld == null)
                return Properties;
            return Properties.Where(p => p.GetValue(tNew) != null && p.GetValue(tNew) != p.GetValue(tOld)).ToArray();
        }

        /// <summary>
        /// 将
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="t"></param>
        /// <returns></returns>
        public static Dictionary<string, object> ObjectToDictionary<T>(this T t)
        {
            if (t == null)
                throw new NullReferenceException(" t is null");
            if (Properties == null)
                Properties = typeof(T).GetProperties();
            Dictionary<string, object> keyValues = new Dictionary<string, object>();
            foreach (var property in Properties)
            {
                keyValues.Add(property.Name, property.GetValue(t));
            }
            return keyValues;

        }
        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="t"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        public static Dictionary<string, object> ObjectToDictionary<T>(this T t, string key)
        {
            if (t == null)
                throw new NullReferenceException(" t is null");
            Dictionary<string, object> keyValues = new Dictionary<string, object>
            {
                { key, t }
            };
            return keyValues;

        }
    }
}
