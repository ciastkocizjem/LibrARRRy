using LibrARRRy.DAL;
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