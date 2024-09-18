using System.ComponentModel.DataAnnotations.Schema;

namespace Web_Development_Project.Models
{
    public class Movies
    {
        public int Id { get; set; }
        public string Name { get; set; } 
        public DateTime ReleaseDate { get; set; }
        public string Genre { get; set; }
        public string Description { get; set; }
        public IFormFile Image { get; set; }
        public string ImageURL { get; set; }
        public string Trailer { get; set; }
        public List<Actors> Actors { get; set; } = new List<Actors>();
    }
}
