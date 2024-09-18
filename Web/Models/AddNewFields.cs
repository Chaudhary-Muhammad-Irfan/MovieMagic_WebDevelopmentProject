using Microsoft.AspNetCore.Identity;

namespace Web_Development_Project.Models
{
    public class AddNewFields : IdentityUser
    {
        public string Name { get; set; }
        public string Country { get; set; }
    }
}
