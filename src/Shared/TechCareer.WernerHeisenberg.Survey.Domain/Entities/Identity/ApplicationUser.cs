using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Identity;
using TechCareer.WernerHeisenberg.Survey.Core.Persistence.EntityTypes;
using TechCareer.WernerHeisenberg.Survey.Domain.Entities.SurveyPool;

namespace TechCareer.WernerHeisenberg.Survey.Domain.Entities.Identity;

public class ApplicationUser: IdentityUser<int>, IEntity,ISoftDeleteEntity
{
    public ApplicationUser()
    {
        SharedConstructor();
    }

    public ApplicationUser(string email) : base(email)
    {
        Email = email;
        SharedConstructor();
    }

    public ApplicationUser(string email, string userName) : base(userName)
    {
        Email = email;
        SharedConstructor();
    }
    
    private void SharedConstructor()
    {
        UserSurveys = Array.Empty<UserSurvey>();
        SolverAnswers = Array.Empty<SolverAnswer>();
    }

    public string Firstname { get; set; }
    public string Lastname { get; set; }
    public bool SystemUser { get; set; }

    public bool Deleted { get; set; }
    
    [JsonIgnore,Timestamp] 
    public byte[] TimeStamp { get; set; }

    public IEnumerable<UserSurvey> UserSurveys { get; set; }
    public IEnumerable<SolverAnswer> SolverAnswers { get; set; }
}