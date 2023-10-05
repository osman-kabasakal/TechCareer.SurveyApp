using System.Collections.Concurrent;
using TechCareer.WernerHeisenberg.Survey.Core.QueryCriterias.Interfaces;

namespace TechCareer.WernerHeisenberg.Survey.Core.QueryCriterias;

internal class CriteriaContext
{
    public CriteriaContext()
    {
        Expressions = new ConcurrentDictionary<string, ICriteriaExpression>();
    }
    public ConcurrentDictionary<string,ICriteriaExpression> Expressions { get; set; }

}