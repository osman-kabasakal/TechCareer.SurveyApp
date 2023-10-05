using System.Linq.Expressions;
using System.Reflection;
using Microsoft.AspNetCore.DataProtection.EntityFrameworkCore;
using TechCareer.WernerHeisenberg.Survey.Domain.Entities.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using TechCareer.WernerHeisenberg.Survey.Core.ApplicationExtesions;
using TechCareer.WernerHeisenberg.Survey.Core.Paginations.Interfaces;
using TechCareer.WernerHeisenberg.Survey.Core.Persistence;
using TechCareer.WernerHeisenberg.Survey.Core.Persistence.EntityTypes;
using TechCareer.WernerHeisenberg.Survey.Core.QueryCriterias.Interfaces;
using TechCareer.WernerHeisenberg.Survey.Core.QueryCriterias.Models;
using TechCareer.WernerHeisenberg.Survey.Core.Specifications.Abstract;
using TechCareer.WernerHeisenberg.Survey.Domain.Entities.QuestionPool;
using TechCareer.WernerHeisenberg.Survey.Domain.Entities.SurveyPool;

namespace TechCareer.WernerHeisenberg.Survey.Infrastructure.Persistence.EFCore;

public class EfApplicationDbContext : IdentityDbContext<ApplicationUser, ApplicationRole, int>, IApplicationDbContext,
    IDataProtectionKeyContext
{
    private readonly IConfiguration _configuration;
    private readonly IQueryCriteriaFactory _criteriaFactory;
    private readonly IOrderLambdaFactory _orderLambdaFactory;
    private readonly IServiceProvider _serviceProvider;
    private readonly IDbPaginateService _paginateService;


    public EfApplicationDbContext(
        DbContextOptions<EfApplicationDbContext> options,
        IConfiguration configuration,
        IQueryCriteriaFactory criteriaFactory,
        IOrderLambdaFactory orderLambdaFactory,
        IServiceProvider serviceProvider,
        IDbPaginateService paginateService
    ) : base(options)
    {
        _configuration = configuration;
        _criteriaFactory = criteriaFactory;
        _orderLambdaFactory = orderLambdaFactory;
        _serviceProvider = serviceProvider;
        _paginateService = paginateService;
    }

    public DbSet<DataProtectionKey> DataProtectionKeys { get; set; }
    public DbSet<Question> Questions { get; set; }
    public DbSet<QuestionAnswer> QuestionPools { get; set; }
    public DbSet<UserSurvey> QuestionPoolQuestions { get; set; }
    public DbSet<SurveyQuestion> QuestionTypes { get; set; }
    public DbSet<SolverAnswer> QuestionOptions { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        if (!optionsBuilder.IsConfigured)
        {
            var connectionString = _configuration.GetConnectionString("DefaultConnection");
            optionsBuilder.UseSqlServer(connectionString);
        }

        base.OnConfiguring(optionsBuilder);
    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
        builder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
    }

    public IQueryable<TEntity> GetTable<TEntity>() where TEntity : class, new()
    {
        return Set<TEntity>().AsQueryable();
    }

    public async Task RunInTransaction(Func<Task> func, CancellationToken cancellationToken = default)
    {
        using (var transaction = await Database.BeginTransactionAsync(cancellationToken))
        {
            try
            {
                await func();
                await transaction.CommitAsync(cancellationToken);
            }
            catch (Exception e)
            {
                await transaction.RollbackAsync(cancellationToken);
                throw;
            }
        }
    }

    public async Task<TReturn> RunInTransaction<TReturn>(Func<Task<TReturn>> func,
        CancellationToken cancellationToken = default)
    {
        TReturn result;
        using (var transaction = await Database.BeginTransactionAsync(cancellationToken))
        {
            try
            {
                result = await func();
                await transaction.CommitAsync(cancellationToken);
            }
            catch (Exception e)
            {
                await transaction.RollbackAsync(cancellationToken);
                throw;
            }
        }

        return result;
    }

    public Task<IPaginate<TViewModel>> GetPaginatedListAsync<TEntity, TViewModel>(QueryCriteria criteria,
        CancellationToken cancellationToken = default) where TEntity : class, IEntity, new()
        where TViewModel : class, new()
    {
        var specification= _serviceProvider.GetRequiredService<BaseSpecification<TEntity,TViewModel>>();
        var orderLambda = _orderLambdaFactory.CreateLambda<TViewModel,TViewModel>(criteria);
        if (orderLambda is null)
        {
            criteria.Columns.Add(specification.DefaultOrderExpression().FindProperty()!.Name);
        }
        var query =  specification.GetQuery(criteria);
        var criteriaExpression = _criteriaFactory.GetExpression<TViewModel,TViewModel>(criteria);
        
        if (criteriaExpression is not null)
        {
            query = query.Where(criteriaExpression);
        }

        if (orderLambda is not null)
        {
            query= orderLambda(query);
        }
        else
        {
            query = specification.DefaultOrder(query, specification.DefaultOrderExpression());
        }
        
        return _paginateService.ToPaginateAsync(query,criteria, cancellationToken);
    }

    public Task AddRangeAsync<TEntity>(IEnumerable<TEntity> entities, CancellationToken cancellationToken = default)
        where TEntity : class, IEntity, new()
    {
        return Set<TEntity>().AddRangeAsync(entities, cancellationToken);
    }

    public Task UpdateAsync<TEntity>(TEntity entity) where TEntity : class, IEntity, new()
    {
        Set<TEntity>().Update(entity);
        return Task.CompletedTask;
    }

    public Task UpdateAsync<TEntity>(IEnumerable<TEntity> entities) where TEntity : class, IEntity, new()
    {
        Set<TEntity>().UpdateRange(entities);
        return Task.CompletedTask;
    }

    public Task DeleteAsync<TEntity>(TEntity entity, CancellationToken cancellationToken = default)
        where TEntity : class, IEntity, new()
    {
        Set<TEntity>().Remove(entity);

        return Task.CompletedTask;
    }

    public Task DeleteAsync<TEntity>(IEnumerable<TEntity> entities, CancellationToken cancellationToken = default)
        where TEntity : class, IEntity, new()
    {
        Set<TEntity>().RemoveRange(entities);
        return Task.CompletedTask;
    }

    public new Task AddAsync<TEntity>(TEntity entities, CancellationToken cancellationToken = default)
        where TEntity : class, IEntity, new()
    {
        return Set<TEntity>().AddAsync(entities, cancellationToken).AsTask();
    }
}