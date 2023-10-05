using System.Linq.Expressions;
using TechCareer.WernerHeisenberg.Survey.Core.QueryCriterias.Attributes;
using TechCareer.WernerHeisenberg.Survey.Core.QueryCriterias.Extensions;
using TechCareer.WernerHeisenberg.Survey.Core.QueryCriterias.Interfaces;
using TechCareer.WernerHeisenberg.Survey.Core.QueryCriterias.Models;

namespace TechCareer.WernerHeisenberg.Survey.Infrastructure.QueryCriterias;

[ExpressionTypeKey(CriteriaTypeKeys.InArray)]
public class InCollectionCriteria : ICriteriaExpression
{
    public Expression? GetExpression<TTarget>(QueryCriteriaContext criteriaContext) where TTarget : class, new()
    {
        return criteriaContext.TargetPropertyInfo.ToArrayExpression(criteriaContext.Query,
            criteriaContext.Converter, criteriaContext.NameProperty, true);
    }
}