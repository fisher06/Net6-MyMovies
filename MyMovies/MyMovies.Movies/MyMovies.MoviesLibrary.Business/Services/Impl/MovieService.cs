
using AutoMapper;
using MyMovies.MoviesLibrary.Business.Model;
using MyMovies.MoviesLibrary.Data.Repository;
using MyMovies.MoviesLibrary.Domain;
using System.ComponentModel.DataAnnotations;

namespace MyMovies.MoviesLibrary.Business.Services.Impl;

public class MovieService : IMovieService
{
    private readonly IMapper _mapper;
    private readonly IMovieRepository _movieRepository;

    public MovieService(IMapper mapper, IMovieRepository movieRepository)
    {
        this._mapper = mapper;
        this._movieRepository = movieRepository;
    }

    public async Task<IEnumerable<MovieDTO>> GetAll()
    {
        var movies = await _movieRepository.GetAll();

        return _mapper.Map<IEnumerable<Movie>, IEnumerable<MovieDTO>>(movies);
    }

    public async Task<MovieDTO?> GetById(int? id)
    {
        var movie = await _movieRepository.GetById(id);

        return _mapper.Map<Movie?, MovieDTO?>(movie);
    }

    public async Task<MovieDTO> Create(MovieDTO? movieDTO)
    {
        var movie = _mapper.Map<MovieDTO?, Movie?>(movieDTO);
        if (movie == null)
            throw new ArgumentNullException(null, "Movie ne peut être null !");

        var context = new ValidationContext(movie);
        var results = new List<ValidationResult>();
        if (!Validator.TryValidateObject(movie, context, results, true))
            throw new ValidationException(message: "Movie incorrect !",
                new AggregateException(results.ConvertAll(new Converter<ValidationResult, ValidationException>
                                                              (v => new ValidationException(v.ErrorMessage)))));

        var createdMovie = await _movieRepository.Create(movie);

        return _mapper.Map<Movie, MovieDTO>(createdMovie);
    }

    public async Task<MovieDTO> Update(MovieDTO? movieDTO)
    {
        var movie = _mapper.Map<MovieDTO?, Movie?>(movieDTO);

        if (movie == null)
            throw new ArgumentNullException(null, "Movie ne peut être null !");

        var context = new ValidationContext(movie);
        var results = new List<ValidationResult>();
        if (!Validator.TryValidateObject(movie, context, results, true))
            throw new ValidationException(message: "Movie incorrect !",
                new AggregateException(results.ConvertAll(new Converter<ValidationResult, ValidationException>
                                                              (v => new ValidationException(v.ErrorMessage)))));

        var updatedMovie = await _movieRepository.Update(movie);

        return _mapper.Map<Movie, MovieDTO>(updatedMovie);
    }
    public async Task<int> Delete(MovieDTO? movieDTO)
    {
        var movie = _mapper.Map<MovieDTO?, Movie?>(movieDTO);

        return await _movieRepository.Delete(movie);
    }
}
