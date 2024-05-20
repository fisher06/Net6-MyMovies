public class DateFormatConstraint : IRouteConstraint
{
    public bool Match(HttpContext? httpContext, IRouter? route, string routeKey, RouteValueDictionary values, RouteDirection routeDirection)
    {
        var result = false;
        if (values.ContainsKey("strDatetime"))
        {
            string? publishedDate = values["strDatetime"]?.ToString();
            DateTime date;
            result = DateTime.TryParse(publishedDate, out date);
        }
        return result;

    }
}