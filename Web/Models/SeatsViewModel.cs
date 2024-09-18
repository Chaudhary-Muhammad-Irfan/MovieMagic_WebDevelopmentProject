namespace Web_Development_Project.Models
{
    public class SeatsViewModel
    {
        public int MovieId { get; set; }
        public int ShowId { get; set; }
        public List<CORE.Entities.Seat> Seats { get; set; } = new List<CORE.Entities.Seat>();
    }
}
