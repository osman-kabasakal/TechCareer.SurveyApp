using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Razor;
using Microsoft.EntityFrameworkCore;
using TechCareer.WernerHeisenberg.Survey.Core;
using TechCareer.WernerHeisenberg.Survey.Core.Persistence;
using TechCareer.WernerHeisenberg.Survey.Infrastructure;
using TechCareer.WernerHeisenberg.Survey.Infrastructure.Persistence.EFCore;
using Vite.AspNetCore.Extensions;


var builder = WebApplication.CreateBuilder(args);

builder.Configuration.AddEnvironmentVariables();

builder.Services.AddViteServices();

builder.Services.AddHttpContextAccessor();
builder.Services.AddInfrastructureDependencies(builder.Configuration);

builder.Services.AddDatabaseDeveloperPageExceptionFilter();



builder.Services.AddRazorPages();

var mvcBuilder = builder.Services.AddControllersWithViews(opt =>
{
    if (builder.Environment.IsDevelopment())
    {
        opt.Filters.Add(new ResponseCacheAttribute()
        {
            NoStore = true,
            Duration = 0,
            Location = ResponseCacheLocation.None
        });
    }
});

if (builder.Environment.IsDevelopment())
{
    mvcBuilder.AddRazorRuntimeCompilation();    
}


var app = builder.Build();
CoreEngine.SetServiceProvider(app.Services);
using (var scope = CoreEngine.GetScope())
{
    var context = (EfApplicationDbContext)scope.ServiceProvider.GetRequiredService<IApplicationDbContext>();
    // context.Database
    await context.Database.EnsureDeletedAsync();
    // await context.Database.EnsureCreatedAsync();
    await context.Database.MigrateAsync();
    var seedManager = scope.ServiceProvider.GetRequiredService<IDbContextSeedService>();
    await seedManager.ContextSeedAsync();
}


// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseMigrationsEndPoint();
}
else
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name : "default",
    pattern : "{controller=Home}/{action=Index}/{id?}"
);

app.UseResponseCaching();
app.MapRazorPages();


if (app.Environment.IsDevelopment())
{
    // Use Vite Dev Server as middleware.
    app.UseViteDevMiddleware();
}

app.Run();

namespace TechCareer.WernerHeisenberg.Survey.Web
{
    public class Program
    {
    }
}