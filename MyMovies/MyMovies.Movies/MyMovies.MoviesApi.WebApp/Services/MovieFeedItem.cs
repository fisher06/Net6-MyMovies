namespace MyMovies.MoviesApi.WebApp.Services;


public record class MovieFeedItem(string Title, string Summary, DateTime PublishedDate)
{
    public MovieFeedItem() : this(String.Empty, string.Empty, DateTime.Now) { }
}