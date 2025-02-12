using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Application_Layer.Services;
using CORE.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
namespace Movie_Magic_API.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class MovieController : ControllerBase
    {
        private readonly MovieService _movieService;
        private readonly GenericService<Movies> _genericService;
        public MovieController(MovieService movieService, GenericService<Movies> genericService)
        {
            _movieService = movieService;
            _genericService = genericService;
        }
        [HttpGet("Genre/{genre}")]
        public async Task<ActionResult<List<Movies>>> ViewMoviesByGenre(string genre)
        {
            var movies = await _movieService.ViewMoviesByGenre(genre);
            return Ok(movies);
        }
        [HttpGet]
        public async Task<ActionResult<List<Movies>>> ViewMovies()
        {
            var movies = await _movieService.ViewMovies();
            return Ok(movies);
        }
        [HttpGet("Upcoming")]
        public async Task<ActionResult<List<Movies>>> ViewUpcomingMovies()
        {
            var movies = await _movieService.ViewUpcomingMovies();
            return Ok(movies);
        }
        [HttpGet("{id}")]
        public async Task<ActionResult<Movies>> GetMovieById(int id)
        {
            var movie = await _movieService.GetMovieById(id);
            if (movie == null)
            {
                return NotFound();
            }
            return Ok(movie);
        }
        [HttpGet("{movieId}/Shows")]
        public async Task<ActionResult<List<MovieShows>>> GetShowsByMovieId(int movieId)
        {
            var shows = await _movieService.GetShowsByMovieId(movieId);
            return Ok(shows);
        }
        [HttpPost]
        public async Task<ActionResult> AddMovie([FromBody] Movies movie)
        {
            await _movieService.AddMovie(movie);
            return CreatedAtAction(nameof(GetMovieById), new { id = movie.Id }, movie);
        }
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteMovie(int id)
        {
            var existingMovie = await _movieService.GetMovieById(id);
            if (existingMovie == null)
            {
                return NotFound();
            }
            await _genericService.Delete(id);
            return NoContent();
        }
    }
}