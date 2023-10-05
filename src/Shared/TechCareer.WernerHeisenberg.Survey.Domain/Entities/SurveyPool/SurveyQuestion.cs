using TechCareer.WernerHeisenberg.Survey.Core.Persistence;
using TechCareer.WernerHeisenberg.Survey.Domain.Entities.QuestionPool;

namespace TechCareer.WernerHeisenberg.Survey.Domain.Entities.SurveyPool;

public class SurveyQuestion: BaseEntity
{
    public SurveyQuestion()
    {
        SolverAnswers = Array.Empty<SolverAnswer>();
    }
    public int? SurveyId { get; set; }
    public int QuestionId { get; set; }
    public int? CorrectAnswerId { get; set; }
    public byte DisplayOrder { get; set; }

    public UserSurvey Survey { get; set; }
    public Question Question { get; set; }

    public QuestionAnswer CorrectAnswer { get; set; }

    public IEnumerable<SolverAnswer> SolverAnswers { get; set; }
}