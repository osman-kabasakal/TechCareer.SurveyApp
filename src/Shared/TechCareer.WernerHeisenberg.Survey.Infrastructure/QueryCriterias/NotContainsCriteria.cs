using System.Linq.Expressions;
using TechCareer.WernerHeisenberg.Survey.Core.ApplicationExtesions;
using TechCareer.WernerHeisenberg.Survey.Core.QueryCriterias.Attributes;
using TechCareer.WernerHeisenberg.Survey.Core.QueryCriterias.Interfaces;
using TechCareer.WernerHeisenberg.Survey.Core.QueryCriterias.Models;

namespace TechCareer.WernerHeisenberg.Survey.Infrastructure.QueryCriterias;
[ExpressionTypeKey( CriteriaTypeKeys.NotContains)]
public class NotContainsCriteria: ICriteriaExpression
{
    public Expression? GetExpression<TTarget>(QueryCriteriaContext criteriaContext) where TTarget : class, new()
    {
        var queryVal = criteriaContext.Converter.ConvertFromInvariantString(criteriaContext.Query.Value.ToSafeString());

        if (queryVal is null)
            return null;

        var containsMethod = criteriaContext.TargetPropertyInfo.PropertyType.GetMethod("Contains",
            new[] { criteriaContext.TargetPropertyInfo.PropertyType });

        if (containsMethod != null)
        {
            var contains = Expression.Call(
                criteriaContext.NameProperty,
                containsMethod,
                Expression.Convert(Expression.Constant(queryVal), criteriaContext.TargetPropertyInfo.PropertyType));

            return Expression.Equal(contains, Expression.Constant(false));
        }

        return null;
    }
}