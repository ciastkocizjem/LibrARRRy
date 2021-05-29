namespace LibrARRRy.Migrations
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Diagnostics;
    using System.Linq;
    using LibrARRRy.Models;
    using Microsoft.AspNet.Identity;
    using Microsoft.AspNet.Identity.EntityFramework;

    internal sealed class Configuration : DbMigrationsConfiguration<LibrARRRy.DAL.LibrARRRyContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
        }

        protected override void Seed(LibrARRRy.DAL.LibrARRRyContext context)
        {
            // Creating roles, users and assigning roles to users
            string[] roles = { "admin", "worker", "reader" };
            var userManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(context));
            foreach (string role in roles)
            {
                // Creating roles
                if (!context.Roles.Any(r => r.Name == role))
                {
                    context.Roles.Add(new IdentityRole(role));
                }
                // Creating user for role
                string email = $"{role}@{role}.com";
                if (!context.Users.Any(u => u.Email == email))
                {
                    string password = role.First().ToString().ToUpper() + role.Substring(1);
                    ApplicationUser user = new ApplicationUser
                    {
                        Email = email,
                        PasswordHash = userManager.PasswordHasher.HashPassword($"{password}123."),
                        EmailConfirmed = true,
                        SecurityStamp = Guid.NewGuid().ToString("D"),
                        UserName = email,
                    };
                    userManager.Create(user);
                    // Add user to role
                    userManager.AddToRole(user.Id, role);
                }
            }

            // Creating first book and components needed
            Author author = new Author { Name = "Dmitry", Surname = "Glukhovsky" };
            Tag tag = new Tag { Name = "Przyszłość" };
            Category category = new Category { Name = "Fantastyka" };

            Book book = new Book { Title = "Futu.re", Description = "Pokonaliśmy śmierć. I co dalej?", ISBN = "14842061", AdditionDate = DateTime.Now, CategoryId = category.CategoryId };
            book.Authors = new List<Author>();
            book.Authors.Add(author);
            book.Tags = new List<Tag>();
            book.Tags.Add(tag);

            if (!context.Authors.Any(a => a.Surname == author.Surname && a.Name == author.Name))
            {
                context.Authors.Add(author);
            }
            if (!context.Tags.Any(t => t.Name == tag.Name))
            {
                context.Tags.Add(tag);
            }
            if (!context.Categories.Any(c => c.Name == category.Name))
            {
                context.Categories.Add(category);
            }

            if (!context.Books.Any(b => b.Title == book.Title && b.ISBN == book.ISBN))
            {
                context.Books.Add(book);
            }

            context.SaveChanges();
        }
    }
}
