using TechCareer.WernerHeisenberg.Survey.Core.Persistence;
using TechCareer.WernerHeisenberg.Survey.Core.Persistence.EntityTypes;

namespace TechCareer.WernerHeisenberg.Survey.Core.Repository;

public class GenericRepository<TEntity>: IGenericRepository<TEntity>
    where TEntity : class, IEntity, new()
{
    private readonly IApplicationDbContext _dbContext;

    public GenericRepository(
        IApplicationDbContext dbContext
    )
    {
        _dbContext = dbContext;
    }
    
    public async Task<TEntity> AddAsync(TEntity entity)
    {
        await _dbContext.AddAsync(entity);
        await _dbContext.SaveChangesAsync();
        return entity;
    }
}