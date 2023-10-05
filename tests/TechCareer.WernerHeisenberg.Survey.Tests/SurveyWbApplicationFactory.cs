using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using TechCareer.WernerHeisenberg.Survey.Infrastructure.Persistence.EFCore;

namespace TechCareer.WernerHeisenberg.Survey.Tests;

public class SurveyWbApplicationFactory: WebApplicationFactory<Survey.Web.Program>
{
    protected override IHostBuilder? CreateHostBuilder()
    {
        var builder = base.CreateHostBuilder();
        
        builder?.ConfigureAppConfiguration((c) =>
        {
            c.AddJsonFile($"{AppDomain.CurrentDomain.BaseDirectory}/appsettings.json");
        });
        
        return builder;
    }

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.ConfigureAppConfiguration((context, configurationBuilder) =>
        {
            configurationBuilder.AddJsonFile($"{AppDomain.CurrentDomain.BaseDirectory}/appsettings.json");
        });
        base.ConfigureWebHost(builder);
    }

    public override ValueTask DisposeAsync()
    {
        var dbContext= Services.CreateScope().ServiceProvider.GetRequiredService<EfApplicationDbContext>();
        dbContext.Database.EnsureDeleted();
        return base.DisposeAsync();
    }
}