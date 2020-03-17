using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Reflection;
using System.Text;

namespace Blazui.Community.UnitOfWork.Collections
{
    public static class EntityMapNameExtensions
    {
        static readonly Dictionary<string, string> EntityTableNames = new Dictionary<string, string>();

        public static string GetTableName<Entity>(this Type entity) where Entity : class
        {
            var type = typeof(Entity);
            if (EntityTableNames.TryGetValue(type.FullName, out string TableName))
                return TableName;
            else
            {
                var tableName = type.IsDefined(typeof(TableAttribute), true) ? type.GetCustomAttribute<TableAttribute>().Name : type.Name;
                EntityTableNames.Add(type.FullName, tableName);
                return tableName;
            }
        }
    }
}
