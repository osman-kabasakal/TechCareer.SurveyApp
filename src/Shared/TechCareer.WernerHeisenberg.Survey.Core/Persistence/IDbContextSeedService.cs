namespace TechCareer.WernerHeisenberg.Survey.Core.Persistence;

public interface IDbContextSeedService
{
    Task ContextSeedAsync(CancellationToken cancellationToken = default);
}