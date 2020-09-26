using AutoMapper;
using System;
using System.Reflection;

namespace BlazorCommunity.AutoMapperExtensions
{
    public static class AutoMapperExtensions
    {
        public static void IngoreNotMapped<TSource, TDestination>(this IMappingExpression<TSource, TDestination> mappingExpression)
        {
            if (mappingExpression is null)
            {
                throw new ArgumentNullException(nameof(mappingExpression));
            }

            var destprops = typeof(TDestination).GetProperties();
            foreach (PropertyInfo item in destprops)
            {
                if (item.IsDefined(typeof(AutoNotMapAttribute), true))
                    mappingExpression.ForMember(item.Name, opt => opt.Ignore());
            }
        }
    }
}