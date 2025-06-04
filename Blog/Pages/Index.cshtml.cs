using Blog.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
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
    public Models.Image Image { get; set; }

    [BindProperty]
    public IFormFile UploadedImage { get; set; }

    [BindProperty]
    public ElementTypes ElementTypes { get; set; }

    public List<Models.Site> Sites { get; set; }

    public List<Models.Element> Elements { get; set; }


    public async Task OnGetAsync(int deleteId, int moveIdUp, int moveIdDown, int removeElement, int editElement)
    {
        if (removeElement > 0)
        {
            Models.Element elementToBeRemoved = await _context.Elements.Where(e => e.Position == removeElement).SingleOrDefaultAsync();
            _context.Elements.Remove(elementToBeRemoved);
            await _context.SaveChangesAsync();
            List<Models.Element> listOfElements = await _context.Elements.OrderBy(e => e.Position).ToListAsync();
            int index = 1;
            foreach (var element in listOfElements)
            {
                element.Position = index;
                index++;
            }
            await _context.SaveChangesAsync();
        }

        //if(editElement > 0)
        //{
        //    Models.Element elementToBeEdited = await _context.Elements.Where(e => e.Position == editElement).SingleOrDefaultAsync();
        //    elementToBeEdited.Content = Title.Content;
        //    await _context.SaveChangesAsync();
        //}


        //if(addElement == 1)
        //{
        //    Title.SiteId = 21;
        //    Title.Position = _context.Elements.Count() + 1;
        //    _context.Elements.Add(Title);
        //    await _context.SaveChangesAsync();
        //}

        if (moveIdDown > 0 && moveIdDown < _context.Elements.Count())
        {
            Models.Element elementToBeMoved = await _context.Elements.Where(e => e.Position == moveIdDown).SingleOrDefaultAsync();
            Models.Element elementAtNewPosition = await _context.Elements.Where(e => e.Position == moveIdDown + 1).SingleOrDefaultAsync();
            elementToBeMoved.Position += 1;
            elementAtNewPosition.Position -= 1;
            await _context.SaveChangesAsync();
        }

        if (moveIdUp >= 1 && moveIdUp <= _context.Elements.Count())
        {
            Models.Element elementToBeMoved = await _context.Elements.Where(e => e.Position == moveIdUp).SingleOrDefaultAsync();
            Models.Element elementAtNewPosition = await _context.Elements.Where(e => e.Position == moveIdUp - 1).SingleOrDefaultAsync();
            elementToBeMoved.Position -= 1;
            elementAtNewPosition.Position += 1;
            await _context.SaveChangesAsync();
        }

        if (deleteId != 0)
        {
            Models.Site siteToBeDeleted = await _context.Sites.FindAsync(deleteId);
            List<Models.Element> elementsToBeDeleted = await _context.Elements.Where(e => e.SiteId == deleteId).ToListAsync();
            foreach (var element in elementsToBeDeleted)
            {
                _context.Elements.Remove(element);
                string fileName = "./wwwroot/userImages/" + elementsToBeDeleted;
                if (System.IO.File.Exists(fileName))
                {
                    System.IO.File.Delete(fileName);
                }
                await _context.SaveChangesAsync();
            }
            if (siteToBeDeleted != null /*&& User.FindFirstValue(ClaimTypes.NameIdentifier) == elementToBeDeleted.UserId*/)
            {
                string fileName = "./wwwroot/userImages/" + siteToBeDeleted;
                if (System.IO.File.Exists(fileName))
                {
                    System.IO.File.Delete(fileName);
                }
                _context.Sites.Remove(siteToBeDeleted);
                await _context.SaveChangesAsync();
            }
        }
        Elements = await _context.Elements.ToListAsync();
        Sites = await _context.Sites.ToListAsync();
    }

    public async Task<IActionResult> OnPostAsync()
    {
        //Site.UserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        //_context.Pages.Add(Site);
        //await _context.SaveChangesAsync();
        Site.Id = 21;
        string fileName = "";
        if (UploadedImage != null)
        {
            fileName = Random.Shared.Next(0, 100000).ToString() + "_" + UploadedImage.FileName;
            using (var fileStream = new FileStream("./wwwroot/userImages/" + fileName, FileMode.Create))
            {
                await UploadedImage.CopyToAsync(fileStream);
            }
            Image.Content = fileName;
            Image.Position = _context.Elements.Count() + 1;
            Image.SiteId = Site.Id;
            _context.Elements.Add(Image);
            await _context.SaveChangesAsync();
            //"./wwwroot/userImages/
        }

        // Add Title
        if (Title.Content != null)
        {

            Title.SiteId = Site.Id;
            Title.Position = _context.Elements.Count() + 1;
            _context.Elements.Add(Title);
            await _context.SaveChangesAsync();
        }

        // Add Text
        if (Text.Content != null)
        {
            Text.SiteId = Site.Id;
            Text.Position = _context.Elements.Count() + 1;
            _context.Elements.Add(Text);
            await _context.SaveChangesAsync();
        }

        return RedirectToPage("./Index");
    }
}
