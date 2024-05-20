using Microsoft.AspNetCore.Mvc;
using MyMovies.MoviesLibrary.Data.Repository;
using MyMovies.MoviesLibrary.Domain;

namespace MyMovies.MoviesApi.WebApp.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
#pragma warning disable CS1591 // Commentaire XML manquant pour le type ou le membre visible publiquement
    public class MoviesController : ControllerBase
    {
        private readonly IMovieRepository _movieRepository;
        public MoviesController(IMovieRepository movieRepository)
        {
            _movieRepository = movieRepository;
        }

        // GET: api/<ActorsController>
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            try
            {
                var movies = await _movieRepository.GetAll();
                return Ok(movies);
            }
            catch (Exception ex)
            {
                //log error
                return StatusCode(500, ex.Message);
            }
        }

        // GET api/<ActorsController>/5
        [HttpGet("{id}", Name = "GetById")]
        public async Task<IActionResult> Get(int id)
        {
            try
            {
                var movie = await _movieRepository.GetById(id);
                if (movie == null)
                    return NotFound();
                return Ok(movie);
            }
            catch (Exception ex)
            {
                //log error
                return StatusCode(500, ex.Message);
            }
        }

        // POST api/<ActorsController>
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] Movie value)
        {
            try
            {
                var createdMovie = await _movieRepository.Create(value);
                return CreatedAtRoute("GetById", new { id = createdMovie.Id }, value);
            }
            catch (Exception ex)
            {
                //log error
                return StatusCode(500, ex.Message);
            }
        }

        // PUT api/<ActorsController>/5
        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id, [FromBody] Movie value)
        {
            try
            {
                var movie = await _movieRepository.GetById(id);
                if (movie == null)
                    return NotFound();
                value.Id = id;
                await _movieRepository.Update(value);
                return NoContent();
            }
            catch (Exception ex)
            {
                //log error
                return StatusCode(500, ex.Message);
            }
        }


        // DELETE api/<ActorsController>/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var movie = await _movieRepository.GetById(id);
                if (movie == null)
                    return NotFound();
                await _movieRepository.Delete(new Movie { Id = id });
                return NoContent();
            }
            catch (Exception ex)
            {
                //log error
                return StatusCode(500, ex.Message);
            }
        }
    }
#pragma warning restore CS1591 // Commentaire XML manquant pour le type ou le membre visible publiquement

}