using Application_Layer.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using System.Collections.Generic;
using System.Diagnostics;
using System.Security.Claims;
using Web_Development_Project.Data;
using Web_Development_Project.Models;

namespace Web_Development_Project.Controllers
{
    public class HomeController : Controller
    { 
        private readonly ILogger<HomeController> _logger;
        private readonly MovieService _movie;
        private readonly GenericService<Movies> _genericMovies;
        private readonly MovieShowsService _movieShows;
        private readonly IMemoryCache _memoryCache;
        public HomeController(ILogger<HomeController> logger , MovieService movie , GenericService<Movies> movies , MovieShowsService movieShows , IMemoryCache memoryCache)
        {
            _logger = logger;
            _movie = movie;
            _genericMovies = movies;
            _movieShows = movieShows; 
            _memoryCache = memoryCache;
        }
        public async Task<IActionResult> Index()
        {
            if (User.HasClaim(claim => claim.Type == ClaimTypes.Email && claim.Value == "Admin@movieMagic.com"))
            {
                return RedirectToAction("ViewMovies", "Admin");
            }
            List<CORE.Entities.Movies> movies = await _movie.ViewMovies();
            return View(movies);
        }
        public async Task<IActionResult> HomeMovies()
        {
            List<CORE.Entities.Movies> movies =await _movie.ViewUpcomingMovies();
            return PartialView("_HomeMovies", movies);
        }
        public IActionResult About()
        {
            return View();
        }
        public IActionResult Contact()
        {
            return View();
        }
        [HttpGet]
        [ResponseCache(Duration =60 , Location =ResponseCacheLocation.Client)]
        public async Task<IActionResult> Gallery()
        {
            if(!_memoryCache.TryGetValue("GalleryCache" , out List<CORE.Entities.Movies> movies))
            {
                movies=await _movie.ViewMovies();
                _memoryCache.Set("GalleryCache",movies,TimeSpan.FromMinutes(5));    
            }
            return View(movies);
        }
        public async Task<IActionResult> Action()
        {
            List<CORE.Entities.Movies> movies =await _movie.ViewMoviesByGenre("action");
            return View(movies);
        }
        public async Task<IActionResult> Comedy()
        {
            List<CORE.Entities.Movies> movies =await _movie.ViewMoviesByGenre("comedy");
            return View(movies);
        }
        public async Task<IActionResult> Cartoon()
        {
            List<CORE.Entities.Movies> movies =await _movie.ViewMoviesByGenre("Cartoon");
            return View(movies);
        }
        public async Task<IActionResult> Love()
        {
            List<CORE.Entities.Movies> movies = await _movie.ViewMoviesByGenre("Romantic");
            return View(movies);
        }
        public async Task<IActionResult> Horror()
        {
            List<CORE.Entities.Movies> movies =await _movie.ViewMoviesByGenre("Horror");
            return View(movies);
        }
        public async Task<IActionResult> ViewMovieDetails(int id)
        {
            Movies m =await _genericMovies.findById(id);
            List<CORE.Entities.MovieShows> shows =await _movieShows.getShowById(id);        
            var viewModel = new ViewModel
            {
                Movie = m,
                Shows = shows
            };
            return View(viewModel);
        }
    }
}