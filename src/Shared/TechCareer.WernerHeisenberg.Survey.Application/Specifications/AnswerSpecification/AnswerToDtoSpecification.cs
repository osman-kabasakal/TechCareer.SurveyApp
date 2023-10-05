using System.Linq.Expressions;
using Microsoft.Extensions.DependencyInjection;
using TechCareer.WernerHeisenberg.Survey.Application.DTOs;
using TechCareer.WernerHeisenberg.Survey.Core.Persistence;
using TechCareer.WernerHeisenberg.Survey.Core.QueryCriterias.Models;
using TechCareer.WernerHeisenberg.Survey.Core.Specifications.Abstract;
using TechCareer.WernerHeisenberg.Survey.Core.Specifications.Extensions;
using TechCareer.WernerHeisenberg.Survey.Domain.Entities.QuestionPool;

namespace TechCareer.WernerHeisenberg.Survey.Application.Specifications.AnswerSpecification;

public class AnswerToDtoSpecification: BaseSpecification<QuestionAnswer,QuestionAnswerDto>
{
    public AnswerToDtoSpecification(IApplicationDbContext context,
        IServiceProvider serviceProvider
        ) : base(context)
    {
        MapProperty(x=>x.Id,x=>x.Id);
        MapProperty(x=>x.QuestionId,x=>x.QuestionId);
        MapProperty(x=>x.DisplayOrder,x=>x.DisplayOrder);
        MapProperty(x=>x.Text,x=>x.Text);
        MapProperty(x=>x.CreatorUserId,x=>x.CreatorUserId);
        MapProperty(x=>x.CreateDate,x=>x.CreateDate);
        MapProperty(x=>x.ModifierUserId,x=>x.ModifierUserId);
        MapProperty(x=>x.ModifyDate,x=>x.ModifyDate);
        MapProperty(x=>x.Deleted,x=>x.Deleted);
        this.CreateLeftJoin((context, criteria, columns) =>
            {
                var questionDtoSpecification =
                    serviceProvider.GetRequiredService<BaseSpecification<Question, QuestionDto>>();
                return questionDtoSpecification.GetQuery(new QueryCriteria()
                {
                    Columns = columns.Where(x=>x!=nameof(QuestionDto.Answers)).ToList()
                });

            }).MapRelationKey(x=>x.QuestionId,x=>x.Id)
            .MapFieldsFromViewToRelationalView(x=>x.QuestionDto,x=>$"{x}");
    }

    public override IOrderedQueryable<QuestionAnswerDto> DefaultOrder<TKey>(IQueryable<QuestionAnswerDto> query, Expression<Func<QuestionAnswerDto, TKey>> orderExpression)
    {
        return query.OrderByDescending(orderExpression);
    }

    public override Expression<Func<QuestionAnswerDto, object>> DefaultOrderExpression()
    {
        return x => x.Id;
    }
}

public class QuestionDtoHelperDto

{
    public int QuestionId { get; set; }
    public List<QuestionDto> Questions { get; set; }
}