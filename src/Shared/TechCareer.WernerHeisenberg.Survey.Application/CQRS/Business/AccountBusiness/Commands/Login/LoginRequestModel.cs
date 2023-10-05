using TechCareer.WernerHeisenberg.Survey.Application.CQRS.Wrappers;

namespace TechCareer.WernerHeisenberg.Survey.Application.CQRS.Business.AccountBusiness.Commands.Login;

public class LoginRequestModel: IRequestWrapper<bool>
{
    public string Email { get; set; }
    public string Password { get; set; }
    public bool RememberMe { get; set; }
}
