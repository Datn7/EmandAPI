namespace EmandAPI.Models.DTOs
{
    public class CreateHealthArticleDTO
    {
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string Content { get; set; } = string.Empty;
        public string Category { get; set; } = string.Empty;
        public List<string> Tags { get; set; } = new();
        public string ImageUrl { get; set; } = string.Empty;
        public int ReadTimeMinutes { get; set; }

    }
}
