using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Blazui.Community.DictionaryExtensions
{
    public static class PropertyInfoExtensions
    {
        private static PropertyInfo[] Properties;

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