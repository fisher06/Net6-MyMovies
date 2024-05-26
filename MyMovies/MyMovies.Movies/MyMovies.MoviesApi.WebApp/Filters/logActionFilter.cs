using Microsoft.AspNetCore.Mvc.Filters;
using System.Diagnostics;

namespace MyMovies.MoviesApi.WebApp.Filters;

public class LogActionFilter : ActionFilterAttribute
{
    private void log(string methodName, RouteData routeData, Action baseMethod)
    {
        var controllerName = routeData.Values["controller"];
        var actionName = routeData.Values["action"];
        var message = $"{methodName} controller : {controllerName} action: {actionName}";
        Debug.WriteLine(message, "Action Filter Log");

        baseMethod();
    }

    public override void OnActionExecuting(ActionExecutingContext context)
        => log("OnActionExecuting", context.RouteData, () => base.OnActionExecuting(context));

    public override void OnActionExecuted(ActionExecutedContext context)
        => log("OnActionExecuted", context.RouteData, () => base.OnActionExecuted(context));

    public override void OnResultExecuting(ResultExecutingContext context)
       => log("OnResultExecuting", context.RouteData, () => base.OnResultExecuting(context));

    public override void OnResultExecuted(ResultExecutedContext context)
        => log("OnResultExecuted", context.RouteData, () => base.OnResultExecuted(context));
}

