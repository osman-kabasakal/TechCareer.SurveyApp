using System.Linq.Expressions;
using TechCareer.WernerHeisenberg.Survey.Core.ApplicationExtesions;
using TechCareer.WernerHeisenberg.Survey.Core.QueryCriterias.Attributes;
using TechCareer.WernerHeisenberg.Survey.Core.QueryCriterias.Interfaces;
using TechCareer.WernerHeisenberg.Survey.Core.QueryCriterias.Models;

namespace TechCareer.WernerHeisenberg.Survey.Infrastructure.QueryCriterias;
[ExpressionTypeKey(CriteriaTypeKeys.RangeOut)]
public class RangeOutCriteria: ICriteriaExpression
{
    public Expression? GetExpression<TTarget>(QueryCriteriaContext criteriaContext) where TTarget : class, new()
    {
        var rangeInSplit = criteriaContext.Query.Value.ToSafeString()
            .Split(new[] { "<>" }, StringSplitOptions.None);

        if (rangeInSplit.Count() != 2) return null;
        
        var minValue = Expression.Convert(Expression.Constant(
                criteriaContext.Converter.ConvertFromInvariantString(rangeInSplit[0].ToSafeString())),
            criteriaContext.TargetPropertyInfo.PropertyType);
            
        var maxValue = Expression.Convert(Expression.Constant(
                criteriaContext.Converter.ConvertFromInvariantString(rangeInSplit[1].ToSafeString())),
            criteriaContext.TargetPropertyInfo.PropertyType);
            
        return Expression.AndAlso(
            Expression.LessThan(criteriaContext.NameProperty,
                minValue
            ),
            Expression.GreaterThan(criteriaContext.NameProperty,
                maxValue
            )
        );
    }
}