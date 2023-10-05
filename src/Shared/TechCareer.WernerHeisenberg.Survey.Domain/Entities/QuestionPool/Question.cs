using TechCareer.WernerHeisenberg.Survey.Core.Persistence;
using TechCareer.WernerHeisenberg.Survey.Core.Persistence.EntityTypes;
using TechCareer.WernerHeisenberg.Survey.Domain.Entities.Identity;
using TechCareer.WernerHeisenberg.Survey.Domain.Entities.SurveyPool;
using TechCareer.WernerHeisenberg.Survey.Domain.Enums;

namespace TechCareer.WernerHeisenberg.Survey.Domain.Entities.QuestionPool;

public class Question: BaseEntity, ISoftDeleteEntity, IAuditableEntity<ApplicationUser>
{
    public Question()
    {
        Answers = Array.Empty<QuestionAnswer>();
        SurveyQuestions = Array.Empty<SurveyQuestion>();
    }
    
    public string Text { get; set; }
    public QuestionTypes QuestionType { get; set; }
    public AnswerTypes AnswerType { get; set; }
    public bool IsPublic { get; set; }
    public bool Deleted { get; set; }
    public int CreatorUserId { get; set; }
    public DateTime CreateDate { get; set; }
    public int? ModifierUserId { get; set; }
    public DateTime? ModifyDate { get; set; }

    public IEnumerable<QuestionAnswer> Answers { get; set; }
    public IEnumerable<SurveyQuestion> SurveyQuestions { get; set; }
    public ApplicationUser CreatorUser { get; set; }
    public ApplicationUser ModifierUser { get; set; }
}