using MyMovies.MoviesLibrary.Business.Model;

namespace MyMovies.MoviesLibrary.Business.Services;

public interface IMovieService
{
    Task<IEnumerable<MovieDTO>> GetAll();

    Task<MovieDTO?> GetById(int? id);

    Task<MovieDTO> Create(MovieDTO? movieDTO);

    Task<MovieDTO> Update(MovieDTO? movieDTO);

    Task<int> Delete(MovieDTO? movieDTO);
}
