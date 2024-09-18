using CORE.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CORE.Interface
{
    public interface IMovieRepository
    {
        Task<List<Movies>> FetchMoviesForSearchButton(string query);
        Task<List<Movies>> ViewMoviesByGenre(string genre);
        Task<List<Movies>> ViewMovies();
        Task<List<Movies>> ViewUpcomingMovies();
        Task<Movies> GetMovieById(int id);
        Task<List<MovieShows>> GetShowsByMovieId(int movieId);
        Task AddMovie(Movies movie);
    }

}
