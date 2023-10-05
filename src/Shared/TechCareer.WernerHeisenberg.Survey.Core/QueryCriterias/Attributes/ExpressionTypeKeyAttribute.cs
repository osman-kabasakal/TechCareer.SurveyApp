namespace TechCareer.WernerHeisenberg.Survey.Core.QueryCriterias.Attributes;

[AttributeUsage( AttributeTargets.Class, AllowMultiple = false, Inherited = true)]
public class ExpressionTypeKeyAttribute: Attribute
{
    public string Key { get; set; }

    public ExpressionTypeKeyAttribute(string key)
    {
        Key = key;
    }
}