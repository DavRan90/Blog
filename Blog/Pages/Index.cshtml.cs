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
    public Models.Title Title { get; set; }

    [BindProperty]
    public Models.Text Text { get; set; }

    

    [BindProperty]
    public Models.Text EditText { get; set; }

    [BindProperty]
    public Models.Title EditTitle { get; set; }

    [BindProperty]
    public Models.Image EditImage { get; set; }

    [BindProperty]
    public Models.Menu EditMenu { get; set; }

    [BindProperty]
    public Models.Image Image { get; set; }

    [BindProperty]
    public IFormFile UploadedImage { get; set; }


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
    public SelectListItem Item { get; set; }
    public List<string> MyPagesList { get; set; }
    public MyPages MyPages { get; set; }
    public List<Models.Element> Elements { get; set; }

    dynamic ViewBag = new System.Dynamic.ExpandoObject();


    public void PopulateOptionsList(string user)
    {
        IEnumerable<SelectListItem> GetOptions =
            _context.Sites.Where(s => s.UserId == user).Select(s => new SelectListItem
            {
                Text = s.Title,
                Value = s.Id.ToString()
            });
        IPages = GetOptions;
        //ViewBag.OptionsList = GetOptions;
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

    public IEnumerable<SelectListItem> GetSites()
    {
        List<string> myPages = new();
        foreach (var site in Sites)
        {
            myPages.Add(site.Title);
        }
        return (IEnumerable<SelectListItem>)myPages;

    }
    public async Task OnGetAsync(int deleteId, int moveIdUp, int moveIdDown, int moveIdLeft, int moveIdRight, int removeElement, int editElement, int removeSite)
    {
        Sites = await DAL.SiteAPIManager.GetAllSites();
        Elements = await DAL.ElementAPIManager.GetAllElements();
        Fonts = new FontSelect();
        //foreach (var site in Sites)
        //{
        //    MyPages.Pages.Add(site.Title);
        //}
        if(User.FindFirstValue(ClaimTypes.NameIdentifier) != null)
        {
            PopulateOptionsList(User.FindFirstValue(ClaimTypes.NameIdentifier).ToString());
        }
        
        PopulateFontsList();


        //Item.Text = "Text";
        //Item.Value = "Value";
        //IPages.Append(Item);

        //Option = GetOptions();

        //IEnumerable<SelectListItem> ListPages =
        //    Elements.Select(i => new SelectListItem
        //    {
        //        Text = "Text 1",
        //        Value = "Value 1"
        //    });

        //Pages = ListPages;



        //Elements = await _context.Elements.ToListAsync();
        //Sites = await _context.Sites.ToListAsync();


        if (removeElement > 0)
        {
            
            //Models.Element elementToBeRemoved = await _context.Elements.Where(e => e.Id == removeElement).SingleOrDefaultAsync();
            //_context.Elements.Remove(elementToBeRemoved);
            //await _context.SaveChangesAsync();


            // Re-arrange positions
            List<Models.Element> listOfElements = await _context.Elements.Where(e => e.SiteId == Site.Id).OrderBy(e => e.Position).ToListAsync();
            int index = 1;
            foreach (var element in listOfElements)
            {
                element.Position = index;
                index++;
            }
            await _context.SaveChangesAsync();
        }

        if(removeSite > 0)
        {
            List<Models.Element> elementsToBeRemoved = await _context.Elements.Where(e => e.SiteId == removeSite).ToListAsync();
            foreach(var element in elementsToBeRemoved)
            {
                await DAL.ElementAPIManager.DeleteElement(element.Id);
            }
            await DAL.SiteAPIManager.DeleteSite(removeSite);
        }

        

        if (moveIdDown > 0 /*&& moveIdDown < _context.Elements.Count()*/)
        {
            Models.Element elementToBeMoved = await _context.Elements.Where(e => e.Id == moveIdDown).SingleOrDefaultAsync();
            Models.Element elementAtNewPosition = await _context.Elements.Where(e => e.SiteId == elementToBeMoved.SiteId && e.Position == elementToBeMoved.Position + 1).SingleOrDefaultAsync();
            elementToBeMoved.Position += 1;
            elementAtNewPosition.Position -= 1;
            await _context.SaveChangesAsync();
        }

        if (moveIdUp > 0 /*&& moveIdUp <= _context.Elements.Count()*/)
        {
            Models.Element elementToBeMoved = await _context.Elements.Where(e => e.Id == moveIdUp).SingleOrDefaultAsync();
            Models.Element elementAtNewPosition = await _context.Elements.Where(e => e.SiteId == elementToBeMoved.SiteId && e.Position == elementToBeMoved.Position - 1).SingleOrDefaultAsync();
            elementToBeMoved.Position -= 1;
            elementAtNewPosition.Position += 1;
            await _context.SaveChangesAsync();
        }

        if (moveIdLeft > 0)
        {
            var menu = await _context.Elements.Where(e => e.ElementType == ElementTypes.Menu).SingleOrDefaultAsync();
            var menuItemToMove = menu.MenuTitles[moveIdLeft];
            var menuLinksToMove = menu.MenuLinks[moveIdLeft];
            menu.MenuLinks.RemoveAt(moveIdLeft);
            menu.MenuTitles.RemoveAt(moveIdLeft);
            menu.MenuLinks.Insert(moveIdLeft - 1, menuLinksToMove);
            menu.MenuTitles.Insert(moveIdLeft-1, menuItemToMove);
            await _context.SaveChangesAsync();
        }

        if (moveIdRight > 0)
        {
            moveIdRight--; // eftersom jag inte vill skicka in ett 0-värde
            var menu = await _context.Elements.Where(e => e.ElementType == ElementTypes.Menu).SingleOrDefaultAsync();
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
        //PopulateList();
        //if (deleteId != 0)
        //{
        //    Models.Site siteToBeDeleted = await _context.Sites.FindAsync(deleteId);
        //    List<Models.Element> elementsToBeDeleted = await _context.Elements.Where(e => e.SiteId == deleteId).ToListAsync();
        //    foreach (var element in elementsToBeDeleted)
        //    {
        //        _context.Elements.Remove(element);
        //        string fileName = "./wwwroot/userImages/" + elementsToBeDeleted;
        //        if (System.IO.File.Exists(fileName))
        //        {
        //            System.IO.File.Delete(fileName);
        //        }
        //        await _context.SaveChangesAsync();
        //    }
        //    if (siteToBeDeleted != null /*&& User.FindFirstValue(ClaimTypes.NameIdentifier) == elementToBeDeleted.UserId*/)
        //    {
        //        string fileName = "./wwwroot/userImages/" + siteToBeDeleted;
        //        if (System.IO.File.Exists(fileName))
        //        {
        //            System.IO.File.Delete(fileName);
        //        }
        //        _context.Sites.Remove(siteToBeDeleted);
        //        await _context.SaveChangesAsync();
        //    }
        //}
        //Elements = await _context.Elements.ToListAsync();
        //Sites = await _context.Sites.ToListAsync();
    }

    public async Task<IActionResult> OnPostAsync()
    {
        //Site.UserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        //_context.Pages.Add(Site);
        //await _context.SaveChangesAsync();

        //Site.Id = 21;

        if(Site.Title != null)
        {
            Site.UserId = "CurrentUser";
            Site.Date = DateTime.Now;
            Site.UserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            await DAL.SiteAPIManager.AddSite(Site);
        }

        string fileName = "";
        if (UploadedImage != null)
        {
            fileName = Random.Shared.Next(0, 100000).ToString() + "_" + UploadedImage.FileName;
            using (var fileStream = new FileStream("./wwwroot/userImages/" + fileName, FileMode.Create))
            {
                await UploadedImage.CopyToAsync(fileStream);
            }
            Image.Content = fileName;
            Image.Position = _context.Elements.Where(e => e.Id == Site.Id).Count() + 1;
            Image.SiteId = Site.Id;
            _context.Elements.Add(Image);
            await _context.SaveChangesAsync();
            //"./wwwroot/userImages/
        }

        // Add Title
        if (Title.Content != null)
        {

            Title.SiteId = Site.Id;
            Title.Position = _context.Elements.Where(e => e.Id == Site.Id).Count() + 1;
            _context.Elements.Add(Title);
            await _context.SaveChangesAsync();
        }

        // Add Text
        if (Text.Content != null)
        {
            Text.SiteId = Site.Id;
            Text.Position = _context.Elements.Where(e => e.SiteId == Site.Id).Count() + 1;
            _context.Elements.Add(Text);
            await _context.SaveChangesAsync();
        }

        if (AddTitle.Content != null)
        {

            Element newElement = new();
            newElement.Content = AddTitle.Content;
            newElement.SiteId = Site.Id;
            newElement.ElementType = ElementTypes.Title;
            newElement.Position = _context.Elements.Where(e => e.SiteId == Site.Id).Count() + 1;
            await DAL.ElementAPIManager.AddElement(newElement);
        }

        if (AddText.Content != null)
        {
            Element newElement = new();
            newElement.Content = AddText.Content;
            newElement.SiteId = Site.Id;
            newElement.ElementType = ElementTypes.Text;
            newElement.Position = _context.Elements.Where(e => e.SiteId == Site.Id).Count() + 1;
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
            Element newElement = new();
            newElement.Content = fileAddName;
            newElement.SiteId = Site.Id;
            newElement.ElementType = ElementTypes.Image;
            newElement.Position = _context.Elements.Where(e => e.SiteId == Site.Id).Count() + 1;

            await DAL.ElementAPIManager.AddElement(newElement);
            //_context.Elements.Add(Image);
            //await _context.SaveChangesAsync();
            //"./wwwroot/userImages/
        }

        if (AddImage.Content != null)
        {
            Element newElement = new();
            newElement.Content = AddImage.Content;
            newElement.SiteId = Site.Id;
            newElement.ElementType = ElementTypes.Image;
            newElement.Position = _context.Elements.Where(e => e.SiteId == Site.Id).Count() + 1;
            await DAL.ElementAPIManager.AddElement(newElement);
        }

        if (!AddMenu.MenuTitles.IsNullOrEmpty())
        {
            if (!AddMenu.MenuTitles[0].IsNullOrEmpty())
            {
                Element newMenu = new();

                for (int i = 0; i < 3; i++)
                {
                    newMenu.MenuTitles.Add(AddMenu.MenuTitles[i]);
                }

                newMenu.Position = _context.Elements.Where(e => e.SiteId == Site.Id).Count() + 1;
                newMenu.ElementType = ElementTypes.Menu;
                newMenu.SiteId = Site.Id;

                await DAL.ElementAPIManager.AddElement(newMenu);
            }
            
        }

        if (EditTitle != null)
        {
            Element newElement = new();
            newElement.Id = EditTitle.Id;
            newElement.Content = EditTitle.Content;
            newElement.SiteId = EditTitle.SiteId;
            newElement.ElementType = ElementTypes.Title;
            newElement.Position = EditTitle.Position;
            await DAL.ElementAPIManager.UpdateElement(newElement);
        }

        if (EditText != null)
        {
            Element newElement = new();
            newElement.Id = EditText.Id;
            newElement.Content = EditText.Content;
            newElement.SiteId = EditText.SiteId;
            newElement.ElementType = ElementTypes.Text;
            newElement.Position = EditText.Position;
            await DAL.ElementAPIManager.UpdateElement(newElement);
        }

        if (!EditMenu.MenuTitles.IsNullOrEmpty())
        {
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

        if (!EditMenu.MenuLinks.IsNullOrEmpty())
        {
            Element newElement = new();
            var existingElement = await _context.Elements.Where(e => e.Id == EditMenu.Id).SingleOrDefaultAsync();
            newElement.MenuTitles = existingElement.MenuTitles;
            newElement.Id = EditMenu.Id;

            for (int i = 0; i < 3; i++)
            {
                newElement.MenuLinks.Add(EditMenu.MenuLinks[i].ToString());
            }
            newElement.SiteId = EditMenu.SiteId;
            newElement.ElementType = ElementTypes.Menu;
            newElement.Position = EditMenu.Position;
            await DAL.ElementAPIManager.UpdateElement(newElement);
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

        return RedirectToPage("./Index");
    }
}
