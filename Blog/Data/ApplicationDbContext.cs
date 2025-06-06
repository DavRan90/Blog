using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Blog.Data;

public class ApplicationDbContext : IdentityDbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }

    public DbSet<Models.Site> Sites { get; set; }
    public DbSet<Models.Element> Elements { get; set; }
    public DbSet<Models.Menu> Menus { get; set; }
    //public DbSet<Models.Title> Titles { get; set; }
    //public DbSet<Models.Text> Texts { get; set; }
    //public DbSet<Models.Image> Images { get; set; }
}
