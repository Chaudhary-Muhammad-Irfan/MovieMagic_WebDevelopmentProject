using CORE.Entities;
using CORE.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application_Layer.Services
{
    public class MovieService
    {
        private readonly IMovieRepository repo;
        public MovieService(IMovieRepository repo)
        {
            this.repo = repo;
        }
        public async Task<List<Movies>> FetchMoviesForSearchButton(string query)
        {
            return await repo.FetchMoviesForSearchButton(query);
        }
        public async Task<List<Movies>> ViewMoviesByGenre(string genre)
        {
            return await repo.ViewMoviesByGenre(genre);
        }
        public async Task<List<Movies>> ViewMovies()
        {
            return await repo.ViewMovies();
        }
        public async Task<List<Movies>> ViewUpcomingMovies()
        {
            return await repo.ViewUpcomingMovies();
        }
        public async Task<Movies> GetMovieById(int id)
        {
            return await repo.GetMovieById(id);
        }
        public async Task<List<MovieShows>> GetShowsByMovieId(int movieId)
        {
            return await repo.GetShowsByMovieId(movieId);
        }
        public async Task AddMovie(Movies movie)
        {
            await repo.AddMovie(movie);
        }
    }
}
