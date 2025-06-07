using System.Text.Json.Serialization;

namespace Blog.Models
{
    public class Element
    {
        [JsonPropertyName("id")]
        public int Id { get; set; }

        [JsonPropertyName("backgroundColor")]
        public ConsoleColor BackgroundColor { get; set; }

        [JsonPropertyName("position")]
        public int? Position { get; set; }

        [JsonPropertyName("content")]
        public string? Content { get; set; }

        [JsonPropertyName("menuTitles")]
        public List<string>? MenuTitles { get; set; } = new(3);

        [JsonPropertyName("elementType")]
        public ElementTypes ElementType { get; set; }

        [JsonPropertyName("siteId")]
        public int? SiteId { get; set; }
    }
}