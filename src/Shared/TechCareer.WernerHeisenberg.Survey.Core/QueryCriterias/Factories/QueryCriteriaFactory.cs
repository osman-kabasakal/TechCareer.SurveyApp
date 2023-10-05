using System.Linq.Expressions;
using TechCareer.WernerHeisenberg.Survey.Core.QueryCriterias.Collections;
using TechCareer.WernerHeisenberg.Survey.Core.QueryCriterias.Interfaces;
using TechCareer.WernerHeisenberg.Survey.Core.QueryCriterias.Models;

namespace TechCareer.WernerHeisenberg.Survey.Core.QueryCriterias.Factories;

internal class QueryCriteriaFactory : IQueryCriteriaFactory
{
    private readonly CriteriaContext _querySpecificationContext;

    public QueryCriteriaFactory(CriteriaContext querySpecificationContext)
    {
        _querySpecificationContext = querySpecificationContext;
    }

    public Expression<Func<TTO, bool>>? GetExpression<TFrom, TTO>(QueryCriteria queryCriteria)
        where TTO : class, new()
        where TFrom : class, new()
    {
        Expression? expression = null;
        var criteriaCollection = new QueryCriteriaCollection<TFrom, TTO>(queryCriteria);

        foreach (var criteriaContext in criteriaCollection)
        {
            if (!_querySpecificationContext.Expressions.TryGetValue(criteriaContext.Query.SearchType,
                    out var criteriaExpression)) continue;

            var criteriaExpressionResult = criteriaExpression.GetExpression<TTO>(criteriaContext);
            if (criteriaExpressionResult is not null)
            {
                expression = expression is null
                    ? criteriaExpressionResult
                    : Expression.AndAlso(expression, criteriaExpressionResult);
            }
        }

        return expression is null
            ? null
            : Expression.Lambda<Func<TTO, bool>>(expression, criteriaCollection.ExpressiomParameter);
    }
}