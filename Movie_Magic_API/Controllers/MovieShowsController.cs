using Application_Layer.Services;
using CORE.Entities;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace YourNamespace.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MovieShowsController : ControllerBase
    {
        private readonly MovieShowsService _movieShowsService;
        private readonly GenericService<MovieShows> _genericService;

        public MovieShowsController(MovieShowsService movieShowsService , GenericService<MovieShows> genericService)
        {
            _movieShowsService = movieShowsService;
            _genericService = genericService;
        }
        [HttpGet("{id}")]
        public async Task<ActionResult<List<MovieShows>>> GetShowById(int id)
        {
            var shows = await _movieShowsService.getShowById(id);

            if (shows == null || shows.Count == 0)
            {
                return NotFound();
            }

            return Ok(shows);
        }
        [HttpGet]
        public async Task<ActionResult<List<MovieShows>>> GetAllShows()
        {
            var shows = await _genericService.viewAll(); 
            return Ok(shows);
        }
        [HttpPost]
        public async Task<ActionResult<MovieShows>> AddShow([FromBody] MovieShows show)
        {
            if (show == null)
            {
                return BadRequest();
            }

            await _genericService.Add(show); 

            return Ok(); 
        }
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateShow(int id, [FromBody] MovieShows show)
        {
            if (show == null || show.Id != id)
            {
                return BadRequest();
            }
            var existingShow = await _genericService.findById(id);
            if (existingShow == null)
            {
                return NotFound();
            }
            await _genericService.Update(existingShow); 
            return NoContent();
        }
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteShow(int id)
        {
            var existingShow = await _movieShowsService.getShowById(id);
            if (existingShow == null)
            {
                return NotFound();
            }

            await _genericService.Delete(id);

            return NoContent();
        }
    }
}
