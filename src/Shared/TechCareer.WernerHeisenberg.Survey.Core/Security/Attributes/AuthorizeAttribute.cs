namespace TechCareer.WernerHeisenberg.Survey.Core.Security.Attributes;

[AttributeUsage(AttributeTargets.Class, AllowMultiple = true, Inherited = true)]
public class AuthorizeAttribute: Attribute
{
    
    /// <summary>
    /// You can use comma separated role names for multiple roles
    /// </summary>
    public string Roles { get; set; }
}