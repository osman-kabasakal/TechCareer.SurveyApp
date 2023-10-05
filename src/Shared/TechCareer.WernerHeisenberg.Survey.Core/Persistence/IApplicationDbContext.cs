using Microsoft.AspNetCore.DataProtection.EntityFrameworkCore;
using TechCareer.WernerHeisenberg.Survey.Core.Paginations.Interfaces;
using TechCareer.WernerHeisenberg.Survey.Core.Persistence.EntityTypes;
using TechCareer.WernerHeisenberg.Survey.Core.QueryCriterias.Models;

namespace TechCareer.WernerHeisenberg.Survey.Core.Persistence;

public interface IApplicationDbContext
{
    IQueryable<TEntity> GetTable<TEntity>()
        where TEntity: class, new();
    
    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
    Task RunInTransaction(Func<Task> func, CancellationToken cancellationToken = default);
    Task<TReturn> RunInTransaction<TReturn>(Func<Task<TReturn>> func, CancellationToken cancellationToken = default);

    Task<IPaginate<TViewModel>> GetPaginatedListAsync<TEntity, TViewModel>(QueryCriteria criteria, CancellationToken cancellationToken = default)
        where TEntity : class, IEntity, new()
        where TViewModel : class, new();
    
    Task AddAsync<TEntity>(
        TEntity entity,
        CancellationToken cancellationToken = default)
        where TEntity : class, IEntity, new();
    
    Task AddRangeAsync<TEntity>(IEnumerable<TEntity> entities, CancellationToken cancellationToken = default)
        where TEntity : class, IEntity, new();

    Task UpdateAsync<TEntity>(TEntity entity)
        where TEntity : class, IEntity, new();
    
    Task UpdateAsync<TEntity>(IEnumerable<TEntity> entity)
        where TEntity : class, IEntity, new();

    Task DeleteAsync<TEntity>(TEntity entity, CancellationToken cancellationToken = default)
        where TEntity : class, IEntity, new();
    
    Task DeleteAsync<TEntity>(IEnumerable<TEntity> entities, CancellationToken cancellationToken = default)
        where TEntity : class, IEntity, new();
}