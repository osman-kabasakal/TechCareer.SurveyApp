using TechCareer.WernerHeisenberg.Survey.Application.CQRS.Wrappers;
using TechCareer.WernerHeisenberg.Survey.Application.Infrastructure;
using TechCareer.WernerHeisenberg.Survey.Application.Models.Responses;

namespace TechCareer.WernerHeisenberg.Survey.Application.CQRS.Business.AccountBusiness.Commands.Login;

public class LoginRequestHandler: IRequestHandlerWrapper<LoginRequestModel,bool>
{
    private readonly IAccountService _accountService;

    public LoginRequestHandler(
        IAccountService accountService
        )
    {
        _accountService = accountService;
    }
    public async Task<ServiceResponse<bool>> Handle(LoginRequestModel request, CancellationToken cancellationToken)
    {
        return await _accountService.PasswordSignInAsync(request);
    }
}