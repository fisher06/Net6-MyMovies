using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using MyMovies.MoviesLibrary.Business.Model;
using MyMovies.MoviesLibrary.Business.Services;

namespace MyMovies.MoviesApi.WebApp.Controllers;

[Route("api/[controller]")]
[ApiController]
[Authorize]
#pragma warning disable CS1591
public class MoviesController : Controller
{
    private readonly IMovieService _movieService;
    public MoviesController(IMovieService movieService)
    {
        _movieService = movieService;
    }

    // GET: api/<MoviesController>
    [HttpGet]
    public async Task<IActionResult> Get()
    {
        try
        {
            var movies = await _movieService.GetAll();
            return Ok(movies);
        }
        catch (Exception ex)
        {
            //log error
            return StatusCode(500, ex.Message);
        }
    }

    // GET api/<MoviesController>/5
    [HttpGet("{id}", Name = "GetById")]
    public async Task<IActionResult> Get(int id)
    {
        try
        {
            var movie = await _movieService.GetById(id);
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

    // POST api/<MoviesController>
    [HttpPost]
    public async Task<IActionResult> Post([FromBody] MovieDTO value)
    {
        try
        {
            //if (value.ReleaseDate > DateTime.Now.Year)
            //{
            //    ModelState.AddModelError("ReleaseDate", "La ReleaseDate doit être inférieure à l'année en cours...");
            //}

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var createdMovie = await _movieService.Create(value);
            return CreatedAtRoute("GetById", new { id = createdMovie.ID }, createdMovie);
        }
        catch (Exception ex)
        {
            //log error
            return StatusCode(500, ex.Message);
        }
    }

    // PUT api/<MoviesController>/5
    [HttpPut("{id}")]
    public async Task<IActionResult> Put(int id, [FromBody] MovieDTO value)
    {
        try
        {
            //if (value.ReleaseDate > DateTime.Now.Year)
            //{
            //    ModelState.AddModelError("ReleaseDate", "La ReleaseDate doit être inférieure à l'année en cours...");
            //}

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var movie = await _movieService.GetById(id);
            if (movie == null)
                return NotFound();
            value.ID = id;
            var updatedMovie = await _movieService.Update(value);
            return Ok(updatedMovie);
        }
        catch (Exception ex)
        {
            //log error
            return StatusCode(500, ex.Message);
        }
    }


    // DELETE api/<MoviesController>/5
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        try
        {
            var movie = await _movieService.GetById(id);
            if (movie == null)
                return NotFound();
            await _movieService.Delete(new MovieDTO { ID = id });
            return NoContent();
        }
        catch (Exception ex)
        {
            //log error
            return StatusCode(500, ex.Message);
        }
    }

    //public IActionResult Index()
    //{
    //    return View();
    //}
    //[NonAction]
    //public string InformationReservee()
    //{
    //    return "Secret";
    //}

    ///// <summary>
    ///// Opération permattant de récupérer les films
    ///// </summary>
    ///// <returns>Liste de films</returns>
    //[HttpGet]
    //public string Get()
    //{
    //    return ControllerContext.ToDisplay("Hello Movies !");
    //}
}
#pragma warning restore CS1591