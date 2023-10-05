using System.Reflection;

namespace TechCareer.WernerHeisenberg.Survey.Core.Specifications.Models;

/// <summary>
/// <see cref="QuerySpecification{TEntity, TView}"/> sınıfı içerisinde
/// View'e ait özelliğin <see cref="ViewMemberInfo"/> içerisine aktareılarak,veritabanı neseninden veya birleştirilen bir sorgudan, belirlenen özelliğin
/// <see cref="ViewMemberInfo"/> özelliğine atanarak
/// eşleştirilmesini sağlayan sınıf
/// Eğer <see cref="ViewMemberInfo"/> özelliği birleştirilecek bir sorgudan belirlenecek bir özellik ile
/// eşleştirilmek isteniyorsa <see cref="IsJoinSource"/> true ve <see cref="JoinId"/> belirtilmek zotundadır
/// </summary>
public class MemberMap
{
    /// <summary>
    /// View sınıfına ait özelliğin birlştirilen bir sınıftan mı eşleştirileceğini
    /// belirlemek için kullanılır
    /// </summary>
    public bool IsJoinSource { get; set; }

    /// <summary>
    /// <see cref="ViewMemberInfo"/> özelliğinin belirlenen veritabanı nesnesinden
    /// veya birleştirilen sorgu cümlesinden hangi özellik ile eşleşeceğini belirlemek için kullanılır
    /// </summary>
    public MemberInfo EntityMemberInfo { get; set; }

    /// <summary>
    /// Veri tabanı nesnesinin aktarılacağı özelliği belirlemek için kullanılır
    /// </summary>
    public MemberInfo ViewMemberInfo { get; set; }

    /// <summary>
    /// View sınıfına ait özellik, birlştirilen bir sınıftan eşleştiriliyorsa
    /// <see cref="QuerySpecification{TEntity, TView}.Joins"/> içerisine benzersiz bir ID ile kaydedilen
    /// <see cref="JoinMap{TEntity, TJoin}"/> sınıfının ID'sini saklamak için kullanılır
    /// </summary>
    public string JoinId { get; set; }

    public bool IsSelectorClause { get; set; }

    public Func<string, string> SelectorClause { get; set; }
}