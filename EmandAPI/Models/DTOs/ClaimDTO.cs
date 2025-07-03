namespace EmandAPI.Models.DTOs
{
    public class ClaimDTO
    {
        public int Id { get; set; }
        public int PolicyId { get; set; }
        public string Description { get; set; }
        public decimal Amount { get; set; }
        public string Status { get; set; }
        public DateTime SubmittedAt { get; set; }
    }
}
