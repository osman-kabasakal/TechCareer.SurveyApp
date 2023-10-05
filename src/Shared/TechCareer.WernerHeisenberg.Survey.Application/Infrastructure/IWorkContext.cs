using TechCareer.WernerHeisenberg.Survey.Domain.Entities.Identity;

namespace TechCareer.WernerHeisenberg.Survey.Application.Infrastructure;

public interface IWorkContext
{
    int? UserId { get; }
}