using TechCareer.WernerHeisenberg.Survey.Core.Paginations.Interfaces;

namespace TechCareer.WernerHeisenberg.Survey.Application.Models.Paginations;

public class Paginate<T>:IPaginate<T>
{
    private Paginate(){}
    public int StartIndex { get; init; }
    public int EndIndex { get; init; }
    public int PageIndex { get; init; }
    public int Size { get; init; }
    public int Count { get; init; }
    public int Pages { get; init; }
    public IEnumerable<T> Items { get; init; }
    public bool HasPrevious { get; init; }
    public bool HasNext { get; init; }

    public static Paginate<T> Create(ICollection<T> collection, IPagingRequestModel requestModel, int? count=null)
    {
        count ??= collection.Count;
        var totalPages = (int)Math.Ceiling(count.Value / (double)requestModel.PageSize);
        return new Paginate<T>
        {
            StartIndex = requestModel.PageIndex * requestModel.PageSize,
            EndIndex = (requestModel.PageIndex * requestModel.PageSize) + count.Value,
            PageIndex = requestModel.PageIndex,
            Size = requestModel.PageSize,
            Count = count.Value,
            Pages = totalPages,
            Items = collection,
            HasPrevious = requestModel.PageIndex > 0,
            HasNext = requestModel.PageIndex < totalPages - 1
        };
    }
}