using CORE.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CORE.Interface
{
    public interface IMovieShowsRepository
    {
        Task<List<MovieShows>> getShowById(int id);
    }
}
