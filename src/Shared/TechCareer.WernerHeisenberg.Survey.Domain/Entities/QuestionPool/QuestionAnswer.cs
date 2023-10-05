using TechCareer.WernerHeisenberg.Survey.Core.Persistence;
using TechCareer.WernerHeisenberg.Survey.Core.Persistence.EntityTypes;
using TechCareer.WernerHeisenberg.Survey.Domain.Entities.Identity;
using TechCareer.WernerHeisenberg.Survey.Domain.Entities.SurveyPool;

namespace TechCareer.WernerHeisenberg.Survey.Domain.Entities.QuestionPool;

public class QuestionAnswer: BaseEntity, IAuditableEntity<ApplicationUser>, ISoftDeleteEntity
{
    public QuestionAnswer()
    {
        PreferredSolventsAnswers = Array.Empty<SolverAnswer>();
        ChosenBySurveyQuestions = Array.Empty<SurveyQuestion>();
    }
    
    public int QuestionId { get; set; }
    public byte DisplayOrder { get; set; }
    public string Text { get; set; }
    public int CreatorUserId { get; set; }
    public DateTime CreateDate { get; set; }
    public int? ModifierUserId { get; set; }
    public DateTime? ModifyDate { get; set; }
    public bool Deleted { get; set; }

    public Question Question { get; set; }
    public IEnumerable<SolverAnswer> PreferredSolventsAnswers { get; set; }
    public ApplicationUser CreatorUser { get; set; }
    public ApplicationUser ModifierUser { get; set; }
    public IEnumerable<SurveyQuestion> ChosenBySurveyQuestions { get; set; }
}