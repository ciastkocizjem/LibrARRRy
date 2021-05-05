﻿using LibrARRRy.DAL;
using LibrARRRy.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace LibrARRRy.Controllers
{
    public class HomeController : Controller
    {
        private LibrARRRyContext db = new LibrARRRyContext();

        
        private List<string> CategoriesCheckBoxes { get; set; }

        public ActionResult Index()
        {
            var books = db.Books.OrderBy(b => b.Title);
            var categories = db.Categories.OrderBy(c => c.Name);
            var tags = db.Tags.OrderBy(t => t.Name);
            

            ViewBag.BooksList = books.ToList();
            ViewBag.CategoriesList = categories.ToList();
            ViewBag.TagsList = tags.ToList();

            return View();
        }

        [HttpPost]
        public ActionResult Index(FormCollection formCollection)
        {
            var books = db.Books;
            var categories = db.Categories.OrderBy(c => c.Name);
            var tags = db.Tags.OrderBy(t => t.Name);

            List<string> selectedCategories = new List<string>();
            List<string> selectedTags = new List<string>();

            foreach (var item in formCollection.AllKeys)
            {
                if ((bool)this.ValueProvider.GetValue(item).ConvertTo(typeof(bool)))
                {
                    if (item.StartsWith("C_"))
                    {
                        selectedCategories.Add(item.Replace("C_", ""));
                    }
                    else
                    {
                        if (item.StartsWith("T_"))
                        {
                            selectedTags.Add(item.Replace("T_", ""));
                        }
                    }
                }
            }

            List<Book> selectedBooks = new List<Book>();

            foreach (Book book in books)
            {
                if (selectedCategories.Contains(book.Category.Name))
                {
                    selectedBooks.Add(book);
                    continue;
                }

                if (selectedTags.Any(t => book.Tags.Any(bt => bt.Name == t)))
                {
                    selectedBooks.Add(book);
                    continue;
                }
            }

            ViewBag.BooksList = selectedBooks;
            ViewBag.CategoriesList = categories.ToList();
            ViewBag.TagsList = tags.ToList();

            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}