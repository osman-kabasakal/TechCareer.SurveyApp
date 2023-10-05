using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using TechCareer.WernerHeisenberg.Survey.Core.Persistence.EntityTypes;

namespace TechCareer.WernerHeisenberg.Survey.Core.Persistence;

[NotMapped]
public abstract class BaseEntity:IEntity
{
    public int Id { get; set; }
    [JsonIgnore] [Timestamp] public byte[] TimeStamp { get; set; }
}