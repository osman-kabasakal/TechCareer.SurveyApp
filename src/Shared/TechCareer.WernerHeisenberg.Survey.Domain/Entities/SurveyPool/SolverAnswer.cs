using TechCareer.WernerHeisenberg.Survey.Core.Persistence;
using TechCareer.WernerHeisenberg.Survey.Domain.Entities.Identity;
using TechCareer.WernerHeisenberg.Survey.Domain.Entities.QuestionPool;

namespace TechCareer.WernerHeisenberg.Survey.Domain.Entities.SurveyPool;

public class SolverAnswer: BaseEntity
{
    public int? SolverUserId { get; set; }
    public int SurveyQuestionId { get; set; }
    public int? SelectedAnswerId { get; set; }

    public string? SolverFullname { get; set; }

    public ApplicationUser SolverUser { get; set; }
    public SurveyQuestion SurveyQuestion { get; set; }
    public QuestionAnswer SelectedAnswer { get; set; }
}