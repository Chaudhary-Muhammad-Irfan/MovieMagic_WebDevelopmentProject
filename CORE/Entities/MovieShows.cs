using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CORE.Entities
{
    public class MovieShows
    {
        public int Id { get; set; }
        public int movieId { get; set; }
        public string movieName { get; set; }
        public DateTime showDate { get; set; }
        public TimeSpan showTime { get; set; }
        public string showDay { get; set; }
    }
}
