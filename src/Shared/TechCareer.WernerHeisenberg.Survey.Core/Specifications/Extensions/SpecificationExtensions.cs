using TechCareer.WernerHeisenberg.Survey.Core.Specifications.Models;

namespace TechCareer.WernerHeisenberg.Survey.Core.Specifications.Extensions;

public static class SpecificationExtensions
{
    public static List<IGrouping<string, KeyValuePair<string, MemberMap>>> SpecificationJoinColumns(
        this Dictionary<string, MemberMap> viewMaps, List<string> columns)
    {
        return viewMaps.Where(x => columns.Contains(x.Key) && x.Value.IsJoinSource)
            .GroupBy(x => x.Value.JoinId).ToList();
    }
    
    public static string JoinColumnName<TEntity, TView>(this JoinMap<TEntity, TView, object> join)
        where TView : class, new()
    {
        return string.Join("", join.EntityKeyMemberInfo.Select(x => x.Name)) + join.RelationalKeyDeclaringType.Name;
    }
}