using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using MyMovies.MoviesApi.WebApp.Filters;
using MyMovies.MoviesApi.WebApp.Services;


namespace MyMovies.MoviesApi.WebApp.Controllers;

// [Route("api/[controller]")]
// [ApiController]
[LogActionFilter]
[Produces("application/xml")]
public class NewsController : ControllerBase
{
    private readonly MovieFeedService feedService;

    public NewsController(MovieFeedService feedService)
    {
        this.feedService = feedService;
    }

    public IList<MovieFeedItem> Movies(string strDatetime)
        => feedService.GetMovieFeed(DateTime.Parse(strDatetime));
}


