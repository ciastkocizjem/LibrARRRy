using LibrARRRy.DAL;
using LibrARRRy.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data.Entity;
using System.Threading.Tasks;

namespace LibrARRRy.Controllers
{
    [Authorize(Roles = ("reader"))]
    public class ShoppingCartController : Controller
    {
        private readonly LibrARRRyContext db = new LibrARRRyContext();
        private readonly int loanDuration = 14; // In days

        // GET: ShoppingCart
        public ActionResult Add(Book book)
        {
            if (Session["cart"] == null)
            {
                List<Book> books = new List<Book>();
                books.Add(book);
                Session["cart"] = books;
                Session["count"] = 1;
            } 
            else
            {
                List<Book> books = (List<Book>)Session["cart"];
                books.Add(book);
                Session["cart"] = books;
                Session["count"] = Convert.ToInt32(Session["count"]) + 1;
            }
            return RedirectToAction("Index", "Home");
        }

        public ActionResult Order()
        {
            if (Session["cart"] != null) return View((List<Book>)Session["cart"]);
            return RedirectToAction("Index", "Home");
        }

        public ActionResult Remove(Book book)
        {
            List<Book> books = (List<Book>)Session["cart"];
            books.RemoveAll(b => b.BookId == book.BookId);
            Session["cart"] = books;
            Session["count"] = Convert.ToInt32(Session["count"]) - 1;

            return RedirectToAction("Order", "ShoppingCart");
        }

        [Authorize] // To force being logged in
        public async Task<ActionResult> Index()
        {
            if (Session["cart"] != null)
            {
                // Get books in cart and storage
                List<Book> books = Session["cart"] as List<Book>;
                var loansController = DependencyResolver.Current.GetService<LoansController>();
                List<Book> notLoanedBooks = new List<Book>();   // For unavaliable books
                var storage = db.Storages.ToList();

                foreach (Book b in books)
                {
                    // Find book in storage
                    var bookInStorage = storage.Where(sb => sb.BookId == b.BookId).FirstOrDefault();
                    if (bookInStorage != null)
                    {
                        // Decrement amount of books in storage if possible and delete book from cart
                        if (bookInStorage.CurrentAmount > 0)
                        {
                            bookInStorage.CurrentAmount--;
                            db.SaveChanges();

                            // Get current user and add books to his loaned books
                            if (User.Identity.IsAuthenticated)
                            {
                                string userName = User.Identity.Name;
                                IdentityManager im = new IdentityManager();
                                ApplicationUser user = im.GetUserByName(userName);

                                await loansController.CreateFromCart(b, user);
                            }
                        }
                        else
                        {
                            // TODO: Add message in UI that the book is not in storage
                            notLoanedBooks.Add(b);
                        }
                    }
                }

                //// Update session (not clear in case some books arent avaliable rn)
                Session["cart"] = notLoanedBooks;
                Session["count"] = notLoanedBooks.Count;
            }
            return RedirectToAction("Order", "ShoppingCart");
        }
    }
}