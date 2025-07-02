namespace EmandAPI.Models.DTOs
{
    public class RegisterDTO
    {
        public string FullName { get; set; }
        public DateTime DateOfBirth { get; set; }
        public string Gender { get; set; }
        public string Address { get; set; }

        public string Email { get; set; }
        public string Password { get; set; }

        public string ProfilePictureUrl { get; set; }
        public double? Latitude { get; set; }
        public double? Longitude { get; set; }
    }
}
