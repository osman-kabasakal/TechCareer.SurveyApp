using TechCareer.WernerHeisenberg.Survey.Application.DTOs;
using TechCareer.WernerHeisenberg.Survey.Core.QueryCriterias.Models;
using TechCareer.WernerHeisenberg.Survey.Domain.Enums;
using TechCareer.WernerHeisenberg.Survey.Infrastructure.QueryCriterias;

namespace TechCareer.WernerHeisenberg.Survey.Tests.UnitTests.Core.Persistence.TheoryDatas;

public class QueryDtoCriteriaDatas: TheoryData<QueryCriteria>
{
    public QueryDtoCriteriaDatas()
    {
        Add(new QueryCriteria()
        {
            
        });
        
        
        Add(new QueryCriteria()
        {
            Columns = new List<string>()
            {
                nameof(QuestionDto.IsPublic),
                nameof(QuestionDto.QuestionType),
                nameof(QuestionDto.AnswerType),
                nameof(QuestionDto.Text)
            },
            SearchBy = new Dictionary<string, PropertySearch>()
            {
                {nameof(QuestionDto.QuestionType), new PropertySearch()
                {
                    SearchType = CriteriaTypeKeys.Equals,
                    Value = QuestionTypes.Text.ToString()
                }}
            }
        });
        
        
        Add(new QueryCriteria()
        {
            Columns = new List<string>()
            {
                nameof(QuestionDto.IsPublic),
                nameof(QuestionDto.QuestionType),
                nameof(QuestionDto.AnswerType),
                nameof(QuestionDto.Text)
            },
            SearchBy = new Dictionary<string, PropertySearch>()
            {
                {nameof(QuestionDto.QuestionType), new PropertySearch()
                {
                    SearchType = CriteriaTypeKeys.NotEquals,
                    Value = QuestionTypes.Text.ToString()
                }}
            }
        });
    }
}