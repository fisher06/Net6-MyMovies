using Microsoft.AspNetCore.Mvc;

namespace MyMovies.MoviesApi.WebApp.Controllers
{
    [ApiExplorerSettings(IgnoreApi = true)]
    public class ErrorController : Controller
    {
        [Route("/error")]
        public IActionResult HandleError() => Problem();

    }
}
