using CORE.Entities;
using CORE.Interface;
using Dapper;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace Infra_Structure_Layer.Repositories
{
    public class MovieShowsRepository : IMovieShowsRepository
    {
        private readonly string connectionString = "Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=Web_Development_Project;Integrated Security=True";

        public async Task<List<MovieShows>> getShowById(int id)
        {
            using (var con = new SqlConnection(connectionString))
            {
                await con.OpenAsync();
                var query = "SELECT * FROM MovieShows WHERE movieId = @id";
                var shows = (await con.QueryAsync<MovieShows>(query, new { id })).ToList();
                return shows;
            }
        }
    }
}
























//using CORE.Entities;
//using CORE.Interface;
//using System;
//using System.Collections.Generic;
//using System.Data.SqlClient;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;

//namespace Infra_Structure_Layer.Repositories
//{
//    public class MovieShowsRepository : IMovieShowsRepository
//    {
//        public async Task<List<MovieShows>> getShowById(int id)
//        {
//            List<MovieShows> shows = new List<MovieShows>();
//            string connectionString = "Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=Web_Development_Project;Integrated Security=True";

//            using (SqlConnection con = new SqlConnection(connectionString))
//            {
//                await con.OpenAsync();
//                string showsQuery = "SELECT * FROM MovieShows WHERE movieId = @id";
//                using (SqlCommand cmdShows = new SqlCommand(showsQuery, con))
//                {
//                    cmdShows.Parameters.AddWithValue("@id", id);
//                    using (SqlDataReader readerShows = await cmdShows.ExecuteReaderAsync())
//                    {
//                        while (await readerShows.ReadAsync())
//                        {
//                            MovieShows show = new MovieShows
//                            {
//                                Id = readerShows.GetInt32(readerShows.GetOrdinal("Id")),
//                                showDate = readerShows.GetDateTime(readerShows.GetOrdinal("showDate")).Date,
//                                showDay = readerShows.GetString(readerShows.GetOrdinal("showDay")),
//                                showTime = readerShows.GetTimeSpan(readerShows.GetOrdinal("showTime"))
//                            };
//                            shows.Add(show);
//                        }
//                    }
//                }
//            }
//            return shows;
//        }
//    }
//}