using TechCareer.WernerHeisenberg.Survey.Core.Persistence;
using TechCareer.WernerHeisenberg.Survey.Core.Persistence.EntityTypes;
using TechCareer.WernerHeisenberg.Survey.Domain.Entities.Identity;

namespace TechCareer.WernerHeisenberg.Survey.Domain.Entities.SurveyPool;

public class UserSurvey: BaseEntity, IAuditableEntity<ApplicationUser>, ISoftDeleteEntity
{
    public UserSurvey()
    {
        SurveyQuestions = Array.Empty<SurveyQuestion>();
    }
    
    public int AssignedUserId { get; set; }
    public string Title { get; set; }
    public string Description { get; set; }
    
    public int CreatorUserId { get; set; }
    public DateTime CreateDate { get; set; }
    public int? ModifierUserId { get; set; }
    public DateTime? ModifyDate { get; set; }
    public bool Deleted { get; set; }


    public ApplicationUser AssignedUser { get; set; }
    public IEnumerable<SurveyQuestion> SurveyQuestions { get; set; }

    public ApplicationUser CreatorUser { get; set; }
    public ApplicationUser ModifierUser { get; set; }
}