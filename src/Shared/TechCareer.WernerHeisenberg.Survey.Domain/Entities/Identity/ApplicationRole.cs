using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Identity;
using TechCareer.WernerHeisenberg.Survey.Core.Persistence.EntityTypes;

namespace TechCareer.WernerHeisenberg.Survey.Domain.Entities.Identity;

public class ApplicationRole: IdentityRole<int>, IEntity,IAuditableEntity,ISoftDeleteEntity
{
    public ApplicationRole():base()
    {
        
    }
    public ApplicationRole(string roleName):base(roleName)
    {
        
    }

    public int Creator { get; set; }
    public DateTime CreateDate { get; set; }
    public int? Modifier { get; set; }
    public DateTime? ModifyDate { get; set; }
    public bool Deleted { get; set; }
    
    [JsonIgnore,Timestamp] 
    public byte[] TimeStamp { get; set; }

    public bool SystemRole { get; set; }
}