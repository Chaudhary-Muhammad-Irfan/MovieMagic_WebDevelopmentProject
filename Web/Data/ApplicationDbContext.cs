using CORE.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Web_Development_Project.Models;

namespace Web_Development_Project.Data
{ 
    public class ApplicationDbContext : IdentityDbContext <AddNewFields>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
        public DbSet<Seat> Seat { get; set; }
        public DbSet<Booking> Bookings { get; set; }
        public DbSet<MovieShows> Shows { get; set; }

    }
}
