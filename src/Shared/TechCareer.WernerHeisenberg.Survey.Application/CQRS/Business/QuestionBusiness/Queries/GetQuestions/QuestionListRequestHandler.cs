using TechCareer.WernerHeisenberg.Survey.Application.CQRS.Wrappers;
using TechCareer.WernerHeisenberg.Survey.Application.DTOs;
using TechCareer.WernerHeisenberg.Survey.Application.Models.Responses;
using TechCareer.WernerHeisenberg.Survey.Core.Paginations.Interfaces;
using TechCareer.WernerHeisenberg.Survey.Core.Persistence;
using TechCareer.WernerHeisenberg.Survey.Domain.Entities.Identity;
using TechCareer.WernerHeisenberg.Survey.Domain.Entities.QuestionPool;

namespace TechCareer.WernerHeisenberg.Survey.Application.CQRS.Business.QuestionBusiness.Queries.GetQuestions;

public class QuestionListRequestHandler: IRequestHandlerWrapper<QuestionsListRequestModel,IPaginate<QuestionDto>>
{
    private readonly IApplicationDbContext _dbContext;

    public QuestionListRequestHandler(
        IApplicationDbContext dbContext
        )
    {
        _dbContext = dbContext;
    }
    public async Task<ServiceResponse<IPaginate<QuestionDto>>> Handle(QuestionsListRequestModel request, CancellationToken cancellationToken)
    {
        var serviceResponse= new ServiceResponse<IPaginate<QuestionDto>>();
        
        serviceResponse.Data= await _dbContext.GetPaginatedListAsync<Question, QuestionDto>(request,
            cancellationToken: cancellationToken);
        
        return serviceResponse;
    }
}