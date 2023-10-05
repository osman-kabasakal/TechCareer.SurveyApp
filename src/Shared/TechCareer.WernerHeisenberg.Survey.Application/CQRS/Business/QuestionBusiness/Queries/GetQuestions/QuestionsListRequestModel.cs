using TechCareer.WernerHeisenberg.Survey.Application.CQRS.Wrappers;
using TechCareer.WernerHeisenberg.Survey.Application.DTOs;
using TechCareer.WernerHeisenberg.Survey.Core.Constants.Authorizes;
using TechCareer.WernerHeisenberg.Survey.Core.Paginations.Interfaces;
using TechCareer.WernerHeisenberg.Survey.Core.QueryCriterias.Models;
using TechCareer.WernerHeisenberg.Survey.Core.Security.Attributes;

namespace TechCareer.WernerHeisenberg.Survey.Application.CQRS.Business.QuestionBusiness.Queries.GetQuestions;

[Authorize(Roles = $"{RoleNamesConstants.Admin},{RoleNamesConstants.Devoloper}")]
public class QuestionsListRequestModel : QueryCriteria, IRequestWrapper<IPaginate<QuestionDto>>
{
    
}