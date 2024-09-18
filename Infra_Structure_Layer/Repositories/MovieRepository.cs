using CORE.Entities;
using CORE.Interface;
using Dapper;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace Infra_Structure_Layer.Repositories
{
    public class MovieRepository : IMovieRepository
    {
        private readonly string _connectionString = "Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=Web_Development_Project;Integrated Security=True";

        public async Task<List<Movies>> FetchMoviesForSearchButton(string query)
        {
            using (var con = new SqlConnection(_connectionString))
            {
                string sql = "SELECT * FROM Movies WHERE Name=@query";
                var movies = await con.QueryAsync<Movies>(sql, new { query });
                return movies.ToList();
            }
        }

        public async Task<List<Movies>> ViewMoviesByGenre(string genre)
        {
            using (var con = new SqlConnection(_connectionString))
            {
                string sql = "SELECT * FROM Movies WHERE Genre=@genre";
                var movies = await con.QueryAsync<Movies>(sql, new { genre });
                return movies.ToList();
            }
        }

        public async Task<List<Movies>> ViewMovies()
        {
            using (var con = new SqlConnection(_connectionString))
            {
                string sql = "SELECT * FROM Movies";
                var movies = await con.QueryAsync<Movies>(sql);
                return movies.ToList();
            }
        }

        public async Task<List<Movies>> ViewUpcomingMovies()
        {
            using (var con = new SqlConnection(_connectionString))
            {
                string sql = "SELECT * FROM Movies WHERE ReleaseDate > @date";
                var movies = await con.QueryAsync<Movies>(sql, new { date = DateTime.Now });
                return movies.ToList();
            }
        }
        public async Task<Movies> GetMovieById(int id)
        {
            using (var con = new SqlConnection(_connectionString))
            {
                string sql = "SELECT * FROM Movies WHERE Id=@id";
                return await con.QuerySingleOrDefaultAsync<Movies>(sql, new { id });
            }
        }
        public async Task<List<MovieShows>> GetShowsByMovieId(int movieId)
        {
            using (var con = new SqlConnection(_connectionString))
            {
                string sql = "SELECT * FROM Shows WHERE MovieId=@movieId";
                var shows = await con.QueryAsync<MovieShows>(sql, new { movieId });
                return shows.ToList();
            }
        }
        public async Task AddMovie(Movies movie)
        {
            using (var con = new SqlConnection(_connectionString))
            {
                await con.OpenAsync();
                using (var transaction = con.BeginTransaction())
                {
                    try
                    {
                        string movieQuery = "INSERT INTO Movies (Name, ReleaseDate, Genre, Description, ImageURL, Trailer) " +
                                            "VALUES (@Name, @ReleaseDate, @Genre, @Description, @ImageURL, @Trailer); " +
                                            "SELECT CAST(SCOPE_IDENTITY() as int);";

                        int movieId = await con.ExecuteScalarAsync<int>(movieQuery, new
                        {
                            movie.Name,
                            movie.ReleaseDate,
                            movie.Genre,
                            movie.Description,
                            movie.ImageURL,
                            movie.Trailer
                        }, transaction);
                        foreach (var actor in movie.Actors.Where(a => !string.IsNullOrEmpty(a.ActorName)))
                        {
                            string actorQuery = "INSERT INTO Actors (ActorName, ImageURL, MovieId) " +
                                                "VALUES (@ActorName, @ImageURL, @MovieId)";

                            await con.ExecuteAsync(actorQuery, new
                            {
                                actor.ActorName,
                                ImageURL = actor.ImageURL ?? (object)DBNull.Value,
                                MovieId = movieId
                            }, transaction);
                        }
                        transaction.Commit();
                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();
                        Console.WriteLine(ex.Message);
                    }
                }
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
//    public class MovieRepository : IMovieRepository
//    {
//        private readonly string _connectionString = "Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=Web_Development_Project;Integrated Security=True";
//        public async Task<List<Movies>> FetchMoviesForSearchButton(string query)
//        {
//            List<Movies> list = new List<Movies>();
//            try
//            {
//                using (SqlConnection con = new SqlConnection(_connectionString))
//                {
//                    await con.OpenAsync();
//                    string read = "select * from Movies where Name=@query";
//                    using (SqlCommand cmd = new SqlCommand(read, con))
//                    {
//                        cmd.Parameters.AddWithValue("@query", query);
//                        using (SqlDataReader reader = await cmd.ExecuteReaderAsync())
//                        {
//                            while (await reader.ReadAsync())
//                            {
//                                Movies m = new Movies
//                                {
//                                    Id = (int)reader["Id"],
//                                    Name = (string)reader["Name"],
//                                    ReleaseDate = (DateTime)reader["ReleaseDate"],
//                                    Genre = (string)reader["Genre"],
//                                    Description = (string)reader["Description"],
//                                    ImageURL = (string)reader["ImageURL"]
//                                };
//                                list.Add(m);
//                            }
//                        }
//                    }
//                }
//            }
//            catch (Exception ex)
//            {
//                Console.WriteLine(ex.Message); // Log the error
//            }
//            return list;
//        }

//        public async Task<List<Movies>> ViewMoviesByGenre(string genre)
//        {
//            List<Movies> list = new List<Movies>();
//            try
//            {
//                using (SqlConnection con = new SqlConnection(_connectionString))
//                {
//                    await con.OpenAsync();
//                    string read = "select * from Movies where Genre=@genre";
//                    using (SqlCommand cmd = new SqlCommand(read, con))
//                    {
//                        cmd.Parameters.AddWithValue("@genre", genre);
//                        using (SqlDataReader reader = await cmd.ExecuteReaderAsync())
//                        {
//                            while (await reader.ReadAsync())
//                            {
//                                Movies m = new Movies
//                                {
//                                    Id = (int)reader["Id"],
//                                    Name = (string)reader["Name"],
//                                    ReleaseDate = (DateTime)reader["ReleaseDate"],
//                                    Genre = (string)reader["Genre"],
//                                    Description = (string)reader["Description"],
//                                    ImageURL = (string)reader["ImageURL"]
//                                };
//                                list.Add(m);
//                            }
//                        }
//                    }
//                }
//            }
//            catch (Exception ex)
//            {
//                Console.WriteLine(ex.Message); // Log the error
//            }
//            return list;
//        }

//        public async Task<List<Movies>> ViewMovies()
//        {
//            List<Movies> list = new List<Movies>();
//            try
//            {
//                using (SqlConnection con = new SqlConnection(_connectionString))
//                {
//                    await con.OpenAsync();
//                    string read = "select * from Movies";
//                    using (SqlCommand cmd = new SqlCommand(read, con))
//                    {
//                        using (SqlDataReader reader = await cmd.ExecuteReaderAsync())
//                        {
//                            while (await reader.ReadAsync())
//                            {
//                                Movies m = new Movies
//                                {
//                                    Id = (int)reader["Id"],
//                                    Name = (string)reader["Name"],
//                                    ReleaseDate = (DateTime)reader["ReleaseDate"],
//                                    Genre = (string)reader["Genre"],
//                                    Description = (string)reader["Description"],
//                                    ImageURL = (string)reader["ImageURL"]
//                                };
//                                list.Add(m);
//                            }
//                        }
//                    }
//                }
//            }
//            catch (Exception ex)
//            {
//                Console.WriteLine(ex.Message);
//            }
//            return list;
//        }

//        public async Task<List<Movies>> ViewUpcomingMovies()
//        {
//            List<Movies> list = new List<Movies>();
//            try
//            {
//                using (SqlConnection con = new SqlConnection(_connectionString))
//                {
//                    await con.OpenAsync();
//                    string read = "SELECT * FROM Movies WHERE ReleaseDate > @date";
//                    using (SqlCommand cmd = new SqlCommand(read, con))
//                    {
//                        cmd.Parameters.AddWithValue("@date", DateTime.Now);
//                        using (SqlDataReader reader = await cmd.ExecuteReaderAsync())
//                        {
//                            while (await reader.ReadAsync())
//                            {
//                                Movies m = new Movies
//                                {
//                                    Id = (int)reader["Id"],
//                                    Name = (string)reader["Name"],
//                                    ReleaseDate = (DateTime)reader["ReleaseDate"],
//                                    Genre = (string)reader["Genre"],
//                                    Description = (string)reader["Description"],
//                                    ImageURL = (string)reader["ImageURL"]
//                                };
//                                list.Add(m);
//                            }
//                        }
//                    }
//                }
//            }
//            catch (Exception ex)
//            {
//                Console.WriteLine(ex.Message);
//            }
//            return list;
//        }

//        public async Task<Movies> GetMovieById(int id)
//        {
//            Movies movie = null;
//            try
//            {
//                using (SqlConnection con = new SqlConnection(_connectionString))
//                {
//                    await con.OpenAsync();
//                    string query = "SELECT * FROM Movies WHERE Id=@id";
//                    using (SqlCommand cmd = new SqlCommand(query, con))
//                    {
//                        cmd.Parameters.AddWithValue("@id", id);
//                        using (SqlDataReader reader = await cmd.ExecuteReaderAsync())
//                        {
//                            if (await reader.ReadAsync())
//                            {
//                                movie = new Movies
//                                {
//                                    Id = (int)reader["Id"],
//                                    Name = (string)reader["Name"],
//                                    ReleaseDate = (DateTime)reader["ReleaseDate"],
//                                    Genre = (string)reader["Genre"],
//                                    Description = (string)reader["Description"],
//                                    ImageURL = (string)reader["ImageURL"]
//                                };
//                            }
//                        }
//                    }
//                }
//            }
//            catch (Exception ex)
//            {
//                Console.WriteLine(ex.Message);
//            }
//            return movie;
//        }

//        public async Task<List<MovieShows>> GetShowsByMovieId(int movieId)
//        {
//            List<MovieShows> shows = new List<MovieShows>();
//            try
//            {
//                using (SqlConnection con = new SqlConnection(_connectionString))
//                {
//                    await con.OpenAsync();
//                    string query = "SELECT * FROM Shows WHERE MovieId=@movieId";
//                    using (SqlCommand cmd = new SqlCommand(query, con))
//                    {
//                        cmd.Parameters.AddWithValue("@movieId", movieId);
//                        using (SqlDataReader reader = await cmd.ExecuteReaderAsync())
//                        {
//                            while (await reader.ReadAsync())
//                            {
//                                MovieShows show = new MovieShows
//                                {
//                                    Id = (int)reader["Id"],
//                                    movieId = (int)reader["movieId"],
//                                    showDate = (DateTime)reader["showDate"],
//                                    showTime = (TimeSpan)reader["showTime"],
//                                    showDay = (string)reader["showDay"]
//                                };
//                                shows.Add(show);
//                            }
//                        }
//                    }
//                }
//            }
//            catch (Exception ex)
//            {
//                Console.WriteLine(ex.Message);
//            }
//            return shows;
//        }

//        public async Task AddMovie(Movies movie)
//        {
//            try
//            {
//                using (SqlConnection con = new SqlConnection(_connectionString))
//                {
//                    await con.OpenAsync();

//                    // Insert the movie
//                    string movieQuery = "INSERT INTO Movies (Name, ReleaseDate, Genre, Description, ImageURL,Trailer) " +
//                                        "VALUES (@Name, @ReleaseDate, @Genre, @Description, @ImageURL, @Trailer); " +
//                                        "SELECT SCOPE_IDENTITY();";
//                    int movieId;

//                    using (SqlCommand cmd = new SqlCommand(movieQuery, con))
//                    {
//                        cmd.Parameters.AddWithValue("@Name", movie.Name);
//                        cmd.Parameters.AddWithValue("@ReleaseDate", movie.ReleaseDate);
//                        cmd.Parameters.AddWithValue("@Genre", movie.Genre);
//                        cmd.Parameters.AddWithValue("@Description", movie.Description);
//                        cmd.Parameters.AddWithValue("@ImageURL", movie.ImageURL);
//                        cmd.Parameters.AddWithValue("@Trailer", movie.Trailer);

//                        movieId = Convert.ToInt32(await cmd.ExecuteScalarAsync());
//                    }

//                    // Insert actors
//                    for (int i = 0; i < movie.Actors.Count; i++)
//                    {
//                        var actor = movie.Actors[i];
//                        if (string.IsNullOrEmpty(actor.ActorName))
//                        {
//                            continue; // Skip actors with empty names
//                        }

//                        string actorQuery = "INSERT INTO Actors (ActorName, ImageURL, MovieId) " +
//                                            "VALUES (@ActorName, @ImageURL, @MovieId)";
//                        using (SqlCommand cmd2 = new SqlCommand(actorQuery, con))
//                        {
//                            cmd2.Parameters.AddWithValue("@ActorName", actor.ActorName);
//                            cmd2.Parameters.AddWithValue("@ImageURL", actor.ImageURL ?? (object)DBNull.Value);
//                            cmd2.Parameters.AddWithValue("@MovieId", movieId);

//                            await cmd2.ExecuteNonQueryAsync();
//                        }
//                    }
//                }
//            }
//            catch (Exception ex)
//            {
//                Console.WriteLine(ex.Message);
//            }
//        }
//    }
//}