using System.Linq.Expressions;
using TechCareer.WernerHeisenberg.Survey.Core.QueryCriterias.Models;

namespace TechCareer.WernerHeisenberg.Survey.Core.QueryCriterias.Interfaces;

public interface ICriteriaExpression
{
    Expression? GetExpression<TTarget>(QueryCriteriaContext criteriaContext) where TTarget : class, new();
}