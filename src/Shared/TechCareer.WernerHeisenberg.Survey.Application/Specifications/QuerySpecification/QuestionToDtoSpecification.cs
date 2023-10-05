using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using TechCareer.WernerHeisenberg.Survey.Application.DTOs;
using TechCareer.WernerHeisenberg.Survey.Core.Persistence;
using TechCareer.WernerHeisenberg.Survey.Core.QueryCriterias.Models;
using TechCareer.WernerHeisenberg.Survey.Core.Specifications.Abstract;
using TechCareer.WernerHeisenberg.Survey.Core.Specifications.Extensions;
using TechCareer.WernerHeisenberg.Survey.Domain.Entities.QuestionPool;

namespace TechCareer.WernerHeisenberg.Survey.Application.Specifications.QuerySpecification;

public class QuestionToDtoSpecification: BaseSpecification<Question,QuestionDto>
{
    public QuestionToDtoSpecification(IApplicationDbContext context,
        
        IServiceProvider serviceProvider) : base(context)
    {
        MapProperty(x=>x.Id,x=>x.Id);
        MapProperty(x=>x.Text,x=>x.Text);
        MapProperty(x=>x.QuestionType,x=>x.QuestionType);
        MapProperty(x=>x.AnswerType,x=>x.AnswerType);
        MapProperty(x=>x.IsPublic,x=>x.IsPublic);
        MapProperty(x=>x.Deleted,x=>x.Deleted);

        this.CreateLeftJoin((ctx, criteria, cols) =>
            {
                 var questionAnswerDto =
                    serviceProvider.GetRequiredService<BaseSpecification<QuestionAnswer, QuestionAnswerDto>>();
            return questionAnswerDto.GetQuery(new QueryCriteria()
            {
                Columns = typeof(QuestionAnswerDto).GetProperties().Select(x=>x.Name).Union(new []{nameof(QuestionAnswerDto.Id)}).Where(x=>x!= nameof(QuestionAnswerDto.QuestionDto)).ToList()
            }).GroupBy(x=>x.QuestionId).Select(x=>new QuestionAnswerDtoHelperDto
            {
                
                QuestionId = x.Key,
                Answers= x.ToList()
            }).AsNoTracking();
        }).MapRelationKey(x=>x.Id,x=>x.QuestionId)
            .MapFieldsFromViewToRelationalView(x=>x.Answers,x=>x.Answers);
            
    }

    public override IOrderedQueryable<QuestionDto> DefaultOrder<TKey>(IQueryable<QuestionDto> query, Expression<Func<QuestionDto, TKey>> orderExpression)
    {
        return query.OrderBy(orderExpression);
    }

    public override Expression<Func<QuestionDto, object>> DefaultOrderExpression()
    {
        return x => x.Id;
    }
}


public class QuestionAnswerDtoHelperDto
{
    public int QuestionId { get; set; }
    public List<QuestionAnswerDto> Answers { get; set; }
}

