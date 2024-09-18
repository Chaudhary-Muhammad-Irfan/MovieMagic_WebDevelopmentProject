using CORE.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CORE.Interface
{
    public interface IBookingRepository 
    {
        Task<List<Seat>> SeatList(int movieId, int showId);
        Task<int> GetLastShowId();
        Task AddSeatsForShow(int movieId, int showId);
        Task UpdateSeats(List<int> seatIds, int movieId, int showId, int bookingId);
    }
}