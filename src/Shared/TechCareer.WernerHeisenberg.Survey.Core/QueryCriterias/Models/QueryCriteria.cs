using System.ComponentModel.DataAnnotations;
using TechCareer.WernerHeisenberg.Survey.Core.Paginations.Interfaces;

namespace TechCareer.WernerHeisenberg.Survey.Core.QueryCriterias.Models;

public  class QueryCriteria : IPagingRequestModel
{
    public QueryCriteria()
    {
        SearchBy = new Dictionary<string, PropertySearch>();
        OrderBy = new Dictionary<string, ColumnOrder>();
        Columns = new List<string>();
    }

    public bool? ArchiveSearch { get; set; } = false;
    public Dictionary<string, PropertySearch> SearchBy { get; set; }

    public Dictionary<string, ColumnOrder> OrderBy { get; set; }
    public List<string> Columns { get; set; }

    [Required] public int PageIndex { get; set; } = 0;
    [Required] public int PageSize { get; set; } = 10;
}