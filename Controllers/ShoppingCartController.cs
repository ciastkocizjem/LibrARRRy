using LibrARRRy.DAL;
using LibrARRRy.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace LibrARRRy.Controllers
{
    public class ShoppingCartController : Controller
    {
        private readonly LibrARRRyContext db = new LibrARRRyContext();

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
    }
}