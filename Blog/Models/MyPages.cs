using Microsoft.AspNetCore.Mvc.Rendering;

namespace Blog.Models
{
    public class MyPages
    {
        public string SelectedPage { get; set; }
        public List<string> Pages { get; set; }
        public IEnumerable<SelectListItem> IPages { get; set; }
        public MyPages()
        {
            IPages = (IEnumerable<SelectListItem>)Pages;
        }
    }
}
