using Microsoft.AspNetCore.Mvc;

namespace MyMovies.MoviesApi.WebApp.ExtendedMethods;

public static class ExtendedControllerContext
{
    public static string ToDisplay(this ControllerContext context, string? message = null )
    {
        var actionDescriptor = context.ActionDescriptor;
        var routeTemplate = actionDescriptor?.AttributeRouteInfo?.Template;
        var routeName = actionDescriptor?.AttributeRouteInfo?.Name;
        var actionName = actionDescriptor?.ActionName;
        var controllerName = actionDescriptor?.ControllerName;
        var routeOrder = actionDescriptor?.AttributeRouteInfo?.Order;
        var method = context.HttpContext.Request.Method;
        var path = context.HttpContext.Request.Path;

        return $"[{method} {path} - {routeTemplate} - {controllerName}.{actionName} {routeName}] - {message}";
    }
}
