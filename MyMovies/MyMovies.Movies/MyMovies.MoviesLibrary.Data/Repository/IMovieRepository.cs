using MyMovies.MoviesLibrary.Domain;

namespace MyMovies.MoviesLibrary.Data.Repository
{
    public interface IMovieRepository
    {
        Task<IEnumerable<Movie>> GetAll();

        Task<Movie?> GetById(int? id);

        Task<Movie> Create(Movie? movie);

        Task<Movie> Update(Movie? movie);

        Task<int> Delete(Movie? movie);
    }
}
