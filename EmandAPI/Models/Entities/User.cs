using Microsoft.AspNetCore.Identity;

namespace EmandAPI.Models.Entities
{
    public class User : IdentityUser
    {
        public string FullName { get; set; }
        public DateTime DateOfBirth { get; set; }
        public string Gender { get; set; }
        public string Address { get; set; }
    }
}
