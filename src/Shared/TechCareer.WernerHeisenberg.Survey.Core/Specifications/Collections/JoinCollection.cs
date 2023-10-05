using System.Collections;
using TechCareer.WernerHeisenberg.Survey.Core.Persistence;
using TechCareer.WernerHeisenberg.Survey.Core.QueryCriterias.Extensions;
using TechCareer.WernerHeisenberg.Survey.Core.QueryCriterias.Models;
using TechCareer.WernerHeisenberg.Survey.Core.Specifications.Extensions;
using TechCareer.WernerHeisenberg.Survey.Core.Specifications.Models;

namespace TechCareer.WernerHeisenberg.Survey.Core.Specifications.Collections;


internal class JoinCollection<TEntity,TView>: IEnumerable<JoinIteratorModel>
    where TView : class, new()
{
    private Dictionary<string, MemberMap> _viewMaps;
    
    private Dictionary<string, JoinMap<TEntity, TView, object>> _joins;
    
    private QueryCriteria _queryCriteria;
    private readonly List<string> _columns;
    private readonly List<IGrouping<string, KeyValuePair<string, MemberMap>>> _joinColumns;
    
    private readonly IApplicationDbContext _context;

    
    
    public JoinCollection(Dictionary<string, MemberMap> viewMaps, Dictionary<string, JoinMap<TEntity, TView, object>> joins, QueryCriteria queryCriteria, IApplicationDbContext context)
    {
        _viewMaps = viewMaps;
        _joins = joins;
        _queryCriteria = queryCriteria;
        _context = context;
        _columns = queryCriteria.SpecificationsColumns<TView>();
        _joinColumns = _viewMaps.SpecificationJoinColumns(_columns);
    }

    public JoinIteratorModel? Current { get; private set; }

    public IEnumerator<JoinIteratorModel> GetEnumerator()
    {
         foreach (var _currentCol in _joinColumns)
        {
            var join = _joins[_currentCol.Key];
            var joinTable = join.JoinQueryBuilder.Invoke(_context, _queryCriteria,_currentCol.Select(x=>x.Value.EntityMemberInfo.Name).ToList());
            var selectQuery = "new ({0})";
            var instanceString = "";
            var currentIndex = _joinColumns.IndexOf(_currentCol);
            for (int i = 0; i < currentIndex; i++)
            {
                var joinType = _joins[_joinColumns[i].Key];
                var joinColumnName = joinType.JoinColumnName();
                instanceString += $"outer.{joinColumnName} as {joinColumnName},";
            }

            if (string.IsNullOrEmpty(instanceString))
            {
                instanceString += $"outer as {join.EntityKeyDeclaringType.Name}";
            }
            else
            {
                instanceString += $"outer.{join.EntityKeyDeclaringType.Name} as {join.EntityKeyDeclaringType.Name}";
            }

            var innerList = new List<string>();

            foreach (var item in _currentCol)
            {
                if (item.Value.IsSelectorClause)
                {
                    innerList.Add(
                        $"{item.Value.SelectorClause("inner").Replace("\n", "").Replace("\t", "").Replace("\r", "").Replace("  ", "")} as {item.Value.EntityMemberInfo.Name}");
                    continue;
                }

                innerList.Add($"inner.{item.Value.EntityMemberInfo.Name} as {item.Value.EntityMemberInfo.Name}");
            }

            if (innerList.Any())
            {
                instanceString = string.Join(",",
                    new[] { instanceString, $"new ({string.Join(",", innerList)}) as {join.JoinColumnName()}" });
            }

            var formatted = string.Format(selectQuery, instanceString);
            var entityRelatedKey = "";
            var innerRelatedKey = "";

            if (!(join.EntityKeyMemberInfo.Any() && join.RelationalKeyMemberInfo.Any() &&
                  join.RelationalKeyMemberInfo.Count() == join.EntityKeyMemberInfo.Count()))
            {
                throw new Exception("İlişkili alanlardan enaz bir tane olmalı ve alan sayıları eşit olmalıdır.");
            }

            if (join.RelationalKeyMemberInfo.Count() == 1)
            {
                entityRelatedKey = currentIndex > 0
                    ? $"it.{join.EntityKeyDeclaringType.Name}.{join.EntityKeyMemberInfo.First().Name}"
                    : "it." + join.EntityKeyMemberInfo.First().Name;
                innerRelatedKey = join.RelationalKeyMemberInfo.First().Name;
            }
            else
            {
                entityRelatedKey = string.Format("new ({0})",
                    string.Join(",",
                        join.EntityKeyMemberInfo.Select((x, i) =>
                            currentIndex > 0
                                ? $"it.{join.EntityKeyDeclaringType.Name}.{x.Name} as key{i}"
                                : $"it.{x.Name} as key{i}")));
                innerRelatedKey = string.Format("new ({0})",
                    string.Join(",", join.RelationalKeyMemberInfo.Select((x, i) => $"{x.Name} as key{i}")));
            }

            if (join.JonStrategy==JoinType.Left)
            {
                formatted=  $"inner.DefaultIfEmpty().Select(u=>{formatted.Replace("inner", "u")})";
            }
            
            yield return new JoinIteratorModel
            {
                SelectClause=formatted,
                LeftKey = entityRelatedKey,
                RightKey = innerRelatedKey,
                JoinQuery = joinTable,
                JoinType = join.JonStrategy
            };
        }
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }
}