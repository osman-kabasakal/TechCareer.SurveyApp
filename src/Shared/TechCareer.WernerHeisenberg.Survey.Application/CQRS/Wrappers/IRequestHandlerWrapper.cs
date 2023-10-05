using MediatR;
using TechCareer.WernerHeisenberg.Survey.Application.Models.Responses;

namespace TechCareer.WernerHeisenberg.Survey.Application.CQRS.Wrappers;

public interface
    IRequestHandlerWrapper<TRequest, TResponseType> : IRequestHandler<TRequest, ServiceResponse<TResponseType>>
    where TRequest : IRequestWrapper<TResponseType>
{
}