namespace TechCareer.WernerHeisenberg.Survey.Core.Specifications.Models;

public class JoinIteratorModel
{
    public string SelectClause { get; set; }
    public JoinType JoinType { get; set; }
    public IQueryable JoinQuery { get; set; }
    public string LeftKey { get; set; }
    public string RightKey { get; set; }
}