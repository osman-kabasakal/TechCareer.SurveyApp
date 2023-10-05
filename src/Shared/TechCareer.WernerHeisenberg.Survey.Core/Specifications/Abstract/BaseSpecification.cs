using System.Linq.Dynamic.Core;
using System.Linq.Expressions;
using System.Reflection;
using TechCareer.WernerHeisenberg.Survey.Core.ApplicationExtesions;
using TechCareer.WernerHeisenberg.Survey.Core.Persistence;
using TechCareer.WernerHeisenberg.Survey.Core.Persistence.EntityTypes;
using TechCareer.WernerHeisenberg.Survey.Core.QueryCriterias.Models;
using TechCareer.WernerHeisenberg.Survey.Core.Specifications.Collections;
using TechCareer.WernerHeisenberg.Survey.Core.Specifications.Models;

namespace TechCareer.WernerHeisenberg.Survey.Core.Specifications.Abstract;


public abstract class BaseSpecification<TEntity, TView>
    where TEntity : class,IEntity, new()
    where TView : class, new()

{

    public BaseSpecification(IApplicationDbContext context)
    {
        Context = context;
        ViewMaps = new Dictionary<string, MemberMap>();
        Joins = new Dictionary<string, JoinMap<TEntity, TView, object>>();
    }

    public Dictionary<string, MemberMap> ViewMaps { get; set; }
    internal Dictionary<string, JoinMap<TEntity, TView, object>> Joins { get; set; }
    public IApplicationDbContext Context { get; }
    
    public abstract IOrderedQueryable<TView> DefaultOrder<TKey>(IQueryable<TView> query,Expression<Func<TView, TKey>> orderExpression);

    public abstract Expression<Func<TView, object>> DefaultOrderExpression();

    /// <summary>
    /// View özelliğini veritabnı nesnesi özelliği ile eşler
    /// </summary>
    /// <example language="c#">
    /// <code>
    ///  MapProperty(TView=>TView.Prop,TEntity=>TEnity.Prop)
    /// </code>
    /// </example>
    /// <param name="fromView">View özelliğini döndürür <see cref="MemberExpression"/></param>
    /// <param name="fromEntity">Veri tabnı özelliğini döndürür <see cref="MemberExpression"/></param>
    public void MapProperty(Expression<Func<TView, object>> fromView, Expression<Func<TEntity, object>> fromEntity)
    {
        var viewMember = fromView.FindProperty();
        var entityMembr = fromEntity.FindProperty();

        ViewMaps.Add(viewMember.Name, new MemberMap()
        {
            IsJoinSource = false,
            EntityMemberInfo = entityMembr,
            ViewMemberInfo = viewMember
        });
    }

    public void MapProperty(Expression<Func<TView, object>> fromView, Func<string, string> fromEntity)
    {
        if (fromEntity is null)
        {
            throw new ArgumentNullException(nameof(fromEntity));
        }

        var viewMember = fromView.FindProperty();

        ViewMaps.Add(viewMember.Name, new MemberMap()
        {
            IsJoinSource = false,
            EntityMemberInfo = viewMember,
            ViewMemberInfo = viewMember,
            IsSelectorClause = true,
            SelectorClause = fromEntity
        });
    }

    /// <summary>View özelliklerinin başka bir sorgudan birden fazla anahtar alana göre birleştirilerek (<see cref="IQueryable{T}.Join" />) eşleştirilmesi gereken durumlarda kullanılır</summary>
    /// <typeparam name="TJoinView">Birleştirilen sorgunun temsil ettiği tiptir</typeparam>
    /// <param name="jointype">Birleştirilecek sorgunun kesişim olarak veya ana sorguya iki sorgunun kesişiminin eklenmesi olarak mı birleştirilmesi gerektiğini bildirir</param>
    /// <param name="joinExpression">
    ///   <see cref="IQSDbContext" /> parametresi alarak geriye <span class="typeparameter">TJoinView</span> tipini temsil eden <see cref="IQueryable{TJoinView}" /> döndüren parametre
    /// </param>
    /// <param name="entityKeys">
    ///   <span class="typeparameter">TEntity</span> tipindeki ana sorguyu temsil eden veritabanı nesnesinden, <paramref name="joinExpression" /> ile döenen <span class="typeparameter">TJoinView</span> tipini temsil eden
    /// sorgu cümlesi ile ilişkili olan özellikleri belirler.<see cref="MemberExpression" /></param>
    /// <param name="relationalKeys">
    ///   <paramref name="joinExpression" /> ile döenen <span class="typeparameter">TJoinView</span> tipini temsil eden
    /// sorgu cümlesinden, <span class="typeparameter">TEntity</span> tipindeki ana sorguyu temsil eden veritabanı nesnesi ile ilişkili özellikleri belirler.<see cref="MemberExpression" /></param>
    /// <param name="propertyMaps">
    ///   <span class="typeparameter">TView</span> tipindeki view nesnesinden hangi özelliklerin
    /// <paramref name="joinExpression" /> ile birleştirilen <span class="typeparameter">TJoinView</span> tipini temsil eden sorgu cümlesindeki özelliklerle eşleneceğini
    /// belirleyen parametre
    /// </param>
    public void MapPropertyByJoinTable<TJoinView>(JoinType jointype,
        Func<IApplicationDbContext, QueryCriteria, IList<string>, IQueryable<TJoinView>> joinExpression,
        IEnumerable<Expression<Func<TEntity, object>>> entityKeys,
        IEnumerable<Expression<Func<TJoinView, object>>> relationalKeys,
        params (Expression<Func<TView, object>> fromView, Expression<Func<TJoinView, object>> fromJoinTbale)[]
            propertyMaps)
        where TJoinView : class, new()
    {
        var joinId = Guid.NewGuid().ToString();
        var model = new JoinMap<TEntity, TView, object>()
        {
            JoinQueryBuilder = joinExpression,
            RelationalKeyMemberInfo = relationalKeys.Select(x => x.FindProperty()).ToList(),
            EntityKeyMemberInfo =
                entityKeys.Select(x => x.FindProperty())
                    .ToList(), // new List<MemberInfo>() { entityKey.FindProperty() }
            JonStrategy = jointype,
            RelationalKeyDeclaringType = typeof(TJoinView),
            Id = joinId
        };
        Joins.Add(joinId, model);
        if (propertyMaps != null && propertyMaps.Any())
        {
            foreach (var item in propertyMaps)
            {
                var viewMember = item.fromView.FindProperty();
                var key = viewMember.Name;
                var memberMap = new MemberMap()
                {
                    IsJoinSource = true,
                    JoinId = joinId,
                    EntityMemberInfo = item.fromJoinTbale.FindProperty(),
                    ViewMemberInfo = viewMember
                };

                ViewMaps[key] = memberMap;
            }
        }
    }

    /// <summary>View özelliklerinin başka bir sorgudan tek bir anahtar alana göre birleştirilerek (<see cref="IQueryable{T}.Join" />) eşleştirilmesi gereken durumlarda kullanılır</summary>
    /// <typeparam name="TJoinView">Birleştirilen sorgunun temsil ettiği tiptir</typeparam>
    /// <param name="jointype">Birleştirilecek sorgunun kesişim olarak veya ana sorguya iki sorgunun kesişiminin eklenmesi olarak mı birleştirilmesi gerektiğini bildirir</param>
    /// <param name="joinExpression">
    ///   <see cref="IQSDbContext" /> parametresi alarak geriye <span class="typeparameter">TJoinView</span> tipini temsil eden <see cref="IQueryable{TJoinView}" /> döndüren parametre
    /// </param>
    /// <param name="entityKey">
    ///   <para>
    ///     <span class="typeparameter">TEntity</span> tipindeki ana sorguyu temsil eden veritabanı nesnesinden, <paramref name="joinExpression" /> ile döenen <span class="typeparameter">TJoinView</span> tipini temsil eden
    /// sorgu cümlesi ile ilişkili olan özelliği belirler.<see cref="MemberExpression" /></para>
    /// </param>
    /// <param name="relationalKey">
    ///   <para>
    ///     <paramref name="joinExpression" /> ile döenen <span class="typeparameter">TJoinView</span> tipini temsil eden
    /// sorgu cümlesinden, <span class="typeparameter">TEntity</span> tipindeki ana sorguyu temsil eden veritabanı nesnesi ile ilişkili özelliği belirler.<see cref="MemberExpression" /></para>
    /// </param>
    /// <param name="propertyMaps">
    ///   <span class="typeparameter">TView</span> tipindeki view nesnesinden hangi özelliklerin
    /// <paramref name="joinExpression" /> ile birleştirilen <span class="typeparameter">TJoinView</span> tipini temsil eden sorgu cümlesindeki özelliklerle eşleneceğini belirleyen parametre
    /// </param>
    public void MapPropertyByJoinTable<TJoinView>(JoinType jointype,
        Func<IApplicationDbContext, QueryCriteria, IList<string>, IQueryable<TJoinView>> joinExpression,
        Expression<Func<TEntity, object>> entityKey, Expression<Func<TJoinView, object>> relationalKey,
        params (Expression<Func<TView, object>> fromView, Expression<Func<TJoinView, object>> fromJoinTbale)[]
            propertyMaps)
        where TJoinView : class, new()
    {
        var joinId = Guid.NewGuid().ToString();
        var model = new JoinMap<TEntity, TView, object>()
        {
            JoinQueryBuilder = joinExpression,
            RelationalKeyMemberInfo = new List<MemberInfo>() { relationalKey.FindProperty() },
            EntityKeyMemberInfo = new List<MemberInfo>() { entityKey.FindProperty() },
            RelationalKeyDeclaringType = typeof(TJoinView),
            JonStrategy = jointype
        };
        Joins.Add(joinId, model);

        if (propertyMaps != null && propertyMaps.Any())
        {
            foreach (var item in propertyMaps)
            {
                var viewMember = item.fromView.FindProperty();
                var key = viewMember.Name;
                var memberMap = new MemberMap()
                {
                    IsJoinSource = true,
                    JoinId = joinId,
                    EntityMemberInfo = item.fromJoinTbale.FindProperty(),
                    ViewMemberInfo = viewMember
                };
                ViewMaps[key] = memberMap;
            }
        }
    }

    /// <summary>
    /// Eşleştirilen özelliklerin <see cref="System.Linq.Dynamic.Core" /> kütüphanesi ile
    /// <span class="typeparameter">TView</span> tipini temsil eden sorgu cümlesine çevirilerek <paramref name="tableQuery" /> parametresinde belirtilen
    /// kurallara göre koşul ifadesini uygulayarak geriye <span class="typeparameter">TView</span> tipini temsil eden sorgu cümlesi döndürür
    /// </summary>
    /// <param name="tableQuery">
    ///   <span class="typeparameter">TView</span> tipini temsil eden sorgu cümlesi
    /// için koşul özelliklerini belirler</param>
    /// <returns>
    ///   <span class="typeparameter">TView</span> tipini temsil eden sorgu cümlesi döndürür</returns>
    /// <exception cref="Exception"></exception>
    public IQueryable<TView> GetQuery(QueryCriteria tableQuery)
    {
        IQueryable query = Context.GetTable<TEntity>();
        if (tableQuery.ArchiveSearch.HasValue && tableQuery.ArchiveSearch.Value && typeof(TEntity).IsAssignableTo(typeof(ISoftDeleteEntity)))
        {
            query=query.Where($"{nameof(ISoftDeleteEntity.Deleted)}=={tableQuery.ArchiveSearch.Value.ToString().ToLowerInvariant()}");
        }
        
        var parseConfig = new ParsingConfig
        {
            UseParameterizedNamesInDynamicQuery = true,
            ResolveTypesBySimpleName = true,
            NullPropagatingUseDefaultValueForNonNullableValueTypes = true
        };

        var joinIterator = new JoinCollection<TEntity, TView>(ViewMaps, Joins, tableQuery, Context);

        foreach (var join in joinIterator)
        {
            if (join.JoinType == JoinType.Left)
            {
                query = query.GroupJoin(parseConfig, join.JoinQuery, join.LeftKey, join.RightKey, join.SelectClause)
                    .SelectMany("it");
            }
            else
            {
                query = query.Join(parseConfig, join.JoinQuery, join.LeftKey, join.RightKey, join.SelectClause);
            }
        }

        var queryForTView = Select<TView>(query, SelectExpressionString(tableQuery));
        
        return queryForTView;
    }

    private string SelectExpressionString(QueryCriteria tableQuery)
    {
        var selectFormat = "new ({0})";
        var colString = new SelectFieldClauseCollection<TEntity, TView>(ViewMaps, tableQuery, Joins).ToList();
        return string.Format(selectFormat, string.Join(",", colString));
    }

    private IQueryable<TResult> Select<TResult>(IQueryable source, string selector, params object[] values)
    {
        if (source == null) throw new ArgumentNullException("source");
        if (selector == null) throw new ArgumentNullException("selector");
        var dynamicLambda = System.Linq.Dynamic.Core.DynamicExpressionParser.ParseLambda(new ParsingConfig
        {
            UseParameterizedNamesInDynamicQuery = true,
            ResolveTypesBySimpleName = true,
            NullPropagatingUseDefaultValueForNonNullableValueTypes = true
        }, source.ElementType, null, selector, values);

        var memberInit = dynamicLambda.Body as NewExpression;
        if (memberInit == null) throw new NotSupportedException();
        var resultType = typeof(TResult);
        var bindings = new List<MemberAssignment>();
        foreach (var member in memberInit.Members)
        {
            var prop = resultType.GetProperty(member.Name);
            var param = memberInit.Arguments[memberInit.Members.IndexOf(member)];

            if (!param.Type.IsAssignableTo(prop.PropertyType) || param.Type == typeof(object))
            {
                param = Expression.Convert(param, prop.PropertyType);
            }

            try
            {
                bindings.Add(Expression.Bind(
                    prop,
                    param
                ));
            }
            catch (Exception)
            {
                throw;
            }
        }

        var body = Expression.MemberInit(Expression.New(resultType), bindings);
        var lambda = Expression.Lambda(body, dynamicLambda.Parameters);
        return source.Provider.CreateQuery<TResult>(
            Expression.Call(
                typeof(Queryable), "Select",
                new Type[] { source.ElementType, lambda.Body.Type },
                source.Expression, Expression.Quote(lambda)));
    }
}