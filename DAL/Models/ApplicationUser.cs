using Microsoft.AspNetCore.Identity;

namespace DAL.Models
{
    public class ApplicationUser : IdentityUser
    {
        public string Name { get; set; }
        public bool IsActive { get; set; }
        public string Image { get; set; }

    }
}
