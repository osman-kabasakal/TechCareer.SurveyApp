using Microsoft.AspNetCore.Mvc.Filters;
using TechCareer.WernerHeisenberg.Survey.Application.CQRS.Exceptions;

namespace TechCareer.WernerHeisenberg.Survey.Web.Filters;

public class WebExceptionFilterAttribute: ExceptionFilterAttribute
{
    private readonly IDictionary<Type, Action<ExceptionContext>> _exceptionHandlers;
    
    public WebExceptionFilterAttribute()
    {
    }

    public override void OnException(ExceptionContext context)
    {
        base.OnException(context);
    }
}