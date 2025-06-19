using Blog.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Runtime.CompilerServices;
using System.Security.Claims;

namespace Blog.Pages;

public class IndexModel : PageModel
{
    private readonly Data.ApplicationDbContext _context;
    public IndexModel(Data.ApplicationDbContext context)
    {
        _context = context;
    }

    [BindProperty]
    public Models.Site Site { get; set; }

    [BindProperty]
    public Models.Element Element { get; set; }

    [BindProperty]
    public Models.Site EditSite { get; set; }

    [BindProperty]
    public Models.Text EditText { get; set; }

    [BindProperty]
    public Models.Title EditTitle { get; set; }

    [BindProperty]
    public Models.Menu EditMenu { get; set; }

    [BindProperty]
    public Models.Image Image { get; set; }

    [BindProperty]
    public IFormFile AddUploadedImage { get; set; }

    [BindProperty]
    public Models.Title AddTitle { get; set; }

    [BindProperty]
    public Models.Text AddText { get; set; }

    [BindProperty]
    public Models.Menu AddMenu { get; set; }

    [BindProperty]
    public Models.Image AddImage { get; set; }

    [BindProperty]
    public ElementTypes ElementTypes { get; set; }

    public List<Models.Site> Sites { get; set; }
    public IEnumerable<SelectListItem> IPages { get; set; }
    public IEnumerable<SelectListItem> IFonts { get; set; }
    public FontSelect Fonts { get; set; }
    public List<Models.Element> Elements { get; set; }


    public void PopulateOptionsList(string user)
    {
        IEnumerable<SelectListItem> GetOptions =
            _context.Sites.Where(s => s.UserId == user).Select(s => new SelectListItem
            {
                Text = s.Title,
                Value = s.Id.ToString()
            });
        IPages = GetOptions;
    }

    public void PopulateFontsList()
    {
        IEnumerable<SelectListItem> GetFonts =
            Fonts.FontFamilies.Select(s => new SelectListItem
            {
                Text = s.Value,
                Value = s.Value
            });
        IFonts = GetFonts;
    }
    public async Task OnGetAsync(int moveIdUp, int moveIdDown, int siteId, int moveIdLeft, int moveIdRight, int removeElement, int removeSite, int selectSite)
    {
        Sites = await DAL.SiteAPIManager.GetAllSites();
        Elements = await DAL.ElementAPIManager.GetAllElements();
        Fonts = new FontSelect();

        if(User.FindFirstValue(ClaimTypes.NameIdentifier) != null)
        {
            PopulateOptionsList(User.FindFirstValue(ClaimTypes.NameIdentifier).ToString());
        }
        
        PopulateFontsList();


        if(selectSite != 0)
        {
            //Site = await DAL.SiteAPIManager.GetSite(selectSite);
            Site = Sites.Where(s => s.Id == selectSite).SingleOrDefault();
        }

        else
        {
            Site = Sites.Where(s => s.UserId == User.FindFirstValue(ClaimTypes.NameIdentifier) && s.IsStartSite).SingleOrDefault();
        }

        if (removeElement != 0)
        {

            Models.Element elementToBeRemoved = await _context.Elements.Where(e => e.Id == removeElement).SingleOrDefaultAsync();
            if(elementToBeRemoved.ElementType == ElementTypes.Image)
            {
                string fileName = "./wwwroot/userImages/" + elementToBeRemoved.Content;
                if (System.IO.File.Exists(fileName))
                {
                    System.IO.File.Delete(fileName);
                }
            }
            _context.Elements.Remove(elementToBeRemoved);
            await _context.SaveChangesAsync();


            // Re-arrange positions
            List<Models.Element> listOfElements = await _context.Elements.Where(e => e.SiteId == elementToBeRemoved.SiteId).OrderBy(e => e.Position).ToListAsync();
            int index = 1;
            foreach (var element in listOfElements)
            {
                element.Position = index;
                index++;
            }
            await _context.SaveChangesAsync();
        }

        if(removeSite != 0)
        {
            List<Models.Element> elementsToBeRemoved = await _context.Elements.Where(e => e.SiteId == removeSite).ToListAsync();
            foreach(var element in elementsToBeRemoved)
            {
                if (element.ElementType == ElementTypes.Image)
                {
                    string fileName = "./wwwroot/userImages/" + element.Content;
                    if (System.IO.File.Exists(fileName))
                    {
                        System.IO.File.Delete(fileName);
                    }
                }
                await DAL.ElementAPIManager.DeleteElement(element.Id);
            }
            await DAL.SiteAPIManager.DeleteSite(removeSite);
        }

        

        if (moveIdDown != 0)
        {
            Models.Element elementToBeMoved = await _context.Elements.Where(e => e.Id == moveIdDown).SingleOrDefaultAsync();
            Models.Element elementAtNewPosition = await _context.Elements.Where(e => e.SiteId == elementToBeMoved.SiteId && e.Position == elementToBeMoved.Position + 1).SingleOrDefaultAsync();
            elementToBeMoved.Position += 1;
            elementAtNewPosition.Position -= 1;
            await _context.SaveChangesAsync();
        }

        if (moveIdUp != 0)
        {
            Models.Element elementToBeMoved = await _context.Elements.Where(e => e.Id == moveIdUp).SingleOrDefaultAsync();
            Models.Element elementAtNewPosition = await _context.Elements.Where(e => e.SiteId == elementToBeMoved.SiteId && e.Position == elementToBeMoved.Position - 1).SingleOrDefaultAsync();
            elementToBeMoved.Position -= 1;
            elementAtNewPosition.Position += 1;
            await _context.SaveChangesAsync();
        }

        if (moveIdLeft != 0)
        {
            var menu = await _context.Elements.Where(e => e.ElementType == ElementTypes.Menu && e.SiteId == siteId).SingleOrDefaultAsync();
            var menuItemToMove = menu.MenuTitles[moveIdLeft];
            var menuLinksToMove = menu.MenuLinks[moveIdLeft];
            menu.MenuLinks.RemoveAt(moveIdLeft);
            menu.MenuTitles.RemoveAt(moveIdLeft);
            menu.MenuLinks.Insert(moveIdLeft - 1, menuLinksToMove);
            menu.MenuTitles.Insert(moveIdLeft-1, menuItemToMove);
            await _context.SaveChangesAsync();
        }

        if (moveIdRight != 0)
        {
            moveIdRight--; // eftersom jag inte vill skicka in ett 0-värde
            var menu = await _context.Elements.Where(e => e.ElementType == ElementTypes.Menu && e.SiteId == siteId).SingleOrDefaultAsync();
            var menuItemToMove = menu.MenuTitles[moveIdRight];
            var menuLinksToMove = menu.MenuLinks[moveIdRight];
            menu.MenuLinks.RemoveAt(moveIdRight);
            menu.MenuTitles.RemoveAt(moveIdRight);
            menu.MenuLinks.Insert(moveIdRight + 1, menuLinksToMove);
            menu.MenuTitles.Insert(moveIdRight + 1, menuItemToMove);
            await _context.SaveChangesAsync();
        }

        Sites = await DAL.SiteAPIManager.GetAllSites();
        Elements = await DAL.ElementAPIManager.GetAllElements();
    }

    public Element CreateElement(string content, ElementTypes type)
    {
        Element newElement = new();
        newElement.Content = content;
        newElement.SiteId = Site.Id;
        newElement.ElementType = type;
        newElement.Position = _context.Elements.Where(e => e.SiteId == Site.Id).Count() + 1;
        return newElement;
    }

    public async Task<IActionResult> OnPostAsync()
    {
        if (Site.Title != null)
        {
            Site.Date = DateTime.Now;
            Site.UserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if(_context.Sites.Where(s => s.UserId == User.FindFirstValue(ClaimTypes.NameIdentifier)).Count() == 0)
            {
                Site.IsStartSite = true;
            }
            else
            {
                Site.IsStartSite = false;

            }
                await DAL.SiteAPIManager.AddSite(Site);
        }

        if (AddTitle.Content != null)
        {
            Element newElement = CreateElement(AddTitle.Content, ElementTypes.Title);
            await DAL.ElementAPIManager.AddElement(newElement);
        }

        if (AddText.Content != null)
        {
            Element newElement = CreateElement(AddText.Content, ElementTypes.Text);
            await DAL.ElementAPIManager.AddElement(newElement);
        }

        string fileAddName = "";
        if (AddUploadedImage != null)
        {
            fileAddName = Random.Shared.Next(0, 100000).ToString() + "_" + AddUploadedImage.FileName;
            using (var fileStream = new FileStream("./wwwroot/userImages/" + fileAddName, FileMode.Create))
            {
                await AddUploadedImage.CopyToAsync(fileStream);
            }
            Element newElement = CreateElement(fileAddName, ElementTypes.Image);

            await DAL.ElementAPIManager.AddElement(newElement);
        }

        if (AddImage.Content != null)
        {
            Element newElement = CreateElement(AddImage.Content, ElementTypes.Image);
            await DAL.ElementAPIManager.AddElement(newElement);
        }

        if (!AddMenu.MenuTitles.IsNullOrEmpty())
        {
            if (!AddMenu.MenuTitles[0].IsNullOrEmpty())
            {
                Element newElement = CreateElement("Menu", ElementTypes.Menu);

                for (int i = 0; i < 3; i++)
                {
                    newElement.MenuTitles.Add(AddMenu.MenuTitles[i]);
                }

                await DAL.ElementAPIManager.AddElement(newElement);
            }
            
        }
        
        if (EditTitle != null)
        {
            //var elementToBeEdited = await _context.Elements.Where(e => e.Id == EditTitle.Id).SingleOrDefaultAsync();
            //Element newElement = CreateElement(EditTitle.Content, ElementTypes.Title);
            Element newElement = new();
            newElement.Content = EditTitle.Content;
            newElement.Id = EditTitle.Id;
            newElement.SiteId = EditTitle.SiteId;
            newElement.ElementType = ElementTypes.Title;
            newElement.Position = EditTitle.Position;
            await DAL.ElementAPIManager.UpdateElement(newElement);
        }

        if (EditText != null)
        {
            //var elementToBeEdited = await _context.Elements.Where(e => e.Id == EditText.Id).SingleOrDefaultAsync();
            //Element newElement = CreateElement(EditText.Content, ElementTypes.Text);
            Element newElement = new();
            newElement.Content = EditText.Content;
            newElement.Id = EditText.Id;
            newElement.SiteId = EditText.SiteId;
            newElement.ElementType = ElementTypes.Text;
            newElement.Position = EditText.Position;
            await DAL.ElementAPIManager.UpdateElement(newElement);
        }

        if (!EditMenu.MenuTitles.IsNullOrEmpty())
        {
            //var elementToBeEdited = await _context.Elements.Where(e => e.Id == EditMenu.Id).SingleOrDefaultAsync();
            Element newElement = new();
            newElement.Id = EditMenu.Id;

            for (int i = 0; i < 3; i++)
            {
                newElement.MenuTitles.Add(EditMenu.MenuTitles[i]);
            }
            newElement.SiteId = EditMenu.SiteId;
            newElement.ElementType = ElementTypes.Menu;
            newElement.Position = EditMenu.Position;
            await DAL.ElementAPIManager.UpdateElement(newElement);
        }

        if (!Element.MenuLinks.IsNullOrEmpty())
        {
            var elementToBeEdited = await _context.Elements.Where(e => e.Id == EditMenu.Id).SingleOrDefaultAsync();
            //Element newElement = new();
            //var existingElement = await _context.Elements.Where(e => e.Id == EditMenu.Id).SingleOrDefaultAsync();
            //newElement.MenuTitles = existingElement.MenuTitles;
            //newElement.Id = EditMenu.Id;

            for (int i = 0; i < 3; i++)
            {
                elementToBeEdited.MenuLinks.Add(Element.MenuLinks[i].ToString());
            }
            //newElement.SiteId = EditMenu.SiteId;
            //newElement.ElementType = ElementTypes.Menu;
            //newElement.Position = EditMenu.Position;
            await DAL.ElementAPIManager.UpdateElement(elementToBeEdited);
        }

        if (!Site.BackgroundColorString.IsNullOrEmpty())
        {
            var siteToBeEdited = await _context.Sites.Where(s => s.Id == Site.Id).SingleOrDefaultAsync();
            siteToBeEdited.BackgroundColorString = Site.BackgroundColorString;
            await DAL.SiteAPIManager.UpdateSite(siteToBeEdited);
        }

        if (!Site.FontColorString.IsNullOrEmpty())
        {
            var siteToBeEdited = await _context.Sites.Where(s => s.Id == Site.Id).SingleOrDefaultAsync();
            siteToBeEdited.FontColorString = Site.FontColorString;
            await DAL.SiteAPIManager.UpdateSite(siteToBeEdited);
        }

        if (!Site.FontFamilyString.IsNullOrEmpty())
        {
            var siteToBeEdited = await _context.Sites.Where(s => s.Id == Site.Id).SingleOrDefaultAsync();
            siteToBeEdited.FontFamilyString = Site.FontFamilyString;
            await DAL.SiteAPIManager.UpdateSite(siteToBeEdited);
        }

        if(!EditSite.Title.IsNullOrEmpty())
        {
            var siteToBeEdited = await _context.Sites.Where(s => s.Id == Site.Id).SingleOrDefaultAsync();
            siteToBeEdited.Title = EditSite.Title;
            await DAL.SiteAPIManager.UpdateSite(siteToBeEdited);
        }

        if(Site.IsStartSite == true)
        {
            var siteToBeEdited = await _context.Sites.Where(s => s.Id == Site.Id).SingleOrDefaultAsync();
            foreach(var site in _context.Sites.Where(s => s.UserId == User.FindFirstValue(ClaimTypes.NameIdentifier)))
            {
                site.IsStartSite = false;
                await DAL.SiteAPIManager.UpdateSite(site);
            }
            siteToBeEdited.IsStartSite = true;
            await DAL.SiteAPIManager.UpdateSite(siteToBeEdited);
        }

        return RedirectToPage("./Index");
    }
}
