using System.Linq.Expressions;
using TechCareer.WernerHeisenberg.Survey.Core.ApplicationExtesions;
using TechCareer.WernerHeisenberg.Survey.Core.QueryCriterias.Attributes;
using TechCareer.WernerHeisenberg.Survey.Core.QueryCriterias.Interfaces;
using TechCareer.WernerHeisenberg.Survey.Core.QueryCriterias.Models;

namespace TechCareer.WernerHeisenberg.Survey.Infrastructure.QueryCriterias;

[ExpressionTypeKey(CriteriaTypeKeys.LessThen)]
public class LessThenCriteria : ICriteriaExpression
{
    public Expression? GetExpression<TTarget>(QueryCriteriaContext criteriaContext) where TTarget : class, new()
    {
        var queryVal = criteriaContext.Converter.ConvertFromInvariantString(criteriaContext.Query.Value.ToSafeString());

        if (queryVal is null)
            return null;

        return Expression.LessThan(criteriaContext.NameProperty,
            Expression.Convert(Expression.Constant(queryVal), criteriaContext.TargetPropertyInfo.PropertyType));
    }
}