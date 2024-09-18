using Application_Layer.Services;
using CORE.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using Web.Models;
using Web_Development_Project.Data;
using Web_Development_Project.Models;
namespace Web_Development_Project.Controllers
{
    public class BookingController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<AddNewFields> _userManager;
        private readonly ILogger<BookingController> _logger;
        private readonly BookingService _booking;
        private readonly IHubContext<Infra_Structure_Layer.Hubs.BookingHub> _hubContext;
        public BookingController(ApplicationDbContext context, BookingService booking, UserManager<AddNewFields> userManager, ILogger<BookingController> logger , IHubContext<Infra_Structure_Layer.Hubs.BookingHub> hubContext)
        {
            _context = context;
            _userManager = userManager;
            _logger = logger;
            _booking = booking;
            _hubContext = hubContext;
        }
        [Authorize]
        public async Task<IActionResult> BookTicket(int movieId, int showId)
        {
            var seats = await _booking.SeatList(movieId, showId);
            var viewModel = new SeatsViewModel
            {
                MovieId = movieId,
                ShowId = showId,
                Seats = seats
            };
            return View(viewModel);
        }
        [HttpPost]
        public async Task<IActionResult> ConfirmBooking([FromBody] BookingViewModel model)
        {
            using (var transaction = _context.Database.BeginTransaction())
            {
                try
                {
                    var booking = new Booking
                    {
                        UserEmail = model.UserEmail,
                        ShowId = model.ShowId,
                        Price = model.TotalPrice,
                        BookingDate = DateTime.UtcNow,
                        MovieId = model.MovieId,
                        SeatNumbers = model.SeatNumbers
                    };
                    _context.Bookings.Add(booking);
                    _context.SaveChanges();
                    var bookingId = booking.Id;
                    var seatIds = model.SeatNumbers
                        .Split(',')
                        .Select(id => int.TryParse(id, out var seatId) ? seatId : (int?)null)
                        .Where(id => id.HasValue)
                        .Select(id => id.Value)
                        .ToList();
                    var seats = _context.Seat
                        .Where(s => seatIds.Contains(s.SeatId) && s.ShowId == model.ShowId && s.MovieId == model.MovieId)
                        .ToList();
                    foreach (var seat in seats)
                    {
                        seat.BookingId = bookingId;
                        seat.IsAvailable = false;
                    }
                    _context.SaveChanges();
                    transaction.Commit();
                    await _hubContext.Clients.All.SendAsync("BookingUpdate", model.MovieId, model.ShowId,seatIds, true);
                    return Json(new { success = true, bookingId = bookingId });
                }
                catch (Exception ex)
                {
                    return View();
                }
            }
        }
    }
}