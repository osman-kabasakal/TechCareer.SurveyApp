namespace TechCareer.WernerHeisenberg.Survey.Core.Persistence.EntityTypes;

public interface ISoftDeleteEntity
{
    bool Deleted { get; set; }
}