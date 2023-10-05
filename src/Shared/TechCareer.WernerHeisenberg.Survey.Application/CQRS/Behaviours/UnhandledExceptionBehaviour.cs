using MediatR;
using Microsoft.Extensions.Logging;
using TechCareer.WernerHeisenberg.Survey.Application.CQRS.Wrappers;
using TechCareer.WernerHeisenberg.Survey.Application.Models.Responses;

namespace TechCareer.WernerHeisenberg.Survey.Application.CQRS.Behaviours
{
    public class UnhandledExceptionBehaviour<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
        where TRequest : IRequest<TResponse>
    {
        private readonly ILogger<TRequest> _logger;

        public UnhandledExceptionBehaviour(ILogger<TRequest> logger)
        {
            _logger = logger;
        }

        public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next,CancellationToken cancellationToken)
        {
            try
            {
                return await next();
            }
            catch (Exception ex)
            {
                var requestName = typeof(TRequest).Name;

                _logger.LogError(ex, "Unhandled Exception for Request {Name} {@Request}", requestName, request);

                try
                {
                    var response = Activator.CreateInstance<TResponse>() as IServiceResponse;
                    if (response is null)
                    {
                        throw ex;
                    }

                    response.ExceptionMessage = ex.Message;
                    response.HasExceptionError = true;
                    response.IsSuccessful = true;
                    return (TResponse)response;
                }
                catch (Exception e)
                {
                    throw;
                }
            }
        }

    }
}
