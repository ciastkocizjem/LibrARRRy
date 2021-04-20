namespace LibrARRRy.Migrations
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

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
        }
    }
}
