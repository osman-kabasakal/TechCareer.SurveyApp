using System.Linq.Expressions;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using TechCareer.WernerHeisenberg.Survey.Application.DTOs;
using TechCareer.WernerHeisenberg.Survey.Core.Persistence;
using TechCareer.WernerHeisenberg.Survey.Core.QueryCriterias.Models;
using TechCareer.WernerHeisenberg.Survey.Core.Specifications.Abstract;
using TechCareer.WernerHeisenberg.Survey.Core.Specifications.Extensions;
using TechCareer.WernerHeisenberg.Survey.Domain.Entities.Identity;

namespace TechCareer.WernerHeisenberg.Survey.Application.Specifications.Roles;

public class RolesToDtoSpecification : BaseSpecification<ApplicationRole, RoleDto>
{
    public class AssignedUserHelperDto
    {
        public int RoleId { get; set; }
        public List<UserDto> userDto { get; set; }

        public AssignedUserHelperDto()
        {
        }
    }

    public RolesToDtoSpecification(IApplicationDbContext context,
        IServiceProvider serviceProvider
    ) : base(context)
    {
        MapProperty(x => x.Id, x => x.Id);
        MapProperty(x => x.Name, x => x.Name);
        MapProperty(x=>x.Deleted,x=>x.Deleted);
        MapProperty(x=>x.SystemRole,x=>x.SystemRole);
        this.CreateLeftJoin((ctx, cr, cols) =>
            {
                var userRolesTable = ctx.GetTable<IdentityUserRole<int>>();
                var usersTable = ctx.GetTable<ApplicationUser>();
                return (
                    from userRole in userRolesTable.GroupBy(x=>x.RoleId)
                    select new AssignedUserHelperDto()
                    {
                        RoleId = userRole.Key,
                        userDto = usersTable.Where(r=>userRolesTable.Any(c=>c.UserId==r.Id)).Select(x => new UserDto()
                        {
                            Id = x.Id,
                            UserName = x.UserName,
                            Email = x.Email,
                            PhoneNumber = x.PhoneNumber,
                            Deleted = x.Deleted,
                            Firstname = x.Firstname,
                            Lastname = x.Lastname,
                            SystemUser = x.SystemUser
                        }).ToList()
                    });
            }).MapRelationKey(x => x.Id, x => x.RoleId)
            .MapFieldsFromViewToRelationalView(x => x.AssignedUsers, x => x.userDto);
        ;
    }


    public override IOrderedQueryable<RoleDto> DefaultOrder<TKey>(IQueryable<RoleDto> query, Expression<Func<RoleDto, TKey>> orderExpression)
    {
        return query.OrderByDescending(orderExpression);
    }

    public override Expression<Func<RoleDto, object>> DefaultOrderExpression()
    {
        return x => x.Id;
    }
}