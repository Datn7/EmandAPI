namespace EmandAPI.Models.DTOs
{
    public class CreateClaimDTO
    {
        public int PolicyId { get; set; }
        public string Description { get; set; }
        public decimal Amount { get; set; }
    }
}
