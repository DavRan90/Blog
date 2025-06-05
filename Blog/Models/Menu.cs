using Blog.Models;

namespace SiteAPI.Models
{
    public class Menu : Element
    {
        public int Id { get; set; }
        public List<string> MenuTitles { get; set; }
        public List<string> MenuLink { get; set; }
    }
}
