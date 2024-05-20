using Microsoft.AspNetCore.Mvc;
using MyMovies.MoviesApi.WebApp.Filters;
using MyMovies.MoviesApi.WebApp.Services;

namespace MyMovies.MoviesApi.WebApp.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [logActionFilter]
    public class NewsController : ControllerBase
    {
        private readonly IMovieFeedService? _movieFeedService;

        public NewsController(IMovieFeedService? movieFeedService)
        {
            _movieFeedService = movieFeedService;
        }


        [HttpGet("{strDateTime:dateFormat}")]
        // GET: NewsController/news/5
        public IActionResult Get(string strDatetime)
        {
            var objMovies = _movieFeedService?.GetMovieFeedItems(DateTime.Parse(strDatetime));
            return Ok(objMovies);
        }

    }
}
