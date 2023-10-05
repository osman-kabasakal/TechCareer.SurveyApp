using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using Xunit.Abstractions;

namespace TechCareer.WernerHeisenberg.Survey.Tests;

public class BaseTest: IClassFixture<SurveyWbApplicationFactory>
{
    protected readonly SurveyWbApplicationFactory _webApplicationFactory;
    protected readonly ITestOutputHelper _testOutputHelper;
    protected readonly HttpClient _client;
    protected readonly IServiceProvider _services;
    
    public BaseTest(
        SurveyWbApplicationFactory webApplicationFactory, 
        ITestOutputHelper testOutputHelper
        )
    {
        _webApplicationFactory = webApplicationFactory;
        _testOutputHelper = testOutputHelper;
        _client = webApplicationFactory.CreateClient(
            new WebApplicationFactoryClientOptions {AllowAutoRedirect = true});
        _services = webApplicationFactory.Services.CreateScope().ServiceProvider;
    }
}