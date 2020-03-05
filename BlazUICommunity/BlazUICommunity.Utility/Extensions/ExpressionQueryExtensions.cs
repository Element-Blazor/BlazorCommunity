//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Linq.Expressions;
//using System.Reflection;

//namespace Blazui.Community.Utility.Extensions
//{
//    public static class ExpressionQueryExtensions
//    {
//        private static readonly Dictionary<Type , PropertyInfo[]> souceProperties = new Dictionary<Type , PropertyInfo[]>();
//        private static readonly Dictionary<Type , PropertyInfo[]> searchProperties = new Dictionary<Type , PropertyInfo[]>();

//        public static Expression<Func<TSource , bool>> CreateQueryExpression<TSource, TSearch>(this TSearch searchRequestEntity) where TSearch : class
//        {
//            if ( searchRequestEntity is null )
//                return null;

//            GetPropertyInfos<TSource , TSearch>(out KeyValuePair<Type , PropertyInfo[]> dataSource , out KeyValuePair<Type , PropertyInfo[]> dataSearch);

//            var pe = Expression.Parameter(dataSource.Key , "p");
//            var expression = Expression.Equal(Expression.Constant(true) , Expression.Constant(true));
//            foreach ( var property in dataSearch.Value )
//            {
//                expression = ComposeExpression(searchRequestEntity , dataSource , pe , expression , property);
//            }
//            return Expression.Lambda<Func<TSource , bool>>(expression , pe);
//        }

//        /// <summary>
//        /// 组合
//        /// </summary>
//        /// <typeparam name="TSearch"></typeparam>
//        /// <param name="searchRequestEntity"></param>
//        /// <param name="dataSource"></param>
//        /// <param name="pe"></param>
//        /// <param name="expression"></param>
//        /// <param name="property"></param>
//        /// <returns></returns>
//        private static BinaryExpression ComposeExpression<TSearch>(TSearch searchRequestEntity , KeyValuePair<Type , PropertyInfo[]> dataSource , ParameterExpression pe , BinaryExpression expression , PropertyInfo property) where TSearch : class
//        {
//            var propertySearchAttribute = property.GetCustomAttribute<ExpressionQueryAttribute>(true);
//            if ( string.IsNullOrWhiteSpace(propertySearchAttribute.ColumnName) )
//                propertySearchAttribute.ColumnName = property.Name;
//            var propertySearchVlaue = property.GetValue(searchRequestEntity , null);
//            var propertySearchAttributeName = propertySearchAttribute.ColumnName;


//            if ( dataSource.Value.Any(p => p.Name == propertySearchAttributeName)
//                && propertySearchVlaue != null
//                && propertySearchVlaue != ( object ) string.Empty
//                 //&& propertySearchAttribute.Operation != OperationType.None
//                 )
//            {
//                var propertyReference = Expression.Property(pe , propertySearchAttributeName);
//                var sourcePropertyType = dataSource.Value.FirstOrDefault(p => p.Name == propertySearchAttributeName).PropertyType;
//                ConstantExpression constantReference = null;
//                Expression Expr = null;

//                bool isGenericType = sourcePropertyType.IsGenericType && sourcePropertyType.GetGenericTypeDefinition() == typeof(Nullable<>);
//                if ( isGenericType )
//                    constantReference = Expression.Constant(Convert.ChangeType(propertySearchVlaue , Nullable.GetUnderlyingType(sourcePropertyType)) , sourcePropertyType);
//                else
//                    constantReference = Expression.Constant(Convert.ChangeType(propertySearchVlaue , sourcePropertyType));

//                switch ( propertySearchAttribute.Operation )
//                {
//                    case OperationType.Equal:
//                        Expr = Expression.Equal(propertyReference , constantReference);
//                        break;
//                    case OperationType.GreaterThan:
//                        Expr = Expression.GreaterThan(propertyReference , constantReference);
//                        break;
//                    case OperationType.LessThan:
//                        Expr = Expression.LessThan(propertyReference , constantReference);
//                        break;
//                    case OperationType.GreaterThanOrEqual:
//                        Expr = Expression.GreaterThanOrEqual(propertyReference , constantReference);
//                        break;
//                    case OperationType.LessThanOrEqual:
//                        Expr = Expression.LessThanOrEqual(propertyReference , constantReference);
//                        break;
//                    case OperationType.Like:
//                        Expr = Expression.Call(propertyReference , typeof(String).GetMethod("Contains" , new Type[] { typeof(string) }) , constantReference);
//                        break;
//                    default: break;

//                }

//                expression = Expression.AndAlso(expression , Expr);
//            }

//            return expression;
//        }

//        private static void GetPropertyInfos<TSource, TSearch>(out KeyValuePair<Type , PropertyInfo[]> dataSource , 
//            out KeyValuePair<Type , PropertyInfo[]> dataSearch) where TSearch : class
//        {
//            var dataSouceType = typeof(TSource);
//            var dataSearchType = typeof(TSearch);

//            dataSource = souceProperties.TryGetValue(dataSouceType , out PropertyInfo[] sourceProps) ?
//               new KeyValuePair<Type , PropertyInfo[]>(dataSouceType , sourceProps) :
//            new KeyValuePair<Type , PropertyInfo[]>(dataSouceType , dataSouceType.GetProperties());
//            dataSearch = searchProperties.TryGetValue(dataSearchType , out PropertyInfo[] searchProps) ?
//                new KeyValuePair<Type , PropertyInfo[]>(dataSearchType , searchProps) :
//                new KeyValuePair<Type , PropertyInfo[]>(dataSearchType , dataSearchType.GetProperties()
//                .Where(p => p.IsDefined(typeof(ExpressionQueryAttribute) , true)).ToArray());
//        }
//    }
//    /// <summary>
//    /// 查询操作类型
//    /// </summary>
//    public enum OperationType
//    {
//        /// <summary>
//        /// 不进行查询
//        /// </summary>
//        None,
//        /// <summary>
//        /// 比较该查询属性的值是否与元数据数据的值相等 即sql中=
//        /// </summary>
//        Equal,
//        /// <summary>
//        /// 比较元数据数据的值是否包含该查询属性的值  即sql中like
//        /// </summary>
//        Like,
//        /// <summary>
//        /// 大于
//        /// </summary>
//        GreaterThan,
//        /// <summary>
//        /// 小于
//        /// </summary>
//        LessThan,
//        /// <summary>
//        /// >=
//        /// </summary>
//        GreaterThanOrEqual,
//        /// <summary>
//        /// <=
//        /// </summary>
//        LessThanOrEqual
//    }

//    /// <summary>
//    /// 
//    /// </summary>
//    public class ExpressionQueryAttribute : Attribute
//    {
//        public string ColumnName { get; set; }
//        public OperationType Operation { get; set; }

//        public ExpressionQueryAttribute(OperationType operation , string displayName = null)
//        {
//            ColumnName = displayName;
//            Operation = operation;
//        }


//    }
//}
