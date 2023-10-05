using System.Reflection;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using TechCareer.WernerHeisenberg.Survey.Core.AppSettings;
using TechCareer.WernerHeisenberg.Survey.Core.QueryCriterias;
using TechCareer.WernerHeisenberg.Survey.Core.QueryCriterias.Attributes;
using TechCareer.WernerHeisenberg.Survey.Core.QueryCriterias.Factories;
using TechCareer.WernerHeisenberg.Survey.Core.QueryCriterias.Interfaces;
using TechCareer.WernerHeisenberg.Survey.Core.Repository;
using TechCareer.WernerHeisenberg.Survey.Core.Specifications.Abstract;
using TechCareer.WernerHeisenberg.Survey.Core.TypeResolver;

namespace TechCareer.WernerHeisenberg.Survey.Core;

public static class DependencyInjection
{
    public static IServiceCollection AddCoreDependencies(this IServiceCollection services,
        ConfigurationManager configurationManager)
    {
        CoreEngine.LoadAssemblies();
        var typeResolver = new ApplicationTypeResolver();
        CoreEngine.SetTypeResolver(typeResolver);
        services.AddSingleton<ITypeResolver>(typeResolver);
        services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));
        services.AddQueryCriteria();

        services.AddSpecification();

        return services;
    }
    
    
     
    private static void AddQueryCriteria(this IServiceCollection services)
    {
        var criteriaContext = new CriteriaContext();
        var criteriaTypes = CoreEngine.TypeResolver.FindClassesOfType<ICriteriaExpression>(true);

        foreach (var criteriaType in criteriaTypes)
        {
            var criteria = (ICriteriaExpression)Activator.CreateInstance(criteriaType);
            var keyAttr = criteriaType.GetCustomAttribute<ExpressionTypeKeyAttribute>();
            if (keyAttr != null && criteria != null)
                criteriaContext.Expressions[keyAttr.Key] = criteria;
        }

        services.AddSingleton(criteriaContext);

        services.AddScoped<IQueryCriteriaFactory, QueryCriteriaFactory>();
        services.AddScoped<IOrderLambdaFactory, OrderLambdaFactory>();
    }
    
    private static void AddSpecification(this IServiceCollection services)
    {
        var specificationTypes = CoreEngine.LoadedAssemblies.SelectMany(x => x.GetTypes())
            .Where(type => type.BaseType != null && type.BaseType.IsGenericType
                                                 && type.BaseType.GetGenericTypeDefinition() ==
                                                 typeof(BaseSpecification<,>)
                                                 && type is { IsAbstract: false, IsValueType: false, IsClass: true });

        foreach (var specificationType in specificationTypes)
        {
            services.AddScoped(specificationType.BaseType!,specificationType);
        }
    }
}