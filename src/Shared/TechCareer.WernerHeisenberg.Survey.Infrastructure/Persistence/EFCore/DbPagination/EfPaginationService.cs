using Microsoft.EntityFrameworkCore;
using TechCareer.WernerHeisenberg.Survey.Application.Models.Paginations;
using TechCareer.WernerHeisenberg.Survey.Core.Paginations.Interfaces;
using TechCareer.WernerHeisenberg.Survey.Core.Persistence;

namespace TechCareer.WernerHeisenberg.Survey.Infrastructure.Persistence.EFCore.DbPagination;

public class EfPaginationService : IDbPaginateService
{
    public async Task<IPaginate<T>> ToPaginateAsync<T>(IQueryable<T> source, IPagingRequestModel pagingRequestModel, CancellationToken cancellationToken = default)
    {
        var count = await source.CountAsync(cancellationToken: cancellationToken);
        var items = await source.Skip(pagingRequestModel.PageIndex * pagingRequestModel.PageSize)
            .Take(pagingRequestModel.PageSize).ToListAsync(cancellationToken: cancellationToken);
        return Paginate<T>.Create(items, pagingRequestModel, count);
    }

    public IPaginate<T> ToPaginate<T>(IQueryable<T> source, IPagingRequestModel pagingRequestModel)
    {
        var count = source.Count();
        var items = source.Skip(pagingRequestModel.PageIndex * pagingRequestModel.PageSize)
            .Take(pagingRequestModel.PageSize).ToList();
        return Paginate<T>.Create(items, pagingRequestModel, count);
    }
}