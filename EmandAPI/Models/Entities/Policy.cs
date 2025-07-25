﻿namespace EmandAPI.Models.Entities
{
    public class Policy
    {
        public int Id { get; set; }
        public string PolicyName { get; set; }
        public string CoverageDetails { get; set; }
        public decimal Premium { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string UserId { get; set; }
        public User User { get; set; }
    }
}
