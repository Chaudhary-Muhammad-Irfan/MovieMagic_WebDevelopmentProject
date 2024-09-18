using System.ComponentModel.DataAnnotations.Schema;

public class Actors
{
    public int MovieId { get; set; }
    public string ActorName { get; set; }
    public string ImageURL { get; set; }
    public IFormFile Image { get; set; }
}
