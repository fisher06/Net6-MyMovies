
namespace MyMovies.MoviesApi.WebApp.Services
{
    public interface IMovieFeedService
    {
        IList<MovieFeedItem> GetMovieFeedItems(DateTime publishedDate);
    }
}