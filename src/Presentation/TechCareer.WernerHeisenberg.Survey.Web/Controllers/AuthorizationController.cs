using MediatR;
using Microsoft.AspNetCore.Mvc;
using TechCareer.WernerHeisenberg.Survey.Application.CQRS.Business.AccountBusiness.Commands.Login;

namespace TechCareer.WernerHeisenberg.Survey.Web.Controllers;

public class AuthorizationController : Controller
{
    private readonly IMediator _mediator;

    public AuthorizationController(
        IMediator mediator
        )
    {
        _mediator = mediator;
    }
    
    [HttpGet("login")]
    public IActionResult Login()
    {
        return View();
    }
    
    [HttpPost("login")]
    public async Task<IActionResult> Login(LoginRequestModel requestModel, string returnUrl)
    {
        var result = await _mediator.Send(requestModel);
        if (result.IsSuccessful && result.Data)
        {
            return Redirect(returnUrl??Url.Action("Index", "Dashboard"));
        }
        requestModel.Password= string.Empty;
        ViewBag.ServiceResponse = result;
        return View(requestModel);
    }

    public IActionResult Logout(string returnUrl)
    {
        throw new NotImplementedException();
    }

    public IActionResult Register(string returnUrl)
    {
        throw new NotImplementedException();
    }
}