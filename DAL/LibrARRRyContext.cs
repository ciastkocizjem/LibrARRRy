using LibrARRRy.Models;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace LibrARRRy.DAL
{
    public class LibrARRRyContext : IdentityDbContext<ApplicationUser>
    {
        public LibrARRRyContext()
            : base("LibrARRRyDB", throwIfV1Schema: false)
        {
            
        }

        public DbSet<Author> Authors { get; set; }
        public DbSet<Book> Books { get; set; }
        public DbSet<Tag> Tags { get; set; }
        public DbSet<State> States { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<User> OurUsers { get; set; }
        public DbSet<Worker> Workers { get; set; }
        public DbSet<Reader> Readers { get; set; }
        public DbSet<Search> Searches { get; set; }
        public DbSet<Storage> Storages { get; set; }

        public static LibrARRRyContext Create()
        {
            return new LibrARRRyContext();
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<Storage>();
        }
    }
}