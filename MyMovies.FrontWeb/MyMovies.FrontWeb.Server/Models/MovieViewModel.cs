namespace MyMovies.FrontWeb.Server.Models;

public record class MovieViewModel(int ID, string? Title, int? ReleaseDate, TimeSpan? Runtime, string? Resume)
{
}
