using System.Linq.Expressions;
using TechCareer.WernerHeisenberg.Survey.Core.ApplicationExtesions;
using TechCareer.WernerHeisenberg.Survey.Core.QueryCriterias.Attributes;
using TechCareer.WernerHeisenberg.Survey.Core.QueryCriterias.Extensions;
using TechCareer.WernerHeisenberg.Survey.Core.QueryCriterias.Interfaces;
using TechCareer.WernerHeisenberg.Survey.Core.QueryCriterias.Models;

namespace TechCareer.WernerHeisenberg.Survey.Infrastructure.QueryCriterias;

[ExpressionTypeKey( CriteriaTypeKeys.NotEquals)]
public class NotEqualsCriteria : ICriteriaExpression
{
    public Expression? GetExpression<TTarget>(QueryCriteriaContext criteriaContext) where TTarget : class, new()
    {
        var queryValEquals = criteriaContext.Converter.ConvertFromInvariantString(criteriaContext.Query.Value.ToSafeString());
        var isNullable = criteriaContext.TargetPropertyInfo.PropertyType.IsNullableType();
        if (isNullable)
        {
            return criteriaContext.TargetPropertyInfo.NotNullNotEqualExpression( criteriaContext.NameProperty,
                Expression.Constant(queryValEquals, criteriaContext.TargetPropertyInfo.PropertyType));
        }

        if (queryValEquals != null)
        {
            return 
                Expression.NotEqual(criteriaContext.NameProperty,
                    Expression.Convert(Expression.Constant(queryValEquals),
                        criteriaContext.TargetPropertyInfo.PropertyType));
        }

        return null;
    }
}