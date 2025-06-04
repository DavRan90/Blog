namespace Blog.Models
{
    public class Element
    {
        public int Id { get; set; }
        public int? Position { get; set; }
        //public string? Title { get; set; }
        //public string? Text { get; set; }
        //public string? Image { get; set; }
        public string? Content { get; set; }
        public int? SiteId { get; set; }
    }
}