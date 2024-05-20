using System.Diagnostics;
using System.Runtime.CompilerServices;
using Microsoft.AspNetCore.Mvc.Filters;

namespace MyMovies.MoviesApi.WebApp.Filters
{
    public class logActionFilter : ActionFilterAttribute
    {
        private void log(RouteData routeData, [CallerMemberName] string? methodName = null)
        {
            var controllerName = routeData.Values["controller"];
            var actionName = routeData.Values["action"];
            Debug.WriteLine($"{methodName} - controller - {controllerName} - action - {actionName}");
        }
        public override void OnActionExecuted(ActionExecutedContext context)
        {
            log(context.RouteData);
            base.OnActionExecuted(context);
        }

        public override void OnActionExecuting(ActionExecutingContext context)
        {
            log(context.RouteData);
            base.OnActionExecuting(context);
        }

        public override void OnResultExecuted(ResultExecutedContext context)
        {
            log(context.RouteData);
            base.OnResultExecuted(context);
        }

        public override void OnResultExecuting(ResultExecutingContext context)
        {
            log(context.RouteData);
            base.OnResultExecuting(context);
        }
    }
}
