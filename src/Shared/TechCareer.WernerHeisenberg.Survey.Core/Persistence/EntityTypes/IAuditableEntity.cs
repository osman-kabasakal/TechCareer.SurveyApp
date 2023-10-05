namespace TechCareer.WernerHeisenberg.Survey.Core.Persistence.EntityTypes;

public interface IAuditableEntity
{

    DateTime CreateDate { get; set; }

    DateTime? ModifyDate { get; set; }
    
}

public interface IAuditableEntity<TUser> : IAuditableEntity
{   
    int CreatorUserId { get; set; }
    TUser CreatorUser { get; set; }
    int? ModifierUserId { get; set; }
    TUser ModifierUser { get; set; }
}