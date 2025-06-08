using System.Drawing;
using System.Text.Json.Serialization;

namespace Blog.Models
{
    public class Site
    {
        [JsonPropertyName("id")]
        public int Id { get; set; }
        [JsonPropertyName("backgroundColorString")]
        public string? BackgroundColorString { get; set; }
        //public ConsoleColor BackgroundColor { get; set; }

        [JsonPropertyName("title")]
        public string? Title { get; set; }
        [JsonPropertyName("elements")]
        public List<Element>? Elements { get; set; }
        [JsonPropertyName("userId")]
        public string? UserId { get; set; }
        [JsonPropertyName("date")]
        public DateTime? Date { get; set; }
    }
}

