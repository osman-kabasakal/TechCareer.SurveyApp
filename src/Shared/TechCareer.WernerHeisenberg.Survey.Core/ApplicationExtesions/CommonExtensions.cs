using System.Linq.Expressions;
using System.Reflection;

namespace TechCareer.WernerHeisenberg.Survey.Core.ApplicationExtesions;

public static class CommonExtensions
{
    public static TimeZoneInfo GetTurkeyTimeZone()
    {
        var timeZones = TimeZoneInfo.GetSystemTimeZones();
        var istanbulTimeZone = timeZones.FirstOrDefault(x => x.Id.Contains("Turkey") || x.Id.Contains("Istanbul"));
        return istanbulTimeZone;
    }

    public static DateTime ToIstanbulDateTime(this DateTime dateTime)
    {
        if (dateTime == null)
        {
            return new DateTime();
        }
        var timeZones = TimeZoneInfo.GetSystemTimeZones();
        var istanbulTimeZone = timeZones.FirstOrDefault(x => x.Id.Contains("Turkey") || x.Id.Contains("Istanbul"));
        var localTime = TimeZoneInfo.ConvertTime(new DateTime(dateTime.Ticks, DateTimeKind.Local),
            TimeZoneInfo.Local,
            istanbulTimeZone!);
        return localTime;
    }
    
    public static string ToSafeString(this object? obj)
    {
        if (obj == null)
            return string.Empty;

        return obj.ToString()!;
    }
    
    public static MemberInfo? FindProperty(this LambdaExpression lambdaExpression)
    {
        Expression expressionToCheck = lambdaExpression;

        var done = false;

        while (!done)
        {
            switch (expressionToCheck.NodeType)
            {
                case ExpressionType.Convert:
                    expressionToCheck = ((UnaryExpression)expressionToCheck).Operand;
                    break;

                case ExpressionType.Lambda:
                    expressionToCheck = ((LambdaExpression)expressionToCheck).Body;
                    break;

                case ExpressionType.MemberAccess:
                    var memberExpression = ((MemberExpression)expressionToCheck);
                    var member = memberExpression.Member;

                    return member;

                default:
                    done = true;
                    break;
            }
        }

        return null;
    }
}