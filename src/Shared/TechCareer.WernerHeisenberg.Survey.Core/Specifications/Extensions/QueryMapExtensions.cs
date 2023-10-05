using System.Linq.Expressions;
using System.Reflection;
using TechCareer.WernerHeisenberg.Survey.Core.ApplicationExtesions;
using TechCareer.WernerHeisenberg.Survey.Core.Persistence;
using TechCareer.WernerHeisenberg.Survey.Core.Persistence.EntityTypes;
using TechCareer.WernerHeisenberg.Survey.Core.QueryCriterias.Models;
using TechCareer.WernerHeisenberg.Survey.Core.Specifications.Abstract;
using TechCareer.WernerHeisenberg.Survey.Core.Specifications.Models;

namespace TechCareer.WernerHeisenberg.Survey.Core.Specifications.Extensions;

public static class QueryMapExtensions
{
    public static CreateJoinQueryModel<TEntity, TView, TJoinView> CreateLeftJoin<TEntity, TView, TJoinView>(this BaseSpecification<TEntity, TView> instance, Func<IApplicationDbContext, QueryCriteria, IList<string>,  IQueryable<TJoinView>> joinExpression)
        where TEntity : class,IEntity, new()
        where TView : class, new()
        where TJoinView : class, new()
        {
            var joinId = Guid.NewGuid().ToString();
            var model = new JoinMap<TEntity,TView, object>()
            {
                JoinQueryBuilder = joinExpression,
                RelationalKeyMemberInfo = new List<MemberInfo>() {  },
                EntityKeyMemberInfo = new List<MemberInfo>() { },
                JonStrategy = JoinType.Left ,
                RelationalKeyDeclaringType = typeof(TJoinView),
                Id=joinId,
            };
            instance.Joins.Add(joinId, model);

            return new CreateJoinQueryModel<TEntity, TView, TJoinView>()
            {
                MapConfiguration = model,
                Specification= instance
            };
        }

        public static CreateJoinQueryModel<TEntity, TView, TJoinView>  MapRelationKey<TEntity, TView, TJoinView,TKeyType>(this CreateJoinQueryModel<TEntity, TView, TJoinView> joinInstance, Expression<Func<TEntity, TKeyType>> entityKey, Expression<Func<TJoinView, TKeyType>> relationalKey)
        where TEntity : class,IEntity, new()
        where TView : class, new()
        where TJoinView : class, new()

        {
            joinInstance.MapConfiguration.RelationalKeyMemberInfo.Add(relationalKey.FindProperty());
            joinInstance.MapConfiguration.EntityKeyMemberInfo.Add(entityKey.FindProperty());

            return joinInstance;
        }

        public static CreateJoinQueryModel<TEntity, TView, TJoinView> MapFieldsFromViewToRelationalView<TEntity, TView, TJoinView, TKeyType>(this CreateJoinQueryModel<TEntity, TView, TJoinView> joinInstance, Expression<Func<TView, TKeyType>> ViewFieldExpression, Expression<Func<TJoinView, TKeyType>> joinFieldExpression )
        where TEntity : class,IEntity, new()
        where TView : class, new()
        where TJoinView : class, new()

        {
            var viewMember = ViewFieldExpression.FindProperty();
            var key = viewMember.Name;
            var memberMap = new MemberMap()
            {
                IsJoinSource = true,
                JoinId = joinInstance.MapConfiguration.Id,
                EntityMemberInfo = joinFieldExpression.FindProperty(),
                ViewMemberInfo = viewMember
            };
            joinInstance.Specification.ViewMaps[key] = memberMap;

            return joinInstance;
        }

        public static CreateJoinQueryModel<TEntity, TView, TJoinView> MapFieldsFromViewToRelationalView<TEntity, TView, TJoinView, TKeyType>(this CreateJoinQueryModel<TEntity, TView, TJoinView> joinInstance, Expression<Func<TView, TKeyType>> ViewFieldExpression, Func<string, string> joinFieldSelectorClause)
        where TEntity : class, IEntity,new()
        where TView : class, new()
        where TJoinView : class, new()

        {
            var viewMember = ViewFieldExpression.FindProperty();
            var key = viewMember.Name;
            var memberMap = new MemberMap()
            {
                IsJoinSource = true,
                JoinId = joinInstance.MapConfiguration.Id,
                EntityMemberInfo = viewMember,
                ViewMemberInfo = viewMember   ,
                IsSelectorClause=true,
                SelectorClause = joinFieldSelectorClause
            };
            joinInstance.Specification.ViewMaps[key] = memberMap;

            return joinInstance;
        }


        public static CreateJoinQueryModel<TEntity, TView, TJoinView> CreateInnerJoin<TEntity, TView, TJoinView>(this BaseSpecification<TEntity, TView> instance, Func<IApplicationDbContext, QueryCriteria, IList<string>, IQueryable<TJoinView>> joinExpression)
        where TEntity : class,IEntity, new()
        where TView : class, new()
        where TJoinView : class, new()
        {
            var joinId = Guid.NewGuid().ToString();
            var model = new JoinMap<TEntity, TView, object>()
            {
                JoinQueryBuilder = joinExpression,
                RelationalKeyMemberInfo = new List<MemberInfo>() { },
                EntityKeyMemberInfo = new List<MemberInfo>() { },
                JonStrategy = JoinType.Inner,
                RelationalKeyDeclaringType = typeof(TJoinView),
                Id = joinId,

            };
            instance.Joins.Add(joinId, model);

            return new CreateJoinQueryModel<TEntity, TView, TJoinView>()
            {
                MapConfiguration = model,
                Specification = instance

            };
        }
}