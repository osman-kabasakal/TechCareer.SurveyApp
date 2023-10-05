using Microsoft.AspNetCore.Identity;
using TechCareer.WernerHeisenberg.Survey.Application.CQRS.Business.AccountBusiness.Commands.Login;
using TechCareer.WernerHeisenberg.Survey.Application.CQRS.Exceptions;
using TechCareer.WernerHeisenberg.Survey.Application.DTOs;
using TechCareer.WernerHeisenberg.Survey.Application.Infrastructure;
using TechCareer.WernerHeisenberg.Survey.Application.Models.Responses;
using TechCareer.WernerHeisenberg.Survey.Domain.Entities.Identity;

namespace TechCareer.WernerHeisenberg.Survey.Infrastructure.Services;

public class AccountService: IAccountService
{
    private readonly SignInManager<ApplicationUser> _signInManager;
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly RoleManager<ApplicationRole> _roleManager;

    public AccountService(
        SignInManager<ApplicationUser> signInManager,
        UserManager<ApplicationUser> userManager,
        RoleManager<ApplicationRole> roleManager
        )
    {
        _signInManager = signInManager;
        _userManager = userManager;
        _roleManager = roleManager;
    }
    
    public async Task<ServiceResponse<string?>> GetUserNameAsync(int userId)
    {
        var user = await _userManager.FindByIdAsync(userId.ToString());
        if (user is null)
        {
            throw new NotFoundException(nameof(ApplicationUser), userId.ToString());
        }
        return new ServiceResponse<string?>()
        {
            Data = await _userManager.GetUserNameAsync(user)
        };
    }

    public async Task<ServiceResponse<bool>> CheckUserPasswordAsync(string email, string password)
    {
        var user = await _userManager.FindByEmailAsync(email);
        if (user is null)
        {
            throw new NotFoundException(nameof(ApplicationUser), email);
        }
        
        var result = await _userManager.CheckPasswordAsync(user, password);

        return new ServiceResponse<bool>()
        {
            Data = result
        };
    }

    public async Task<ServiceResponse<bool>> PasswordSignInAsync(LoginRequestModel loginRequestModel)
    {
        var result = await _signInManager.PasswordSignInAsync(loginRequestModel.Email, loginRequestModel.Password, loginRequestModel.RememberMe, false);
        if (result.Succeeded)
        {
            return new ServiceResponse<bool>()
            {
                Data = true
            };
        }

        throw new NotFoundException("User", "Email or Password is wrong");
    }

    public Task<ServiceResponse<int>> CreateUserAsync(string userName, string password)
    {
        throw new NotImplementedException();
    }

    public Task<ServiceResponse<bool>> UserIsInRoleAsync(int userId, string role)
    {
        throw new NotImplementedException();
    }

    public Task<ServiceResponse<bool>> DeleteUserAsync(string userId)
    {
        throw new NotImplementedException();
    }
}