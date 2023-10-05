using System.Collections;
using System.ComponentModel;
using System.Linq.Expressions;
using System.Reflection;
using TechCareer.WernerHeisenberg.Survey.Core.QueryCriterias.Attributes;
using TechCareer.WernerHeisenberg.Survey.Core.QueryCriterias.Models;

namespace TechCareer.WernerHeisenberg.Survey.Core.QueryCriterias.Collections;

internal class QueryCriteriaCollection<TFrom, TTo> : IEnumerable<QueryCriteriaContext>
{
    private readonly QueryCriteria _criteria;

    public QueryCriteriaCollection(QueryCriteria criteria)
    {
        _criteria = criteria;
        ExpressiomParameter = Expression.Parameter(typeof(TTo), "TableQueriesX");
    }

    public IEnumerator<QueryCriteriaContext> GetEnumerator()
    {
        var criteriaType = typeof(TFrom);
        var targetType = typeof(TTo);
        var criteriaProperties = criteriaType.GetProperties();
        foreach (var criteriaProp in criteriaProperties)
        {
            if (criteriaProp.PropertyType.IsGenericType && criteriaProp.PropertyType.GetGenericTypeDefinition()
                    .GetInterfaces()
                    .Any(i => i.IsAssignableTo(typeof(IEnumerable))))
                continue;

            string propertyName = null;
            try
            {
                var targetMapAttributes = criteriaProp.GetCustomAttributes<MapEntityFieldNameAttribute>();

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
                propertyName = criteriaProp.Name;
            }

            var targetPropertyInfo = targetType.GetProperty(propertyName);
            if (targetPropertyInfo is null
                || (targetPropertyInfo.PropertyType.IsGenericType && targetPropertyInfo.PropertyType
                    .GetGenericTypeDefinition().GetInterfaces().Any(i => i == typeof(IEnumerable)))
                || !criteriaProp.PropertyType.IsAssignableTo(targetPropertyInfo.PropertyType)) continue;

            if (!_criteria.SearchBy.TryGetValue(criteriaProp.Name, out var searchBy)) continue;

            var nameProperty = Expression.Property(ExpressiomParameter, propertyName);

            var converter = TypeDescriptor.GetConverter(targetPropertyInfo.PropertyType);

            yield return new QueryCriteriaContext(searchBy, targetPropertyInfo, converter, nameProperty);
        }
    }

    public ParameterExpression ExpressiomParameter { get; set; }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }
}