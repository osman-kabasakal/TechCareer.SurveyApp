using MediatR;
using TechCareer.WernerHeisenberg.Survey.Application.Models.Responses;

namespace TechCareer.WernerHeisenberg.Survey.Application.CQRS.Wrappers;

public interface IRequestWrapper<T>: IRequest<ServiceResponse<T>>
{
    
}