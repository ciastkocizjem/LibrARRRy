namespace LibrARRRy.Migrations
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;
    using LibrARRRy.Models;

    internal sealed class Configuration : DbMigrationsConfiguration<LibrARRRy.DAL.LibrARRRyContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
        }

        protected override void Seed(LibrARRRy.DAL.LibrARRRyContext context)
        {
            //  This method will be called after migrating to the latest version.

            //  You can use the DbSet<T>.AddOrUpdate() helper extension method
            //  to avoid creating duplicate seed data.

            //context.Categories.AddOrUpdate(new Models.Category { CategoryId = 1, Name = "Sci-Fi" });
            //context.Authors.AddOrUpdate(new Models.Author { AuthorId = 1, Name = "Dmitrrrry", Surname = "Glukoza" });
            //context.Books.AddOrUpdate(new Models.Book
            //{
            //    Title = "Przyszłość",
            //    AdditionDate = DateTime.Now,
            //    BookId = 1,
            //    ISBN = "9900OP",
            //    Category = context.Categories.First(),
            //    CategoryId = context.Categories.First().CategoryId,
            //    Authors = context.Authors.First().
            //});

            //IdentityManager im = new IdentityManager();
            //// Create admin
            //im.CreateUser(new ApplicationUser() { Email = "admin@librarrry.com", UserName = "Admin" }, "Admin123.");
            //im.CreateUser(new ApplicationUser() { Email = "worker@librarrry.com", UserName = "Worker" }, "Worker123.");
            //im.CreateUser(new ApplicationUser() { Email = "anita@mail.com", UserName = "Anita" }, "Anita123.");
            //// Create roles
            //im.CreateRole("admin");
            //im.CreateRole("worker");
            //im.CreateRole("reader");
            //// Assign roles
            //im.AddUserToRoleByUsername("admin@librarrry.com", "admin");
            //im.AddUserToRoleByUsername("worker@librarrry.com", "worker");
            //im.AddUserToRoleByUsername("anita@mail.com", "reader");
        }
    }
}
