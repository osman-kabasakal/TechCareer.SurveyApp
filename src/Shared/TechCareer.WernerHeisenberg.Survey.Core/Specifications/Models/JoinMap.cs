using System.Reflection;
using TechCareer.WernerHeisenberg.Survey.Core.Persistence;
using TechCareer.WernerHeisenberg.Survey.Core.QueryCriterias.Models;

namespace TechCareer.WernerHeisenberg.Survey.Core.Specifications.Models;

/// <summary>
///  <see cref="QuerySpecification{TEntity, TView}"/> sınıfı için, view sınıfına ait özelliğin
///  başka bir <see cref="IQueryable{TEntity}"/> tipindeki sorgudan haritalanması için gerekli olan
///  kuralların belirlendiği sınıfdır.
/// </summary>
/// <typeparam name="TEntity"><see cref="IQueryable{T}"/> tipindeki generic sınıfı için kullanılan tiptir</typeparam>
/// <typeparam name="TJoin"><see cref="IQueryable{TEntity}"/> tipindeki generic sınıf ile ilişkilendirelecek diğer <see cref="IQueryable{T}"/> tipindeki tiptir</typeparam>
public class JoinMap<TEntity, TView, TJoin> where TJoin : class, new()
    where TView : class, new()
{
    public JoinMap()
    {
        EntityKeyMemberInfo = new List<MemberInfo>();
        RelationalKeyMemberInfo = new List<MemberInfo>();
    }

    /// <summary>
    /// <see cref="QuerySpecification{TEntity, TView}"/> sınıfında bulunan <see cref="QuerySpecification{TEntity, TView}.MapPropertyByJoinTable"/> method içerisinde kullanılan parametredir
    /// ilişkişli olan view özelliğinin, ilişiki verilecek sorgu sınıfını döndüren method olarak tanımlanır
    /// </summary>
    public Func<IApplicationDbContext, QueryCriteria,IList<string>, IQueryable<TJoin>> JoinQueryBuilder { get; set; }

    /// <summary>
    /// <see cref="JoinMap{TEntity, TJoin}.JoinQueryBuilder"/> ile dönen sorgu cümlesinde
    /// ilişkilendirilmek istenen ana sorgu içerisindeki anahtar özelliklerdir
    /// Birden fazla anahtara göre ilişkilendirme yapıldığı için <see cref="IEnumerable{T}"/> tipindedir
    /// </summary>
    public List<MemberInfo> EntityKeyMemberInfo { get; set; }

    /// <summary>
    ///  <see cref="JoinMap{TEntity, TJoin}.JoinQueryBuilder"/> ile dönen sorgu cümlesinin,
    ///  ana sorgu içerisinde belirlenen anahtar özelliklerin karşılığıdır.
    ///  Birden fazla anahtara göre ilişkilendirme yapıldığı için <see cref="IEnumerable{T}"/> tipindedir
    ///
    /// <see cref="JoinMap{TEntity, TJoin}.EntityKeyMemberInfo"/> ile belirlenen anahtar özellik ile aynı sayıda özellik girilmelidir
    /// </summary>
    public List<MemberInfo> RelationalKeyMemberInfo { get; set; }

    private Type _relationalKeyDeclaringType;
    private Type _entityKeyDeclaringType;

    /// <summary>
    /// <see cref="System.Linq.Dynamic.Core"/> ile dynamic olarak olarak çekilen ve  birleştirilecek sorgunun özelliklerinin hangi tipe ait olduğunu belirleyen özelliktir
    /// </summary>
    public Type RelationalKeyDeclaringType { get; set; }

    /// <summary>
    /// <see cref="System.Linq.Dynamic.Core"/> ile dynamic olarak olarak çekilen ve  ana sorgunun özelliklerinin hangi tipe ait olduğunu belirleyen özelliktir
    /// </summary>
    public Type EntityKeyDeclaringType => (_entityKeyDeclaringType ??
                                           (_entityKeyDeclaringType = typeof(TEntity)));

    /// <summary>
    /// Birleştirilecek sorgunun inner veya left join olarak birleştirilmesinin sağlandığı kuraldır.
    /// </summary>
    public JoinType JonStrategy { get; set; }

    public string Id { get; internal set; }
}