using Microsoft.AspNetCore.Mvc;
using MyMovies.MoviesLibrary.Data.Repository;
using MyMovies.MoviesLibrary.Domain;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace MyMovies.MoviesApi.WebApp.Controllers;

[Route("api/[controller]")]
[ApiController]
public class ActorsController : ControllerBase
{
    private readonly IActorRepository _actorRepository;
    public ActorsController(IActorRepository actorRepository)
    {
        _actorRepository = actorRepository;
    }

    // GET: api/<ActorsController>
    [HttpGet]
    public async Task<IActionResult> Get()
    {
        try
        {
            var actors = await _actorRepository.GetAll();
            return Ok(actors);
        }
        catch (Exception ex)
        {
            //log error
            return StatusCode(500, ex.Message);
        }
    }

    // GET api/<ActorsController>/5
    [HttpGet("{id}", Name = "GetActorById")]
    public async Task<IActionResult> Get(int id)
    {
        try
        {
            var actor = await _actorRepository.GetById(id);
            if (actor == null)
                return NotFound();
            return Ok(actor);
        }
        catch (Exception ex)
        {
            //log error
            return StatusCode(500, ex.Message);
        }
    }

    // POST api/<ActorsController>
    [HttpPost]
    public async Task<IActionResult> Post([FromBody] Actor value)
    {
        try
        {
            var createdActor = await _actorRepository.Create(value);
            return CreatedAtRoute("GetActorById", new { id = createdActor.ID }, value);
        }
        catch (Exception ex)
        {
            //log error
            return StatusCode(500, ex.Message);
        }
    }

    // PUT api/<ActorsController>/5
    [HttpPut("{id}")]
    public async Task<IActionResult> Put(int id, [FromBody] Actor value)
    {
        try
        {
            var actor = await _actorRepository.GetById(id);
            if (actor == null)
                return NotFound();
            value.ID = id;
            await _actorRepository.Update(value);
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
            var actor = await _actorRepository.GetById(id);
            if (actor == null)
                return NotFound();
            await _actorRepository.Delete(new Actor { ID = id });
            return NoContent();
        }
        catch (Exception ex)
        {
            //log error
            return StatusCode(500, ex.Message);
        }
    }
}
