using CORE.Entities;
using CORE.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application_Layer.Services
{
    public class BookingService 
    {
        private readonly IBookingRepository repo;
        public BookingService(IBookingRepository repo)
        {
            this.repo = repo;
        }
        public async Task<List<Seat>> SeatList(int movieId, int showId)
        {
            return await repo.SeatList(movieId, showId);
        }
        public async Task<int> GetLastShowId()
        {
            return await repo.GetLastShowId();
        }
        public async Task AddSeatsForShow(int movieId, int showId)
        {
            await repo.AddSeatsForShow(movieId, showId);
        }
        public async Task UpdateSeats(List<int> seatIds, int movieId, int showId, int bookingId)
        {
            await repo.UpdateSeats(seatIds, movieId, showId, bookingId);
        }


    }
}
