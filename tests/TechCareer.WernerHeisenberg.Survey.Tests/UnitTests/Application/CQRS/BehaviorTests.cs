using MediatR;
using Microsoft.Extensions.DependencyInjection;
using TechCareer.WernerHeisenberg.Survey.Application.CQRS.Exceptions;
using Xunit.Abstractions;

namespace TechCareer.WernerHeisenberg.Survey.Tests.UnitTests.Application.CQRS;

public class BehaviorTests: BaseTest
{
    private readonly IMediator _mediator;

    public BehaviorTests(SurveyWbApplicationFactory webApplicationFactory, ITestOutputHelper testOutputHelper) : base(webApplicationFactory, testOutputHelper)
    {
        _mediator = _services.GetRequiredService<IMediator>();
    }
    
    
    [Fact]
    public async Task UnauthorizeException_ShouldBeThrown()
    {
        var exception = await Assert.ThrowsAsync<UnauthorizeException>(async () =>
        {
            await _mediator.Send(new ());
        });
        
        Assert.Equal("User was not found!", exception.Message);
    }
}