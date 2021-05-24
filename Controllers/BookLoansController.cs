using LibrARRRy.DAL;
using LibrARRRy.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace LibrARRRy.Controllers
{
    public class BookLoansController : Controller
    {
        private readonly LibrARRRyContext db = new LibrARRRyContext();

        // GET: BookLoans
        public ActionResult Loans()
        {
            List<Book> books = db.Books.ToList();

            // Get user
            string userName = User.Identity.Name;
            IdentityManager im = new IdentityManager();
            ApplicationUser user = im.GetUserByName(userName);
            List<Loan> currentlyLoaned = user.Loaned.OrderBy(l => l.ReturnedDate).ToList();
            books = books.Where(b => currentlyLoaned.Any(l => l.BookId == b.BookId)).ToList();

            return View(currentlyLoaned);
        }

        [Authorize]
        public ActionResult Return(Book book)
        {
            IdentityManager im = new IdentityManager();
            var storage = db.Storages.ToList();

            // Increment current amount in storage
            var bookInStorage = storage.Where(sb => sb.BookId == book.BookId).FirstOrDefault();
            if (bookInStorage != null)
            {
                bookInStorage.CurrentAmount++;
                db.SaveChanges();

                // Add returned date to loan
                var loansController = DependencyResolver.Current.GetService<LoansController>();
                loansController.EditFromBookLoans(book, im.GetUserByName(User.Identity.Name));
            }

            return RedirectToAction("Loans", "BookLoans");
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