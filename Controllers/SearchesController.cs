using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using LibrARRRy.DAL;
using LibrARRRy.Models;

namespace LibrARRRy.Controllers
{
    public class SearchesController : Controller
    {
        private LibrARRRyContext db = new LibrARRRyContext();

        // GET: Searches
        public ActionResult Index()
        {
            var searches = db.Searches.Include(s => s.ApplicationUser);
            return View(searches.ToList());
        }

        // GET: Searches/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Search search = db.Searches.Find(id);
            if (search == null)
            {
                return HttpNotFound();
            }
            return View(search);
        }

        // GET: Searches/Create
        public ActionResult Create()
        {
            ViewBag.ApplicationUserId = new SelectList(db.Users, "Id", "Email");
            return View();
        }

        // POST: Searches/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "SearchId,Content,ApplicationUserId")] Search search)
        {
            if (ModelState.IsValid)
            {
                db.Searches.Add(search);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.ApplicationUserId = new SelectList(db.Users, "Id", "Email", search.ApplicationUserId);
            return View(search);
        }

        public ActionResult CreateFromHome(ApplicationUser user, string searched)
        {
            if (ModelState.IsValid)
            {
                Search search = new Search() { ApplicationUserId = user.Id, Content = searched };
                search.Books = GetSearchedBooks(searched);

                //foreach (Book b in books)
                //{
                //    search.Books.Add(b);
                //}

                db.Searches.Add(search);
                db.SaveChanges();
            }
            return RedirectToAction("Index", "Home");
        }

        public List<Book> GetSearchedBooks(string searchString)
        {
            string[] operators = { " AND ", " OR ", "NOT " };
            var books = db.Books.OrderBy(b => b.Title);
            List<Book> finalBooks = new List<Book>();

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

                    //ViewBag.BooksList = searchedBooks;
                    finalBooks = searchedBooks;
                }
                else
                {
                    //ViewBag.BooksList = books.Where(b => b.ISBN.Contains(searchString) || b.Title.Contains(searchString)
                    //    || b.Authors.Any(a => a.Name.Contains(searchString) || a.Surname.Contains(searchString)));
                    finalBooks = books.Where(b => b.ISBN.Contains(searchString) || b.Title.Contains(searchString)
                         || b.Authors.Any(a => a.Name.Contains(searchString) || a.Surname.Contains(searchString))).ToList();
                }
            }
            else
            {
                //ViewBag.BooksList = books.ToList();
                finalBooks = books.ToList();
            }

            return finalBooks;
        }

        // GET: Searches/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Search search = db.Searches.Find(id);
            if (search == null)
            {
                return HttpNotFound();
            }
            ViewBag.ApplicationUserId = new SelectList(db.Users, "Id", "Email", search.ApplicationUserId);
            return View(search);
        }

        // POST: Searches/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "SearchId,Content,ApplicationUserId")] Search search)
        {
            if (ModelState.IsValid)
            {
                db.Entry(search).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.ApplicationUserId = new SelectList(db.Users, "Id", "Email", search.ApplicationUserId);
            return View(search);
        }

        // GET: Searches/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Search search = db.Searches.Find(id);
            if (search == null)
            {
                return HttpNotFound();
            }
            return View(search);
        }

        // POST: Searches/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Search search = db.Searches.Find(id);
            db.Searches.Remove(search);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
