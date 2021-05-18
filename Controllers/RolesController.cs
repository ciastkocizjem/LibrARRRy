using LibrARRRy.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace LibrARRRy.Controllers
{
    public class RolesController : Controller
    {
        // GET: Roles
        public ActionResult Index()
        {
            IdentityManager im = new IdentityManager();
            im.CreateRole("admin");
            im.CreateRole("reader");
            im.CreateRole("worker");
            im.AddUserToRoleByUsername("admin@admin.com", "admin");
            im.AddUserToRoleByUsername("user@user.com", "reader");
            return View();
        }
    }
}