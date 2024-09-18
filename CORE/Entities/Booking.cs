using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CORE.Entities
{
    public class Booking
    { 
        public int Id { get; set; }
        public int MovieId { get; set; }
        public int ShowId { get; set; }
        public string UserEmail { get; set; }
        public string SeatNumbers { get; set; }
        public int Price { get; set; }
        public DateTime BookingDate { get; set; }
    }
}
