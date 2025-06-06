namespace Blog.Models
{
    public class Menu : Element
    {
        public int Id { get; set; }
        public int NumberOfTitles { get; set; }
        public List<string> MenuTitles { get; set; } = new(3);
        public List<string> MenuLink { get; set; }
    }
}
