using System.Collections;
using Microsoft.AspNetCore.Mvc;
using MyMovies.MoviesApi.WebApp.Extensions;
using MyMovies.MoviesApi.WebApp.Filters;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace MyMovies.MoviesApi.WebApp.Controllers
{
    /// <summary>
    /// Actors Controller
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    [logActionFilter]
    public class ActorsController : ControllerBase
    {
        /// <summary>
        /// Operation de recuperation des films
        /// </summary>
        // GET: api/<ActorsController>
        [HttpGet]
        public IEnumerable Get()
        {
            //throw new Exception("error");
            return new string[] { ControllerContext.ToDisplay("value1"), ControllerContext.ToDisplay("value2") };
        }

        // GET api/<ActorsController>/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return ControllerContext.ToDisplay(id.ToString());
        }

        //public IActionResult Get(int id)
        //{
        //    try
        //    {
        //        return Ok($"{id}");
        //    }
        //    catch (Exception e)
        //    {
        //        return BadRequest(e);
        //    }


        //}

        // POST api/<ActorsController>
        [HttpPost]
        public void Post([FromBody] string value)
        {
        }

        // PUT api/<ActorsController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/<ActorsController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
