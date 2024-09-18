using CORE.Entities;
using CORE.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text; 
using System.Threading.Tasks;

namespace Application_Layer.Services
{
    public class MovieShowsService
    {
        private readonly IMovieShowsRepository repo;
        public MovieShowsService(IMovieShowsRepository repo)
        {  this.repo = repo; }
        public async Task<List<MovieShows>> getShowById(int id)
        {
            return await repo.getShowById(id);
        }
    }
}
