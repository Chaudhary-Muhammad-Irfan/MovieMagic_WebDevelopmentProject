using Application_Layer.Services;
using CORE.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking.Internal;
using System.IO;
using System.Numerics;
using System.Security.Cryptography;
using System.Text.Json; 
using Web_Development_Project.Data;
using Web_Development_Project.Models; 
namespace Web_Development_Project.Controllers  
{ 
    [Authorize(Policy = "AdminPolicy")]  
    public class AdminController : Controller
    { 
        private readonly ILogger<AdminController> _logger; 
        private readonly IWebHostEnvironment _env;
        private readonly ApplicationDbContext _context;
        private readonly BookingService _bookingService;
        private readonly MovieService _movieService;
        private readonly MovieShowsService _showService;
        private readonly GenericService<Models.Movies> _genericService;
        private readonly GenericService<CORE.Entities.MovieShows> _genericServiceShows;
        private readonly GenericService<CORE.Entities.AspNetUsers> _genericServiceUsers;
        public AdminController( ILogger<AdminController> logger,IWebHostEnvironment env  , ApplicationDbContext context , 
            BookingService booking , MovieService movie , MovieShowsService show ,
            GenericService<Models.Movies> generic , GenericService<CORE.Entities.MovieShows> genericShow ,
            GenericService<CORE.Entities.AspNetUsers> genericUser)
        {
            _logger = logger;
            _env = env;
            _context = context;
            _bookingService = booking;
            _movieService = movie;
            _showService = show;
            _genericService = generic;
            _genericServiceShows = genericShow;
            _genericServiceUsers = genericUser;
        }
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult AddMovie()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> AddMovie(Models.Movies movie)
        {
            movie.ImageURL = saveImageAsync(movie.Image);
            foreach (var actor in movie.Actors)
            {
                actor.ImageURL = saveImageAsync(actor.Image);
            }
            List<CORE.Entities.Actors> coreActors=new List<CORE.Entities.Actors>();
            CORE.Entities.Actors coreActor=new CORE.Entities.Actors();
            foreach (var actor in movie.Actors)
            {
                coreActor.ActorName = actor.ActorName;
                coreActor.MovieId = actor.MovieId;
                coreActors.Add(coreActor);
            }
            CORE.Entities.Movies coreMovie = new CORE.Entities.Movies
            {
                Id = movie.Id,
                Name = movie.Name,
                Description = movie.Description,
                Genre = movie.Genre,
                Trailer = movie.Trailer,
                ReleaseDate = movie.ReleaseDate,
                Actors = coreActors,
                ImageURL=movie.ImageURL,
            };
            await _movieService.AddMovie(coreMovie);
            return RedirectToAction("ViewMovies", "Admin");
        }
        private async Task<string> SaveImageAsync(IFormFile file)
        {
            if (file != null && file.Length > 0)
            {
                var fileName = Path.GetFileName(file.FileName);
                var filePath = Path.Combine(_env.WebRootPath, "images", fileName);

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await file.CopyToAsync(stream);
                }

                return $"images/{fileName}";
            }

            return null;
        }
        public async Task<IActionResult> ViewMovieDetails(int id) 
        {
            await _genericService.findById(id);
            var movie = _genericService.findById(id);
            if (movie == null)
            {
                return NotFound();
            }
            return View(movie);
        }
        public async Task<IActionResult> getMovieById(int id)
        {
            await _genericService.findById(id);
            var movie = _genericService.findById(id);
            if (movie==null)
            {
                return NotFound();
            }
            return View(movie);
        }
        public IActionResult session()
        {
            HttpContext.Session.SetString("djhkshk", "123");
            object o=HttpContext.Session.GetString("djgkshk");
            return View(o);
        }
        public IActionResult method()
        {
            // View Bag
            ViewBag.x = 5;  // it is of dynamic type. and only for the current request.
            //View Data
            ViewData["str"] = "this is a string";  //  type Dictionary (uses ViewDataDictionary which is a dictionary).
            // in viewData uses key-value pair
            //Temp Data
            TempData["a"] = "This is temp data"; // type dictionary , expire after one use , uses key-value pair
            return View();

            // View bag also uses viewData in backened. they can send data form a controller to the same view. 
            // tempData can send data form controller to that view and also to other controllers.



            //<h1>@ViewBag.x</h1>
            //<h1>@ViewData["str"]</h1>
            //<h1>@TempData["a"]</h1>

            
        }
        public async Task<IActionResult> getShowById(int Id=1002)
        {
            return View(await _genericServiceShows.findById(Id));
        }
        private string saveImageAsync(IFormFile picture)
        {
            string imageFolder = Path.Combine(_env.WebRootPath, "UploadingFiles");
            if (!Directory.Exists(imageFolder))
                Directory.CreateDirectory(imageFolder);
            if (picture == null)
            {
                return Path.Combine("UploadingFiles", "default.jpg"); 
            }
            string uniqueFileName = Guid.NewGuid().ToString() + "_" + picture.FileName;
            string filePath = Path.Combine(imageFolder, uniqueFileName);

            using (var fileStream = new FileStream(filePath, FileMode.Create))
            {
                picture.CopyTo(fileStream);
            }
            return Path.Combine("UploadingFiles", uniqueFileName).Replace("\\", "/");
        }
        [HttpGet]
        public async Task<IActionResult> ViewMovies()
        {
            List<Models.Movies> movies = await _genericService.viewAll();
            List<ViewModel> viewModels = new List<ViewModel>();
            foreach (var movie in movies)
            {
                List<CORE.Entities.MovieShows> shows =await _showService.getShowById(movie.Id);
                ViewModel vm = new ViewModel { Movie = movie, Shows = shows };
                viewModels.Add(vm);
            }
            return View(viewModels);
        }
        public IActionResult DeleteMovie()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> DeleteMovie(int Id)
        {
            await _genericService.Delete(Id);
            return RedirectToAction("ViewMovies", "Admin");
        }
        public IActionResult ViewBookings()
        {
            return View();
        }
        [HttpGet]
        public async Task<IActionResult> ViewUsers()
        {
            return View(await _genericServiceUsers.viewAll());
        }
        [HttpGet]
        public async Task<IActionResult> viewShows()
        {
            var allShows =await _genericServiceShows.viewAll();
            return View(allShows);
        }
        public IActionResult AddShow()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> AddShow(CORE.Entities.MovieShows movieshow)
        {
            await _genericServiceShows.Add(movieshow);
            await _bookingService.GetLastShowId();
            TempData["ShowId"] =await _bookingService.GetLastShowId();
            TempData["MovieId"] = movieshow.movieId;
            return RedirectToAction("InsertSeats");
        }
        public async Task<IActionResult> InsertSeats()
        {
            int showId = (int)TempData["ShowId"];
            int movieId = (int)TempData["MovieId"];
            await _bookingService.AddSeatsForShow(movieId, showId);
            return RedirectToAction("ViewMovies");
        }
        public IActionResult DeleteShow()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> DeleteShow(int Id)
		{
            await _genericServiceShows.Delete(Id);
            return RedirectToAction("ViewShows", "Admin");
        }
        public IActionResult UpdateShow(int id)
        {
            var show = _genericServiceShows.findById(id);
            if (show == null)
            {
                return NotFound();
            }
            return View(show);
        }
        [HttpPost]
        public async Task<IActionResult> updateShow(CORE.Entities.MovieShows movieShows)
        {
            await _genericServiceShows.Update(movieShows);
            return RedirectToAction("ViewShows", "Admin");
        }
        public async Task<IActionResult> Search(string MovieName)
        {

            List<Models.Movies> movies = await _genericService.viewAll();
            List<ViewModel> viewModels = new List<ViewModel>();
            foreach (var movie in movies)
            {
                List<CORE.Entities.MovieShows> shows = await _showService.getShowById(movie.Id);
                ViewModel vm = new ViewModel { Movie = movie, Shows = shows };
                viewModels.Add(vm);
            }
            if (MovieName == null)
            {                
                return PartialView("_MoviesListPartial", viewModels);
            }
            else
            {
                var filteredList = viewModels.Where(a => a.Movie.Name.Contains(MovieName, StringComparison.OrdinalIgnoreCase)).ToList();
                return PartialView("_MoviesListPartial", filteredList);
            }
        }
    }
} 