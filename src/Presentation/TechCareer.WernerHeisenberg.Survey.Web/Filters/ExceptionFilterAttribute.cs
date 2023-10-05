using Microsoft.AspNetCore.Mvc.Filters;
using TechCareer.WernerHeisenberg.Survey.Application.CQRS.Exceptions;

namespace TechCareer.WernerHeisenberg.Survey.Web.Filters;

public class WebExceptionFilterAttribute: ExceptionFilterAttribute
{
    private readonly IDictionary<Type, Action<ExceptionContext>> _exceptionHandlers;
    
    public WebExceptionFilterAttribute()
    {
        // _exceptionHandlers = new Dictionary<Type, Action<ExceptionContext>>
        // {
        //     { typeof(ValidationException), HandleValidationException },
        //     { typeof(NotFoundException), HandleNotFoundException },
        //     { typeof(UnauthorizedAccessException), HandleUnauthorizedAccessException },
        //     { typeof(ForbiddenAccessException), HandleForbiddenAccessException },
        // };
    }

    public override void OnException(ExceptionContext context)
    {
        base.OnException(context);
    }
}