using System.ComponentModel;
using System.Linq.Expressions;
using System.Reflection;
using TechCareer.WernerHeisenberg.Survey.Core.ApplicationExtesions;
using TechCareer.WernerHeisenberg.Survey.Core.QueryCriterias.Models;

namespace TechCareer.WernerHeisenberg.Survey.Core.QueryCriterias.Extensions;

public static class QueryCriteriaExtensions
{
    public static BinaryExpression NotNullEqualExpression(this PropertyInfo targetPropertyInfo,
        Expression nameProperty, ConstantExpression value)
    {
        var notNullExpression =
            Expression.NotEqual(nameProperty, Expression.Constant(null, targetPropertyInfo.PropertyType));
        var notNullAndEqualExpression = Expression.AndAlso(notNullExpression, Expression.Equal(nameProperty, value));
        return notNullAndEqualExpression;
    }

    public static BinaryExpression NotNullNotEqualExpression(this PropertyInfo targetPropertyInfo,
        Expression nameProperty, ConstantExpression value)
    {
        var notNullExpression =
            Expression.NotEqual(nameProperty, Expression.Constant(null, targetPropertyInfo.PropertyType));
        var notNullAndEqualExpression = Expression.AndAlso(notNullExpression, Expression.NotEqual(nameProperty, value));
        return notNullAndEqualExpression;
    }

    public static Expression? ToArrayExpression(this PropertyInfo prop, PropertySearch query,
        TypeConverter converter, Expression nameProperty, bool inArray)
    {
        var targetList = Activator.CreateInstance(typeof(List<>).MakeGenericType(prop.PropertyType));

        var splitInArrayValue = query.Value.ToSafeString().Split('~');

        var addMethodInfo =
            targetList?.GetType().GetMethod("Add", new[] { prop.PropertyType });

        if (addMethodInfo is null)
        {
            return null;
        }
        
        foreach (var val in splitInArrayValue)
        {
            var vv = converter.ConvertFromInvariantString(val.ToSafeString());

            if (vv is null)
                continue;

            addMethodInfo?.Invoke(targetList, new[] { vv });
        }

        if (splitInArrayValue.Any())
        {
            var containsMethod3 = targetList?.GetType().GetMethod("Contains",
                new[] { prop.PropertyType });
            if (containsMethod3 is null)
            {
                return null;
            }

            var contains = Expression.Call(
                Expression.Constant(targetList),
                containsMethod3,
                nameProperty
            );
            
              return  Expression.Equal(contains, Expression.Constant(inArray));
        }

        return null;
    }
    
    public static List<string> ViewPropNames<TView>()
    {
        return typeof(TView).GetProperties().Select(x => x.Name).ToList();
    }

    public static string? ViewKeyName<TView>(this string propName)
    {
        var member = typeof(TView).GetProperties().FirstOrDefault(x => x.Name.Equals(propName));
        if (member != null)
            return member.Name;

        return null;
    }
    
    public static List<string> SpecificationsColumns<TView>(this QueryCriteria criteria)
        where TView : class, new()
    {
        return criteria.SearchBy.Keys.ToList().Union(criteria.OrderBy.Keys.ToList())
            .Union(criteria.Columns.Any() ? criteria.Columns : ViewPropNames<TView>())
            .Select(x => x.ViewKeyName<TView>())
            .Where(x => !string.IsNullOrEmpty(x)).ToList();
    }

    public static bool IsNullableType(this Type type)
    {
        return type.IsGenericType && type.GetGenericTypeDefinition() == typeof(Nullable<>);
    }
}