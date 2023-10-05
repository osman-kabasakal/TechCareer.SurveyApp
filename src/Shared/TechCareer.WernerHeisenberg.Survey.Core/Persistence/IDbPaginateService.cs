using TechCareer.WernerHeisenberg.Survey.Core.Paginations.Interfaces;

namespace TechCareer.WernerHeisenberg.Survey.Core.Persistence;

public interface IDbPaginateService
{
    Task<IPaginate<T>> ToPaginateAsync<T>(IQueryable<T> source, IPagingRequestModel pagingRequestModel, CancellationToken cancellationToken = default);
    IPaginate<T> ToPaginate<T>(IQueryable<T> source, IPagingRequestModel pagingRequestModel);
}