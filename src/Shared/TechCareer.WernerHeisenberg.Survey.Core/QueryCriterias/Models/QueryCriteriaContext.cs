using System.ComponentModel;
using System.Linq.Expressions;
using System.Reflection;

namespace TechCareer.WernerHeisenberg.Survey.Core.QueryCriterias.Models;

public record struct QueryCriteriaContext(
    PropertySearch Query,
    PropertyInfo TargetPropertyInfo,
    TypeConverter Converter,
    Expression NameProperty
    );