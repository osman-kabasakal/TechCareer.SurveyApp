using System.Linq.Expressions;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using TechCareer.WernerHeisenberg.Survey.Application.DTOs;
using TechCareer.WernerHeisenberg.Survey.Core.Persistence;
using TechCareer.WernerHeisenberg.Survey.Core.QueryCriterias.Models;
using TechCareer.WernerHeisenberg.Survey.Core.Specifications.Abstract;
using TechCareer.WernerHeisenberg.Survey.Core.Specifications.Extensions;
using TechCareer.WernerHeisenberg.Survey.Domain.Entities.Identity;

namespace TechCareer.WernerHeisenberg.Survey.Application.Specifications.UserSpecification;

public class UserToDtoSpecification : BaseSpecification<ApplicationUser, UserDto>
{
    public UserToDtoSpecification(IApplicationDbContext context,
        IServiceProvider serviceProvider
    ) : base(context)
    {
        MapProperty(x => x.Id, x => x.Id);
        MapProperty(x => x.UserName, x => x.UserName);
        MapProperty(x => x.Email, x => x.Email);
        MapProperty(x => x.PhoneNumber, x => x.PhoneNumber);
        MapProperty(x => x.Deleted, x => x.Deleted);
        MapProperty(x => x.Firstname, x => x.Firstname);
        MapProperty(x => x.Lastname, x => x.Lastname);
        MapProperty(x => x.SystemUser, x => x.SystemUser);

        this.CreateLeftJoin((ctx, cr, cols) =>
            {
                var userRoleTable = ctx.GetTable<IdentityUserRole<int>>();
                var roleTable = ctx.GetTable<ApplicationRole>();
                return (
                    from userRole in userRoleTable.GroupBy(x=>x.UserId)
                    select new UserRolesHelperDto()
                    {
                        UserId = userRole.Key,
                        RoleDtos = roleTable.Where(r=>userRoleTable.Any(c=>c.RoleId==r.Id)).Select(x => new RoleDto()
                        {
                            Id = x.Id,
                            Name = x.Name,
                            Deleted = x.Deleted,
                            SystemRole = x.SystemRole
                        }).ToList()
                    });
            }).MapRelationKey(x => x.Id, x => x.UserId)
            .MapFieldsFromViewToRelationalView(x => x.Roles, x => x.RoleDtos);
    }


    public override IOrderedQueryable<UserDto> DefaultOrder<TKey>(IQueryable<UserDto> query,
        Expression<Func<UserDto, TKey>> orderExpression)
    {
        return query.OrderByDescending(orderExpression);
    }

    public override Expression<Func<UserDto, object>> DefaultOrderExpression()
    {
        return x => x.Id;
    }
}

public class UserRolesHelperDto
{
    public int UserId { get; set; }
    public List<RoleDto> RoleDtos { get; set; }
}