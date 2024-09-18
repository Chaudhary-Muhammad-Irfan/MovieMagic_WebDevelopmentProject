using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CORE.Entities
{
    public class Seat
    {
        public int SeatId { get; set; }
        public int MovieId { get; set; }
        public int ShowId { get; set; }
        public int? BookingId { get; set; }
        public bool IsAvailable { get; set; } = true;
    }
}
