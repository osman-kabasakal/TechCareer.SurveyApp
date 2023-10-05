namespace TechCareer.WernerHeisenberg.Survey.Core.Paginations.Interfaces;

public interface IPagingRequestModel
{
    int PageIndex { get; set; }
    int PageSize { get; set; }
}