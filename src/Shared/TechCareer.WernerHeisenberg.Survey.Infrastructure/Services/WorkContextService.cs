using System.Security.Claims;
using Microsoft.AspNetCore.Http;
using TechCareer.WernerHeisenberg.Survey.Application.Infrastructure;

namespace TechCareer.WernerHeisenberg.Survey.Infrastructure.Services;

public class WorkContextService : IWorkContext
{
    private readonly IHttpContextAccessor _httpContextAccessor;

    public WorkContextService(
        IHttpContextAccessor httpContextAccessor
        )
    {
        _httpContextAccessor = httpContextAccessor;
    }

    public int? UserId
    {
        get
        {
            var userId = _httpContextAccessor.HttpContext?.User?.FindFirstValue(ClaimTypes.NameIdentifier);
            return int.TryParse(userId, out var id) ? id : (int?)null;
        }
    }
}