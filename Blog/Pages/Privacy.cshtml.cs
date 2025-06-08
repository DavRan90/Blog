using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Blog.Pages;

public class PrivacyModel : PageModel
{
    public List<Models.Site> Sites { get; set; }

    public List<Models.Element> Elements { get; set; }

    public async Task OnGetAsync()
    {
        Sites = await DAL.SiteAPIManager.GetAllSites();
        Elements = await DAL.ElementAPIManager.GetAllElements();
    }
}

