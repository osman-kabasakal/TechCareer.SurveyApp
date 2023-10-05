using TechCareer.WernerHeisenberg.Survey.Application.CQRS.Business.AccountBusiness.Commands.Login;
using TechCareer.WernerHeisenberg.Survey.Application.DTOs;
using TechCareer.WernerHeisenberg.Survey.Application.Models.Responses;
using TechCareer.WernerHeisenberg.Survey.Domain.Entities.Identity;

namespace TechCareer.WernerHeisenberg.Survey.Application.Infrastructure;

public interface IAccountService
{
    Task<ServiceResponse<string?>> GetUserNameAsync(int userId);

    Task<ServiceResponse<bool>> CheckUserPasswordAsync(string email, string password);
    
    Task<ServiceResponse<bool>> PasswordSignInAsync(LoginRequestModel loginRequestModel);

    Task<ServiceResponse<int>> CreateUserAsync(string userName, string password);

    Task<ServiceResponse<bool>> UserIsInRoleAsync(int userId, string role);

    Task<ServiceResponse<bool>> DeleteUserAsync(string userId);
}