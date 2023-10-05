using System.Collections;
using TechCareer.WernerHeisenberg.Survey.Core.QueryCriterias.Extensions;
using TechCareer.WernerHeisenberg.Survey.Core.QueryCriterias.Models;
using TechCareer.WernerHeisenberg.Survey.Core.Specifications.Extensions;
using TechCareer.WernerHeisenberg.Survey.Core.Specifications.Models;

namespace TechCareer.WernerHeisenberg.Survey.Core.Specifications.Collections;

internal class SelectFieldClauseCollection<TEntity,TView>: IEnumerable<string>
where TView : class, new()
{
    private Dictionary<string, MemberMap> _viewMaps;
    private readonly List<string> _columns;
    private readonly List<IGrouping<string, KeyValuePair<string, MemberMap>>> _joinColumns;
    private Dictionary<string, JoinMap<TEntity, TView, object>> _joins;

    public SelectFieldClauseCollection(Dictionary<string, MemberMap> viewMaps, QueryCriteria queryCriteria, Dictionary<string, JoinMap<TEntity, TView, object>> joins)
    {
        _viewMaps = viewMaps;
        _joins = joins;
        _columns = queryCriteria.SpecificationsColumns<TView>();
        _joinColumns = _viewMaps.SpecificationJoinColumns(_columns);
    }
    
    public IEnumerator<string> GetEnumerator()
    {
        foreach (var col in _columns)
        {
            if (_viewMaps.TryGetValue(col, out var memberMap))
            {
                if (memberMap.IsJoinSource)
                {
                    var joinConfig = _joins[memberMap.JoinId];
                    yield return  $"it.{joinConfig.JoinColumnName()}.{memberMap.EntityMemberInfo.Name} as {memberMap.ViewMemberInfo.Name}";
                }
                else
                {
                    // as {memberMap.ViewMemberInfo.Name}
                    var prefix = $"it";

                    if (_joinColumns.Any())
                    {
                        prefix = $"it.{typeof(TEntity).Name}";
                    }

                    if (memberMap.IsSelectorClause)
                    {
                        prefix = memberMap.SelectorClause(prefix);
                    }
                    else
                    {
                        prefix += $".{memberMap.EntityMemberInfo.Name}";
                    }

                    var s = $"{prefix} as {memberMap.ViewMemberInfo.Name}";

                    yield return s.Replace("\n", "").Replace("\t", "").Replace("\r", "").Replace("  ", "");
                }
            }
        }
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }
}