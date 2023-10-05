using System.Linq.Expressions;
using TechCareer.WernerHeisenberg.Survey.Core.QueryCriterias.Collections;
using TechCareer.WernerHeisenberg.Survey.Core.QueryCriterias.Interfaces;
using TechCareer.WernerHeisenberg.Survey.Core.QueryCriterias.Models;

namespace TechCareer.WernerHeisenberg.Survey.Core.QueryCriterias.Factories;

internal class OrderLambdaFactory: IOrderLambdaFactory
{
    public Func<IQueryable<TTo>, IOrderedQueryable<TTo>>? CreateLambda<TFrom, TTo>(QueryCriteria criteria)
    {
        var orderExpressionCollection = new OrderExpressionCollection<TFrom, TTo>(criteria);
        var orderExpressionList= orderExpressionCollection.ToList();
        if (!orderExpressionList.Any())
            return null;

        return Expression.Lambda<Func<IQueryable<TTo>, IOrderedQueryable<TTo>>>(Expression.Block(
            new[] { orderExpressionCollection.OrderedQueryableVariable },
            orderExpressionList
        ), orderExpressionCollection.QueryParameter).Compile();
    }
}