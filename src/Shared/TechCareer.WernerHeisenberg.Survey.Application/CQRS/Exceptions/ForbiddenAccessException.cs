using TechCareer.WernerHeisenberg.Survey.Core.Constants.Exceptions;

namespace TechCareer.WernerHeisenberg.Survey.Application.CQRS.Exceptions
{
    public class ForbiddenAccessException : Exception
    {
        public ForbiddenAccessException() : base(ExceptionMessagesConstants.ForbiddenExceptionMessage) { }
    }
}
