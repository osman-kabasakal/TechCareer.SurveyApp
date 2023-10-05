using System.Collections;
using System.Collections.Concurrent;
using System.Linq.Expressions;
using System.Reflection;
using TechCareer.WernerHeisenberg.Survey.Core.QueryCriterias.Attributes;
using TechCareer.WernerHeisenberg.Survey.Core.QueryCriterias.Models;

namespace TechCareer.WernerHeisenberg.Survey.Core.QueryCriterias.Collections;

internal class OrderExpressionCollection<TFrom,TTo>: IEnumerable<Expression>
{
    private readonly QueryCriteria _criteria;
    
    public ParameterExpression OrderedQueryableVariable { get; set; }

    public ParameterExpression QueryParameter { get; set; }

    public OrderExpressionCollection(QueryCriteria criteria)
    {
        _criteria = criteria;
    }

    private MethodInfo GetGenericMethodInfo(PropertyInfo propertyInfo, bool isThen, KeyValuePair<string, ColumnOrder> columnConfig)
    {
        return (isThen
            ? (columnConfig.Value.OrderType == ColumnOrderType.Asc
                ? GetMethodInfo("ThenBy", typeof(IOrderedQueryable<>))
                : GetMethodInfo("ThenByDescending", typeof(IOrderedQueryable<>)))
            : (columnConfig.Value.OrderType == ColumnOrderType.Asc
                ? GetMethodInfo("OrderBy", typeof(IQueryable<>))
                : GetMethodInfo("OrderByDescending", typeof(IQueryable<>)))).MakeGenericMethod(typeof(TTo), propertyInfo.PropertyType);
    }
    
    private static readonly ConcurrentDictionary<string, MethodInfo> QueryableMethodInfoCache =
        new ConcurrentDictionary<string, MethodInfo>();
    
    MethodInfo GetMethodInfo(string methodName, Type genericTypeDefinition)
    {
        var typeCacheKey = $"{methodName}.{genericTypeDefinition.FullName}";
        if (QueryableMethodInfoCache.TryGetValue(typeCacheKey, out var ınfo))
        {
            return ınfo;
        }

        var methods = typeof(Queryable).GetMethods();
        var method = methods
            .Where(x => x.Name == methodName)
            .Select(x => new { M = x, P = x.GetParameters() });

        var genericMethod = method
            .Where(x => x.P.Length == 2
                        && x.P[0].ParameterType.IsGenericType
                        && x.P[0].ParameterType.GetGenericTypeDefinition() == genericTypeDefinition
                        && x.P[1].ParameterType.IsGenericType
                        && x.P[1].ParameterType.GetGenericTypeDefinition() == typeof(Expression<>))
            .Select(x => new { x.M, A = x.P[1].ParameterType.GetGenericArguments() });
        var genericMethod2 = genericMethod
            .Where(x => x.A[0].IsGenericType
                        && x.A[0].GetGenericTypeDefinition() == typeof(Func<,>))
            .Select(x => new { x.M, A = x.A[0].GetGenericArguments() })
            .Where(x => x.A[0].IsGenericParameter
                        && x.A[1].IsGenericParameter)
            .Select(x => x.M)
            .Single();
        
        if (!QueryableMethodInfoCache.ContainsKey(typeCacheKey))
            QueryableMethodInfoCache.TryAdd(typeCacheKey, genericMethod2);
        
        return genericMethod2;
    }
    
    public IEnumerator<Expression> GetEnumerator()
    {
        QueryParameter = Expression.Parameter(typeof(IQueryable<TTo>), "OrderQuery");

        var orderParam = Expression.Parameter(typeof(TTo), "o");

       OrderedQueryableVariable = Expression.Variable(typeof(IOrderedQueryable<TTo>), "rt");

        List<Expression> blockExpressions = new List<Expression>();

        var filterType = typeof(TFrom);
        foreach (var (columnConfig, index) in _criteria.OrderBy.OrderBy(x => x.Value.Rank).ThenBy(x => x.Key)
                     .Select((v, i) => (v, i)))
        {
            var isThen = index > 0;
            var prop = filterType.GetProperty(columnConfig.Key);

            if (prop is null)
                continue;

            string propertyName = null;

            try
            {
                var targetMapAttributes = prop.GetCustomAttributes<MapEntityFieldNameAttribute>();

                var targetTypeMapFiledAttribute = targetMapAttributes.Count() > 1
                    ? targetMapAttributes.FirstOrDefault(x => x.TargetType == typeof(TTo))
                    : targetMapAttributes.FirstOrDefault();

                propertyName = targetTypeMapFiledAttribute?.FiledName;
            }
            catch (Exception e)
            {
                propertyName = null;
            }

            if (propertyName is null)
            {
                propertyName = prop.Name;
            }

            var targetPropertyInfo = typeof(TTo).GetProperty(propertyName);
            if (targetPropertyInfo is null) continue;

            yield return Expression.Assign(
                OrderedQueryableVariable,
                Expression.Call(
                    null,
                    GetGenericMethodInfo(targetPropertyInfo, isThen, columnConfig),
                    isThen ? OrderedQueryableVariable : QueryParameter,
                    Expression.Lambda(
                        Expression.Property(orderParam, propertyName)
                        , orderParam))
            );
        }
    }

    

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }
}