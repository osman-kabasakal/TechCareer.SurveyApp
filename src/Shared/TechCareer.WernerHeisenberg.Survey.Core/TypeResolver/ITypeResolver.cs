namespace TechCareer.WernerHeisenberg.Survey.Core.TypeResolver;

public interface ITypeResolver
{
    IEnumerable<Type> FindClassesOfType<T>(bool onlyConcreteClasses = true);
    IEnumerable<Type> FindClassesOfType(Type assignTypeFrom, bool onlyConcreteClasses = true);

}