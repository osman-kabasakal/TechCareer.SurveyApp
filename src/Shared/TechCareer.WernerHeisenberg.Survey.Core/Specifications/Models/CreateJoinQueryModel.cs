using TechCareer.WernerHeisenberg.Survey.Core.Persistence.EntityTypes;
using TechCareer.WernerHeisenberg.Survey.Core.Specifications.Abstract;

namespace TechCareer.WernerHeisenberg.Survey.Core.Specifications.Models;

public class CreateJoinQueryModel<TEntity, TView, TJoinView>
    where TEntity : class,IEntity, new()
    where TView : class, new()
{
    public JoinMap<TEntity, TView, object> MapConfiguration { get; set; }

    public BaseSpecification<TEntity, TView> Specification { get; set; }
}