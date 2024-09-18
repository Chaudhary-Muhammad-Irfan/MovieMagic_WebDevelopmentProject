using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Threading.Tasks;
using Dapper;
using CORE.Interface;
using CORE.Entities;

namespace Infra_Structure_Layer.Repositories
{
    public class BookingRepository : IBookingRepository
    {
        public async Task<List<Seat>> SeatList(int movieId, int showId)
        {
            string conn = "Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=Web_Development_Project;Integrated Security=True";
            string query = "SELECT * FROM Seat WHERE ShowId = @ShowId AND MovieId = @MovieId";

            using (var connection = new SqlConnection(conn))
            {
                var seats = await connection.QueryAsync<Seat>(query, new { ShowId = showId, MovieId = movieId });
                return seats.AsList();
            }
        }
        public async Task<int> GetLastShowId()
        {
            string conn = "Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=Web_Development_Project;Integrated Security=True";
            string query = "SELECT TOP 1 [Id] FROM [dbo].[MovieShows] ORDER BY [Id] DESC";
            using (var connection = new SqlConnection(conn))
            {
                var result = await connection.ExecuteScalarAsync<int?>(query);
                return result ?? 0;
            }
        }
        public async Task AddSeatsForShow(int movieId, int showId)
        {
            string conn = "Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=Web_Development_Project;Integrated Security=True";
            string query = "INSERT INTO Seat (MovieId, ShowId, SeatId, IsAvailable, BookingId) VALUES (@MovieId, @ShowId, @SeatNumber, @IsAvailable, @BookingId)";

            using (var connection = new SqlConnection(conn))
            {
                await connection.OpenAsync();

                for (int seatNumber = 1; seatNumber <= 107; seatNumber++)
                {
                    await connection.ExecuteAsync(query, new
                    {
                        MovieId = movieId,
                        ShowId = showId,
                        SeatNumber = seatNumber,
                        IsAvailable = true,
                        BookingId = (int?)null
                    });
                }
            }
        }
        public async Task UpdateSeats(List<int> seatIds, int movieId, int showId, int bookingId)
        {
            string conn = "Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=Web_Development_Project;Integrated Security=True";
            var seatIdsString = string.Join(",", seatIds);
            using (var connection = new SqlConnection(conn))
            {
                await connection.OpenAsync();
                string selectQuery = $"SELECT SeatId FROM Seat WHERE SeatId IN ({seatIdsString}) AND ShowId = @ShowId AND MovieId = @MovieId";
                var seatIdsToUpdate = await connection.QueryAsync<int>(selectQuery, new { ShowId = showId, MovieId = movieId });
                if (seatIdsToUpdate.AsList().Count > 0)
                {
                    string updateQuery = $"UPDATE Seat SET BookingId = @BookingId, IsAvailable = 0 WHERE SeatId IN ({string.Join(",", seatIdsToUpdate)})";
                    await connection.ExecuteAsync(updateQuery, new { BookingId = bookingId });
                }
            }
        }
    }
}






















//using System;
//using System.Collections.Generic;
//using System.Data.SqlClient;
//using System.Threading.Tasks;
//using CORE.Interface;
//using CORE.Entities;

//namespace Infra_Structure_Layer.Repositories
//{
//    public class BookingRepository : IBookingRepository
//    {
//        public async Task<List<Seat>> SeatList(int movieId, int showId)
//        {
//            List<Seat> seats = new List<Seat>();
//            string conn = "Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=Web_Development_Project;Integrated Security=True";
//            string query = "SELECT * FROM Seat WHERE ShowId = @ShowId AND MovieId = @MovieId";

//            using (var connection = new SqlConnection(conn))
//            {
//                using (var command = new SqlCommand(query, connection))
//                {
//                    command.Parameters.AddWithValue("@ShowId", showId);
//                    command.Parameters.AddWithValue("@MovieId", movieId);
//                    await connection.OpenAsync();
//                    using (SqlDataReader reader = await command.ExecuteReaderAsync())
//                    {
//                        while (await reader.ReadAsync())
//                        {
//                            var seat = new Seat
//                            {
//                                SeatId = reader.GetInt32(2),
//                                MovieId = reader.GetInt32(0),
//                                ShowId = reader.GetInt32(1),
//                                BookingId = reader.IsDBNull(3) ? (int?)null : reader.GetInt32(3),
//                                IsAvailable = reader.GetBoolean(4)
//                            };
//                            seats.Add(seat);
//                        }
//                    }
//                }
//            }
//            return seats;
//        }
//        public async Task<int> GetLastShowId()
//        {
//            string conn = "Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=Web_Development_Project;Integrated Security=True";
//            string query = "SELECT TOP 1 [Id] FROM [dbo].[MovieShows] ORDER BY [Id] DESC";

//            using (var connection = new SqlConnection(conn))
//            {
//                using (var command = new SqlCommand(query, connection))
//                {
//                    await connection.OpenAsync();
//                    var result = await command.ExecuteScalarAsync();
//                    return result != null ? Convert.ToInt32(result) : 0;
//                }
//            }
//        }

//        public async Task AddSeatsForShow(int movieId, int showId)
//        {
//            string conn = "Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=Web_Development_Project;Integrated Security=True";
//            string query = "INSERT INTO Seat (MovieId, ShowId, SeatId, IsAvailable , BookingId) VALUES (@MovieId, @ShowId, @SeatNumber, @IsAvailable , @BookingId)";

//            using (var connection = new SqlConnection(conn))
//            {
//                await connection.OpenAsync();
//                using (var command = new SqlCommand(query, connection))
//                {
//                    command.Parameters.AddWithValue("@MovieId", movieId);
//                    command.Parameters.AddWithValue("@ShowId", showId);
//                    command.Parameters.AddWithValue("@IsAvailable", true);
//                    command.Parameters.AddWithValue("@BookingId", DBNull.Value);

//                    for (int seatNumber = 1; seatNumber <= 107; seatNumber++)
//                    {
//                        command.Parameters.AddWithValue("@SeatNumber", seatNumber);
//                        await command.ExecuteNonQueryAsync();
//                        command.Parameters.RemoveAt("@SeatNumber");
//                    }
//                }
//            }
//        }

//        public async Task UpdateSeats(List<int> seatIds, int movieId, int showId, int bookingId)
//        {
//            var seatIdsString = string.Join(",", seatIds);
//            string connectionString = "Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=Web_Development_Project;Integrated Security=True";

//            using (SqlConnection conn = new SqlConnection(connectionString))
//            {
//                await conn.OpenAsync();
//                string selectQuery = $"SELECT * FROM Seat WHERE SeatId IN ({seatIdsString}) AND ShowId = @ShowId  AND MovieId = @MovieId";
//                using (SqlCommand selectCmd = new SqlCommand(selectQuery, conn))
//                {
//                    selectCmd.Parameters.AddWithValue("@ShowId", showId);
//                    selectCmd.Parameters.AddWithValue("@MovieId", movieId);
//                    using (SqlDataReader reader = await selectCmd.ExecuteReaderAsync())
//                    {
//                        var seatIdsToUpdate = new List<int>();
//                        while (await reader.ReadAsync())
//                        {
//                            seatIdsToUpdate.Add(reader.GetInt32(0));
//                        }
//                        reader.Close();
//                        if (seatIdsToUpdate.Count > 0)
//                        {
//                            string updateQuery = $"UPDATE Seat SET BookingId = @BookingId, IsAvailable = 0 WHERE SeatId IN ({string.Join(",", seatIdsToUpdate)})";

//                            using (SqlCommand updateCmd = new SqlCommand(updateQuery, conn))
//                            {
//                                updateCmd.Parameters.AddWithValue("@BookingId", bookingId);
//                                await updateCmd.ExecuteNonQueryAsync();
//                            }
//                        }
//                    }
//                }
//            }
//        }
//    }
//}