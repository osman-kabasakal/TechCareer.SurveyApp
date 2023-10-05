using TechCareer.WernerHeisenberg.Survey.Core.Persistence;
using TechCareer.WernerHeisenberg.Survey.Core.Persistence.EntityTypes;

namespace TechCareer.WernerHeisenberg.Survey.Core.Repository;

public interface IGenericRepository<TEntity>
where TEntity : class, IEntity ,new()
{
    
}