namespace TechCareer.WernerHeisenberg.Survey.Core.Persistence.EntityTypes;


public interface IEntity
{
    int Id { get; set; }
    byte[] TimeStamp { get; set; }
}