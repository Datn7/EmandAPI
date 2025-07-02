namespace EmandAPI.Models.Entities
{
    public class Claim
    {
        public int Id { get; set; }
        public string Description { get; set; }
        public decimal Amount { get; set; }
        public string Status { get; set; } = "Submitted";
        public DateTime SubmittedAt { get; set; } = DateTime.UtcNow;

        public int PolicyId { get; set; }
        public Policy Policy { get; set; }
    }
}
