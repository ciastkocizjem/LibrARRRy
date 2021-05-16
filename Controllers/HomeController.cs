using LibrARRRy.DAL;
using LibrARRRy.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace LibrARRRy.Controllers
{
    public class HomeController : Controller
    {
        private LibrARRRyContext db = new LibrARRRyContext();
        private readonly string[] operators = { " AND ", " OR ", "NOT " };
        
        private List<string> CategoriesCheckBoxes { get; set; }

        public ActionResult Index()
        {
            var books = db.Books.OrderBy(b => b.Title);
            var categories = db.Categories.OrderBy(c => c.Name);
            var tags = db.Tags.OrderBy(t => t.Name);
            

            ViewBag.BooksList = books.ToList();
            ViewBag.CategoriesList = categories.ToList();
            ViewBag.TagsList = tags.ToList();
            NewBooks();

            return View();
        }

        public void NewBooks()
        {
            int numerOfDays = 5;

            var books = db.Books.OrderBy(b => b.Title);

            DateTime limitDate = DateTime.Now.AddDays(-numerOfDays);

            ViewBag.NewBookList = books.Where(b => b.AdditionDate.CompareTo(limitDate) > 0).ToList();
        }

        [HttpPost]
        public ActionResult SearchBooks(string searchString)
        {
            var books = db.Books.OrderBy(b => b.Title);
            var categories = db.Categories.OrderBy(c => c.Name);
            var tags = db.Tags.OrderBy(t => t.Name);

            if (!String.IsNullOrEmpty(searchString))
            {
                // Check if search string contains operators 
                if (operators.Any(searchString.Contains))
                {
                    List<Book> searchedBooks = books.ToList();
                    string[] splittedSearch = searchString.Split(new char[] { ' ' }, 3, StringSplitOptions.RemoveEmptyEntries);
                    for (int i = 0; i < splittedSearch.Length; i++)
                    {
                        string s = splittedSearch[i];
                        switch (s)
                        {
                            case "AND":
                                if (i != 0 && splittedSearch.Length == 3)
                                {
                                    searchedBooks = searchedBooks.Where(b => b.ISBN.Contains(splittedSearch[i - 1]) || b.Title.Contains(splittedSearch[i - 1])
                                        || b.Authors.Any(a => a.Name.Contains(splittedSearch[i - 1]) || a.Surname.Contains(splittedSearch[i - 1]))).ToList();
                                    searchedBooks = searchedBooks.Where(b => b.ISBN.Contains(splittedSearch[i + 1]) || b.Title.Contains(splittedSearch[i + 1])
                                        || b.Authors.Any(a => a.Name.Contains(splittedSearch[i + 1]) || a.Surname.Contains(splittedSearch[i + 1]))).ToList();
                                }
                                break;

                            case "OR":
                                if (i != 0 && splittedSearch.Length == 3)
                                {
                                    searchedBooks = searchedBooks.Where(b => (b.ISBN.Contains(splittedSearch[i - 1]) || b.ISBN.Contains(splittedSearch[i + 1]))
                                        || (b.Title.Contains(splittedSearch[i - 1]) || b.Title.Contains(splittedSearch[i + 1]))
                                        || b.Authors.Any(a => 
                                            (a.Name.Contains(splittedSearch[i - 1]) || a.Name.Contains(splittedSearch[i + 1])) 
                                            || (a.Surname.Contains(splittedSearch[i - 1]) || a.Surname.Contains(splittedSearch[i + 1])))).ToList();
                                }
                                break;

                            case "NOT":
                                if (i == 0 && splittedSearch.Length == 2)
                                {
                                    searchedBooks = searchedBooks.Where(b => !b.ISBN.Contains(splittedSearch[i + 1]) && !b.Title.Contains(splittedSearch[i + 1])
                                        && b.Authors.Any(a => !a.Name.Contains(splittedSearch[i + 1]) && !a.Surname.Contains(splittedSearch[i + 1]))).ToList();
                                }
                                break;

                            default:
                                break;
                        }
                    }

                    ViewBag.BooksList = searchedBooks;
                }
                else
                {
                    ViewBag.BooksList = books.Where(b => b.ISBN.Contains(searchString) || b.Title.Contains(searchString)
                        || b.Authors.Any(a => a.Name.Contains(searchString) || a.Surname.Contains(searchString)));
                }
            }
            else
            {
                ViewBag.BooksList = books.ToList();
            }

            ViewBag.CategoriesList = categories.ToList();
            ViewBag.TagsList = tags.ToList();

            return View("~/Views/Home/Index.cshtml");
        }

        [HttpPost]
        public ActionResult FilterBooks(FormCollection formCollection)
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
                // Selected both categories and tags
                if (selectedCategories.Count > 0 && selectedTags.Count > 0)
                {
                    if (selectedCategories.Contains(book.Category.Name) && selectedTags.Any(t => book.Tags.Any(bt => bt.Name == t)))
                    {
                        selectedBooks.Add(book);
                    }
                }
                else
                {
                    // Selected only categories
                    if (selectedCategories.Count > 0)
                    {
                        if (selectedCategories.Contains(book.Category.Name))
                        {
                            selectedBooks.Add(book);
                        }
                    }
                    else 
                    {
                        // Selected only tags
                        if (selectedTags.Count > 0)
                        {
                            if (selectedTags.Any(t => book.Tags.Any(bt => bt.Name == t)))
                            {
                                selectedBooks.Add(book);
                            }
                        }
                        else
                        {
                            // Selection is empty
                            selectedBooks = books.ToList();
                            break;
                        }
                    }
                }
            }

            ViewBag.BooksList = selectedBooks;
            ViewBag.CategoriesList = categories.ToList();
            ViewBag.TagsList = tags.ToList();

            return View("~/Views/Home/Index.cshtml");
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

        public ActionResult BookDetails(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Book book = db.Books.Find(id);
            if (book == null)
            {
                return HttpNotFound();
            }
            return View(book);
        }
    }
}