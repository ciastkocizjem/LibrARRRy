using LibrARRRy.DAL;
using LibrARRRy.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace LibrARRRy.Views
{
    public class UsersController : Controller
    {
        private LibrARRRyContext db = new LibrARRRyContext();
        // GET: Users
        public ActionResult Index()
        {
            IdentityManager im = new IdentityManager();

            ViewBag.Users = db.Users.ToList();
            ViewBag.AllRoles = im.GetRoles();

            return View();
        }
    }
}