using System.Reflection;
using Microsoft.Extensions.DependencyInjection;
using TechCareer.WernerHeisenberg.Survey.Core.ApplicationExtesions;
using TechCareer.WernerHeisenberg.Survey.Core.TypeResolver;

namespace TechCareer.WernerHeisenberg.Survey.Core;

public class CoreEngine
{
    public static IServiceProvider? ServiceProvider = null;
    public static List<Assembly> LoadedAssemblies { get; set; }
    
    public static ITypeResolver TypeResolver { get; set; }

    public static void SetServiceProvider(IServiceProvider serviceProvider)
    {
        ServiceProvider = serviceProvider;
    }
    
    public static void SetTypeResolver(ITypeResolver typeResolver)
    {
        TypeResolver = typeResolver;
    }
    
    public static void LoadAssemblies()
    {
        var loadedAssemblies = AppDomain.CurrentDomain.GetAssemblies().ToList();
        var loadedPath = loadedAssemblies.Where(a => !a.IsDynamic).Select(x => x.Location);

        var referencedAssemblyPath = Directory.GetFiles(AppDomain.CurrentDomain.BaseDirectory, "*.dll");
        var delta = referencedAssemblyPath.Where(r => !loadedPath.Contains(r, StringComparer.InvariantCultureIgnoreCase));

        foreach (var referencePath in delta)
        {
            if (referencePath.StartsWith(AppDomain.CurrentDomain.BaseDirectory + "TechCareer.WernerHeisenberg.Survey"))
            {
                loadedAssemblies.Add(AppDomain.CurrentDomain.Load(AssemblyName.GetAssemblyName(referencePath.ToSafeString())));
            }
        }
        
        LoadedAssemblies = loadedAssemblies;
    }
    
    public static IServiceScope? GetScope(IServiceProvider serviceProvider = null)
    {
        var provider = serviceProvider ?? ServiceProvider;
        return provider?
            .GetRequiredService<IServiceScopeFactory>()
            .CreateScope();
    }
}