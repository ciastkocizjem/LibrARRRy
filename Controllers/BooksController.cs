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
using LibrARRRy.ViewModel;

namespace LibrARRRy.Controllers
{
    [Authorize(Roles = "admin")]
    public class BooksController : Controller
    {
        private LibrARRRyContext db = new LibrARRRyContext();

        List<Book> books;
        public BooksController()
        {
            books = db.Books.Include(b => b.Category).ToList();
        }

        // Used when view is reloaded when no authors or tags was chosen
        private void CreateViewBags()
        {
            var allAuthorsList = db.Authors.ToList();
            ViewBag.AllAuthors = allAuthorsList.Select(o => new SelectListItem
            {
                Text = o.Name + " " + o.Surname,
                Value = o.AuthorId.ToString()
            });

            var allTagsList = db.Tags.ToList();
            ViewBag.AllTags = allTagsList.Select(o => new SelectListItem
            {
                Text = o.Name,
                Value = o.TagId.ToString()
            });

            ViewBag.CategoryId = new SelectList(db.Categories, "CategoryId", "Name");
        }

        // GET: Books/Details/5
        public ActionResult Details(int? id)
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

        // GET: Books/Create
        public ActionResult Create()
        {
            var bookViewModel = new BooksViewModel();

            var allAuthorsList = db.Authors.ToList();
            ViewBag.AllAuthors = allAuthorsList.Select(o => new SelectListItem
            {
                Text = o.Name + " " + o.Surname,
                Value = o.AuthorId.ToString()
            });

            var allTagsList = db.Tags.ToList();
            ViewBag.AllTags = allTagsList.Select(o => new SelectListItem
            {
                Text = o.Name,
                Value = o.TagId.ToString()
            });
            ViewBag.CategoryId = new SelectList(db.Categories, "CategoryId", "Name");
            return View();
        }

        // POST: Books/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(BooksViewModel booksViewModel)
        {
            if (booksViewModel.SelectedAuthors == null)
            {
                ModelState.AddModelError("", "Proszę wybrać autora");
                CreateViewBags();
            }

            if (booksViewModel.SelectedTags == null)
            {
                ModelState.AddModelError("", "Proszę wybrać przynajmniej jeden tag");
                CreateViewBags();
            }

            if (ModelState.IsValid)
            {
                var bookToAdd = db.Books.Include(i => i.Authors).Include(i => i.Tags).FirstOrDefault();
                if (bookToAdd == null)
                {
                    bookToAdd = new Book();
                }

                if (TryUpdateModel(bookToAdd, "Book", new string[] { "Title", "ISBN", "CategoryId", "AdditionDate", "Description" }))
                {
                    var updatedAuthors = new HashSet<int>(booksViewModel.SelectedAuthors);
                    foreach (Author author in db.Authors)
                    {
                        if (!updatedAuthors.Contains(author.AuthorId))
                            bookToAdd.Authors.Remove(author);
                        else
                            bookToAdd.Authors.Add(author);
                    }

                    var updateTags = new HashSet<int>(booksViewModel.SelectedTags);
                    foreach (Tag tag in db.Tags)
                    {
                        if (!updateTags.Contains(tag.TagId))
                            bookToAdd.Tags.Remove(tag);
                        else
                            bookToAdd.Tags.Add(tag);
                    }
                }

                db.Books.Add(bookToAdd);
                db.SaveChanges();
                return RedirectToAction("All", "ManagePanel");

            }

            ViewBag.CategoryId = new SelectList(db.Categories, "CategoryId", "Name");
            return View(booksViewModel);
        }

        // GET: Books/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var bookViewModel = new BooksViewModel
            {
                Book = db.Books.Include(i => i.Authors).Include(i => i.Tags).First(i => i.BookId == id)
            };

            if (bookViewModel == null)
            {
                return HttpNotFound();
            }

            var allAuthorsList = db.Authors.ToList();
            bookViewModel.AllAuthors = allAuthorsList.Select(o => new SelectListItem
            {
                Text = o.Name + " " + o.Surname,
                Value = o.AuthorId.ToString()
            });

            var allTagsList = db.Tags.ToList();
            bookViewModel.AllTags = allTagsList.Select(o => new SelectListItem
            {
                Text = o.Name,
                Value = o.TagId.ToString()
            });

            foreach(Author a in bookViewModel.Book.Authors)
            {
                bookViewModel.SelectedAuthors.Add(a.AuthorId);
            }
            foreach(Tag t in bookViewModel.Book.Tags)
            {
                bookViewModel.SelectedTags.Add(t.TagId);
            }

            ViewBag.CategoryId = new SelectList(db.Categories, "CategoryId", "Name", bookViewModel.Book.CategoryId);
            return View(bookViewModel);
        }

        // POST: Books/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(BooksViewModel booksViewModel)
        {
            if (booksViewModel.SelectedAuthors == null)
            {
                ModelState.AddModelError("", "Proszę wybrać autora");
                CreateViewBags();
            }

            if (booksViewModel.SelectedTags == null)
            {
                ModelState.AddModelError("", "Proszę wybrać przynajmniej jeden tag");
                CreateViewBags();
            }

            if (booksViewModel == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            if (ModelState.IsValid)
            {
                var bookToUpdate = db.Books.Include(i => i.Authors).Include(i => i.Tags).First(i => i.BookId == booksViewModel.Book.BookId);

                if (TryUpdateModel(bookToUpdate, "Book", new string[] { "Title", "ISBN", "CategoryId", "AdditionDate", "Description" }))
                {
                    var newAuthor = db.Authors.Where(m => booksViewModel.SelectedAuthors.Contains(m.AuthorId)).ToList();
                    var updatedAuthors = new HashSet<int>(booksViewModel.SelectedAuthors);
                    foreach (Author author in db.Authors)
                    {
                        if (!updatedAuthors.Contains(author.AuthorId))
                            bookToUpdate.Authors.Remove(author);
                        else
                            bookToUpdate.Authors.Add(author);
                    }

                    var newTag = db.Tags.Where(m => booksViewModel.SelectedTags.Contains(m.TagId)).ToList();
                    var updateTags = new HashSet<int>(booksViewModel.SelectedTags);
                    foreach (Tag tag in db.Tags)
                    {
                        if (!updateTags.Contains(tag.TagId))
                            bookToUpdate.Tags.Remove(tag);
                        else
                            bookToUpdate.Tags.Add(tag);
                    }

                    var category = db.Categories.Where(c => c.CategoryId == booksViewModel.CategoryId).FirstOrDefault();
                    bookToUpdate.CategoryId = booksViewModel.CategoryId;
                    bookToUpdate.Category = category;
                }
                db.Entry(bookToUpdate).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("All", "ManagePanel");
            }
            ViewBag.CategoryId = new SelectList(db.Categories, "CategoryId", "Name", booksViewModel.Book.CategoryId);
            return View(booksViewModel);
        }

        // GET: Books/Delete/5
        public ActionResult Delete(int? id)
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

        // POST: Books/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Book book = db.Books.Find(id);
            db.Books.Remove(book);
            db.SaveChanges();
            return RedirectToAction("All", "ManagePanel");
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
