using Blog.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace Blog.Pages;

public class PrivacyModel : PageModel
{
    private readonly Data.ApplicationDbContext _context;
    public PrivacyModel(Data.ApplicationDbContext context)
    {
        _context = context;
    }
    public List<Models.Site> Sites { get; set; }

    public List<Models.Element> Elements { get; set; }

    public Site SiteToShow { get; set; }
    public FontSelect Fonts { get; set; }


    public async Task OnGetAsync(int showPage)
    {
        Sites = await DAL.SiteAPIManager.GetAllSites();
        Elements = await DAL.ElementAPIManager.GetAllElements();
        Fonts = new FontSelect();

        if (showPage > 0)
        {
            SiteToShow = Sites.Where(s => s.Id == showPage).SingleOrDefault();
        }
        else
        {
            SiteToShow = Sites.Where(s => s.UserId == User.FindFirstValue(ClaimTypes.NameIdentifier)).FirstOrDefault();
            //SiteToShow = Sites.Where(s => s.Id == 1 && User.FindFirstValue(ClaimTypes.NameIdentifier) == s.UserId).SingleOrDefault();
        }

        
    }
}

