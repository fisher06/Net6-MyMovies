using Microsoft.AspNetCore.Mvc;

namespace MyMovies.MoviesApi.WebApp.Controllers;

[ApiExplorerSettings(IgnoreApi = true)]
public class ErrorController : ControllerBase
{
    [Route("/error")]
    public IActionResult HandleError() => Problem();
}
