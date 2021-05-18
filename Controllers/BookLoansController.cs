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
            return View(books);
        }

        public ActionResult Return(Book book)
        {
            return RedirectToAction("Loans", "BookLoans");
        }
    }
}