using System.Diagnostics;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using TechCareer.WernerHeisenberg.Survey.Core.Persistence;
using TechCareer.WernerHeisenberg.Survey.Domain.Entities.Identity;
using TechCareer.WernerHeisenberg.Survey.Infrastructure.Persistence.EFCore;
using TechCareer.WernerHeisenberg.Survey.Application;
using TechCareer.WernerHeisenberg.Survey.Application.Infrastructure;
using TechCareer.WernerHeisenberg.Survey.Core.AppSettings;
using TechCareer.WernerHeisenberg.Survey.Infrastructure.Persistence.EFCore.DbPagination;
using TechCareer.WernerHeisenberg.Survey.Infrastructure.Services;

namespace TechCareer.WernerHeisenberg.Survey.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection
        AddInfrastructureDependencies(this IServiceCollection services,
            ConfigurationManager configuration)
    {
        services.AddApplicationDependencies(configuration);

        services.AddScoped<IDbPaginateService, EfPaginationService>();
        
        services.AddDbContext<IApplicationDbContext, EfApplicationDbContext>(
            opt =>
            {
                var conStr = configuration.GetConnectionString("DefaultConnection");
                opt.UseSqlServer(conStr,
                    builder => { builder.MigrationsAssembly(typeof(EfApplicationDbContext).Assembly.FullName); });
                opt.EnableDetailedErrors();
                opt.EnableSensitiveDataLogging();
                opt.EnableServiceProviderCaching();
                opt.LogTo((s) => Debug.WriteLine(s));
            }
        );


        services.AddDataProtection()
            .PersistKeysToDbContext<EfApplicationDbContext>();

        services.AddAuthentication(o =>
            {
                o.DefaultScheme = IdentityConstants.ApplicationScheme;
                o.DefaultSignInScheme = IdentityConstants.ExternalScheme;
            })
            .AddIdentityCookies(o => { });
        
        services.AddIdentityCore<ApplicationUser>(o =>
            {
                o.Stores.MaxLengthForKeys = 128;
                o.SignIn.RequireConfirmedAccount = false;
                o.SignIn.RequireConfirmedEmail = false;
                o.SignIn.RequireConfirmedPhoneNumber = false;
                o.SignIn.RequireConfirmedAccount = true;
                o.User.RequireUniqueEmail = true;
                o.Password.RequireDigit = false;
                o.Password.RequiredLength = 8;
                o.Password.RequireLowercase = true;
                o.Password.RequireUppercase = true;
                o.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromSeconds(0);
            })
            .AddSignInManager()
            .AddDefaultTokenProviders()
            .AddRoles<ApplicationRole>()
            .AddEntityFrameworkStores<EfApplicationDbContext>();
        
        services.Configure<SeedSetting>(configuration.GetSection("SeedSetting"));

        services.AddScoped<IDbContextSeedService, ContextSeedService>();

        services.AddScoped<IAccountService, AccountService>();
        services.AddScoped<IWorkContext, WorkContextService>();

        return services;
    }
}