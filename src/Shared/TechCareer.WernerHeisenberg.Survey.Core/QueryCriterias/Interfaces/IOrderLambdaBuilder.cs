using TechCareer.WernerHeisenberg.Survey.Core.QueryCriterias.Models;

namespace TechCareer.WernerHeisenberg.Survey.Core.QueryCriterias.Interfaces;

public interface IOrderLambdaFactory
{
    Func<IQueryable<TTo>, IOrderedQueryable<TTo>>? CreateLambda<TFrom, TTo>(QueryCriteria criteria);
}