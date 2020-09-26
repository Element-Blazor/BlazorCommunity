using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Reflection;

namespace Blazui.Community.UnitOfWork.Collections
{
    public static class EntityMapNameExtensions
    {
        private static readonly ConcurrentDictionary<string, string> EntityTableNames = new ConcurrentDictionary<string, string>();

        public static string GetTableName<Entity>(this Type entity) where Entity : class
        {
            var type = typeof(Entity);
            if (EntityTableNames.TryGetValue(type.FullName, out string TableName))
                return TableName;
            else
            {
                var tableName = type.IsDefined(typeof(TableAttribute), true) ? type.GetCustomAttribute<TableAttribute>().Name : type.Name;
                EntityTableNames.TryAdd(type.FullName, tableName);
                return tableName;
            }
        }
    }
}