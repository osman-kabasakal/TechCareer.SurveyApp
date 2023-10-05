using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using TechCareer.WernerHeisenberg.Survey.Application.DTOs;
using TechCareer.WernerHeisenberg.Survey.Core.Persistence;
using TechCareer.WernerHeisenberg.Survey.Core.QueryCriterias.Models;
using TechCareer.WernerHeisenberg.Survey.Domain.Entities.Identity;
using TechCareer.WernerHeisenberg.Survey.Domain.Entities.QuestionPool;
using TechCareer.WernerHeisenberg.Survey.Tests.UnitTests.Core.Persistence.TheoryDatas;
using Xunit.Abstractions;

namespace TechCareer.WernerHeisenberg.Survey.Tests.UnitTests.Core.Persistence;



public class PersistenceUnitTests: BaseTest
{
    private readonly IApplicationDbContext _dbContext;

    public PersistenceUnitTests(SurveyWbApplicationFactory webApplicationFactory, ITestOutputHelper testOutputHelper) : base(webApplicationFactory, testOutputHelper)
    {
        _dbContext = _services.GetRequiredService<IApplicationDbContext>();
        
    }
    
    [Theory]
    [ClassData(typeof(QuestionDatas))]
    public async Task Question_ShouldBeAdded(Question question)
    {
        await _dbContext.AddAsync(question);
        await _dbContext.SaveChangesAsync();
        
        Assert.NotEqual(0, question.Id);
    }
    
    [Theory]
    [ClassData(typeof(QueryDtoCriteriaDatas))]
    public async Task GetPaginatedListAsync_Criteria_NotEmpty(QueryCriteria criteria)
    {
        var list = await _dbContext.GetPaginatedListAsync<Question,QuestionDto>(criteria);
        
        Assert.NotEmpty(list.Items);
    }
    
    [Fact]
    public async Task GetPaginatedListAsync_UserDto_NotEmpty()
    {
        var userRoles = _dbContext.GetTable<IdentityUserRole<int>>().ToList();
        
        Assert.NotEmpty(userRoles);
        var list = await _dbContext.GetPaginatedListAsync<ApplicationUser,UserDto>(new QueryCriteria()
        {
            Columns = new List<string>()
            {
                nameof(UserDto.Roles)
            },
            SearchBy = new Dictionary<string, PropertySearch>()
            {
                {
                    nameof(UserDto.Id),
                    new PropertySearch() { Value = "1", SearchType = "Equals" }
                },
            }
        });
        
        Assert.Empty(list.Items);
    }
    
}