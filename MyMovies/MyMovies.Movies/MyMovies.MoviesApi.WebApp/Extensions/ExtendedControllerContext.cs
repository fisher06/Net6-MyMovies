using Microsoft.AspNetCore.Mvc;

namespace MyMovies.MoviesApi.WebApp.Extensions;

public static class ExtendedControllerContext
{
    public static string ToDisplay(this ControllerContext context, string? message = null)
    {
        var method = context.HttpContext.Request.Method;
        var path = context.HttpContext.Request.Path;

        var actionDescriptor = context.ActionDescriptor;
        var routeTemplate = actionDescriptor.AttributeRouteInfo?.Template;
        var controllerName = actionDescriptor.ControllerName;
        var actionName = actionDescriptor.ActionName;
        var routeName = actionDescriptor.AttributeRouteInfo?.Name;

        return $"[{method} {path} - {routeTemplate} - {controllerName}.{actionName} {routeName}] - {message}";
    }
}
