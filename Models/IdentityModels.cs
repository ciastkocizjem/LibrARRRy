using System.Data.Entity;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;

namespace LibrARRRy.Models
{
    // You can add profile data for the user by adding more properties to your ApplicationUser class, please visit https://go.microsoft.com/fwlink/?LinkID=317594 to learn more.
    public class ApplicationUser : IdentityUser
    {
        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<ApplicationUser> manager)
        {
            // Note the authenticationType must match the one defined in CookieAuthenticationOptions.AuthenticationType
            var userIdentity = await manager.CreateIdentityAsync(this, DefaultAuthenticationTypes.ApplicationCookie);
            // Add custom user claims here
            return userIdentity;
        }
    }

    //public class LibrARRRyContext : IdentityDbContext<ApplicationUser>
    //{
    //    public LibrARRRyContext()
    //        : base("LibrARRRyDB", throwIfV1Schema: false)
    //    {
    //    }

    //    public DbSet<Author> Authors { get; set; }
    //    public DbSet<Book> Books { get; set; }
    //    public DbSet<Tag> Tags { get; set; }
    //    public DbSet<State> States { get; set; }
    //    public DbSet<Category> Categories { get; set; }
    //    public DbSet<User> Users { get; set; } //To przykrywa userów z Identity!! (coś trzeba z tym zrobić, lol)
    //    public DbSet<Worker> Workers { get; set; }
    //    public DbSet<Reader> Readers { get; set; }
    //    public DbSet<Search> Searches { get; set; }

    //    public static LibrARRRyContext Create()
    //    {
    //        return new LibrARRRyContext();
    //    }
    //}
}