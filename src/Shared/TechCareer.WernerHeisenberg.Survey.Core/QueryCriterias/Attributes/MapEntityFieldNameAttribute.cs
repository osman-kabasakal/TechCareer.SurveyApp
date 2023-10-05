namespace TechCareer.WernerHeisenberg.Survey.Core.QueryCriterias.Attributes;

[AttributeUsage(AttributeTargets.Property, AllowMultiple = true)]
public class MapEntityFieldNameAttribute:Attribute
{
    public MapEntityFieldNameAttribute(string filedName)
    {
        FiledName = filedName;
    }

    public string FiledName { get; set; }
    public Type TargetType { get; set; }
}