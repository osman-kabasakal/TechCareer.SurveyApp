using System.Linq.Expressions;
using TechCareer.WernerHeisenberg.Survey.Core.QueryCriterias.Models;

namespace TechCareer.WernerHeisenberg.Survey.Core.QueryCriterias.Interfaces;

public interface IQueryCriteriaFactory
{
    Expression<Func<TTO, bool>>? GetExpression<TFrom, TTO>(QueryCriteria queryCriteria)
        where TTO : class, new()
        where TFrom : class, new();
}