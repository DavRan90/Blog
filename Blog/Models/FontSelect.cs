namespace Blog.Models
{
    public class FontSelect
    {
        public Dictionary<int, string> FontFamilies { get; set; }
        public FontSelect()
        {
            var fontFamilies = new Dictionary<int, string>()
            {
                { 1, "'Franklin Gothic Medium'" },
                { 2, "'Gill Sans', 'Gill Sans MT', Calibri, 'Trebuchet MS', sans-serif" },
                { 3, "'Lucida Sans', 'Lucida Sans Regular', 'Lucida Grande', 'Lucida Sans Unicode', Geneva, Verdana, sans-serif" },
                { 4, "'Segoe UI', Tahoma, Geneva, Verdana, sans-serif" },
                { 5, "'Times New Roman', Times, serif" }
            };
            FontFamilies = fontFamilies;
        }
    }
}
