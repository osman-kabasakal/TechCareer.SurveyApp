using System.Reflection;
using FluentValidation;
using MediatR;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using TechCareer.WernerHeisenberg.Survey.Application.CQRS.Behaviours;
using TechCareer.WernerHeisenberg.Survey.Application.CQRS.Wrappers;
using TechCareer.WernerHeisenberg.Survey.Core;
using TechCareer.WernerHeisenberg.Survey.Core.AppSettings;

namespace TechCareer.WernerHeisenberg.Survey.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplicationDependencies(this IServiceCollection services, ConfigurationManager configuration)
    {
        services.AddCoreDependencies(configuration);


        services.AddMediatR((mediatRServiceConfiguration) =>
        {
            mediatRServiceConfiguration.RegisterServicesFromAssemblyContaining(typeof(IRequestWrapper<>));
            mediatRServiceConfiguration.AddBehavior(typeof(IPipelineBehavior<,>),
                typeof(UnhandledExceptionBehaviour<,>));
            mediatRServiceConfiguration.AddBehavior(typeof(IPipelineBehavior<,>), typeof(AuthorizationBehaviour<,>));
            mediatRServiceConfiguration.AddBehavior(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));
        } );

        services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());
        return services;
    }
}