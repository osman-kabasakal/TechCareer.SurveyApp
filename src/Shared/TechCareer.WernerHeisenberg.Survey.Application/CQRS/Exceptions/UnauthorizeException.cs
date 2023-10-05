namespace TechCareer.WernerHeisenberg.Survey.Application.CQRS.Exceptions
{
    public class UnauthorizeException : Exception
    {
        public UnauthorizeException() : base("User was not found!")
        {
            
        }
    }
}
