using System.Reflection;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using TechCareer.WernerHeisenberg.Survey.Application.CQRS.Exceptions;
using TechCareer.WernerHeisenberg.Survey.Application.DTOs;
using TechCareer.WernerHeisenberg.Survey.Application.Infrastructure;
using TechCareer.WernerHeisenberg.Survey.Core.Constants.Authorizes;
using TechCareer.WernerHeisenberg.Survey.Core.QueryCriterias.Models;
using TechCareer.WernerHeisenberg.Survey.Core.Security.Attributes;
using TechCareer.WernerHeisenberg.Survey.Core.Specifications.Abstract;
using TechCareer.WernerHeisenberg.Survey.Domain.Entities.Identity;

namespace TechCareer.WernerHeisenberg.Survey.Application.CQRS.Behaviours
{
    public class AuthorizationBehaviour<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
        where TRequest : IRequest<TResponse>
    {
        private readonly ILogger<TRequest> _logger;
        private readonly IWorkContext _workContext;
        private readonly IAccountService _accountService;
        private readonly BaseSpecification<ApplicationUser, UserDto> _userDtoSpecification;

        public AuthorizationBehaviour(
            ILogger<TRequest> logger,
            IWorkContext workContext,
            IAccountService accountService,
            BaseSpecification<ApplicationUser, UserDto> userDtoSpecification)
        {
            _logger = logger;
            _workContext = workContext;
            _accountService = accountService;
            _userDtoSpecification = userDtoSpecification;
        }

        public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next,
            CancellationToken cancellationToken)
        {
            var authorizeAttributes = request.GetType().GetCustomAttributes<AuthorizeAttribute>();

            var attributes = authorizeAttributes as AuthorizeAttribute[] ?? authorizeAttributes.ToArray();
            if (!attributes.Any()) return await next();


            if (_workContext.UserId == null)
            {
                throw new UnauthorizedAccessException();
            }

            var user = await _userDtoSpecification.GetQuery(new QueryCriteria()
            {
                Columns = new List<string>()
                {
                    nameof(UserDto.Roles)
                },
                SearchBy = new Dictionary<string, PropertySearch>()
                {
                    {
                        nameof(UserDto.Id),
                        new PropertySearch() { Value = _workContext.UserId.Value.ToString(), SearchType = "Equals" }
                    },
                }
            }).AsNoTracking().FirstOrDefaultAsync(cancellationToken: cancellationToken);

            if (user is null)
            {
                throw new UnauthorizeException();
            }

            if (user.Roles.Any(
                    x => x.Name is RoleNamesConstants.Admin or RoleNamesConstants.Devoloper))
            {
                return await next();
            }

            var authorizeAttributesWithRoles = attributes.Where(a => !string.IsNullOrWhiteSpace(a.Roles));


            var attributesWithRoles = authorizeAttributesWithRoles as AuthorizeAttribute[] ??
                                      authorizeAttributesWithRoles.ToArray();

            if (attributesWithRoles.Any())
            {
                var requiredRoles = attributesWithRoles.Select(a =>
                    a.Roles.Split(',').Select(x => x.Trim()).Where(x => !string.IsNullOrEmpty(x))).ToList();
                if (user.Roles.Any(x => requiredRoles.Any(y => y.Contains(x.Name))))
                {
                    _logger.LogInformation("Authorization Request: {@UserId} {@Request}", _workContext.UserId,
                        request);
                    throw new ForbiddenAccessException();
                }
            }

            return await next();
        }
    }
}