namespace TechCareer.WernerHeisenberg.Survey.Core.Paginations.Interfaces;

public interface IPaginate<T>
{
    int StartIndex { get; }

    int EndIndex { get; }
    
    int PageIndex { get; }

    int Size { get; }

    int Count { get; }

    int Pages { get; }

    IEnumerable<T> Items { get; }

    bool HasPrevious { get; }

    bool HasNext { get; }
}