
public interface IDateFormatConstraint
{
    bool Match(HttpContext? httpContext, IRouter? route, string routeKey, RouteValueDictionary values, RouteDirection routeDirection);
}