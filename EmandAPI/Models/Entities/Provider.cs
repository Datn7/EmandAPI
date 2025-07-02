namespace EmandAPI.Models.Entities
{
    public class Provider
    {
        public int Id { get; set; }
        public string ProviderName { get; set; }
        public string Specialty { get; set; }
        public string Address { get; set; }
        public string Contact { get; set; }
        public string WorkingHours { get; set; }
    }
}
