using LibrARRRy.DAL;
using LibrARRRy.Models;
using LibrARRRy.ViewModel;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace LibrARRRy.Controllers
{
    public class ManagePanelController : Controller
    {
        private readonly LibrARRRyContext db = new LibrARRRyContext();

        private ApplicationUserManager _userManager;

        public ApplicationUserManager UserManager
        {
            get
            {
                return _userManager ?? HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
            }
            private set
            {
                _userManager = value;
            }
        }

        public ManagePanelController()
        {
        }

        public ManagePanelController(ApplicationUserManager userManager)
        {
            UserManager = userManager;
        }

        // GET: ManagePanel
        [Authorize(Roles = "admin,worker")]
        public ActionResult All()
        {
            dynamic dynamicObject = new ExpandoObject();
            dynamicObject.Books = db.Books.ToList();
            dynamicObject.Authors = db.Authors.ToList();
            dynamicObject.Categories = db.Categories.ToList();
            dynamicObject.Tags = db.Tags.ToList();
            dynamicObject.Storages = db.Storages.ToList();

            var confirmReaders = new List<ConfirmReadersViewModel>();
            var userStore = new UserStore<ApplicationUser>(db);

            foreach (var user in userStore.Users)
            {
                var r = new ConfirmReadersViewModel
                {
                    Id = user.Id,
                    Email = user.Email,
                    IsConfirmed = user.EmailConfirmed
                };
                confirmReaders.Add(r);
            }
            foreach (var user in confirmReaders)
            {
                user.Role = UserManager.GetRoles(userStore.Users.First(s => s.Email == user.Email).Id);
            }

            confirmReaders = confirmReaders.Where(s => s.Role.Contains("reader")).ToList();

            dynamicObject.Readers = confirmReaders;
            return View(dynamicObject);
        }

        public async Task<ActionResult> ConfirmAsync(string id)
        {
            if (id == "")
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var user = await UserManager.FindByIdAsync(id);

            user.EmailConfirmed = true;
            UserManager.Update(user);
            return RedirectToAction("All");
        }

        public async Task<ActionResult> Delete(string id)
        {
            if (id == "")
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var user = await UserManager.FindByIdAsync(id);
            var logins = user.Logins.ToList();
            var rolesForUser = await UserManager.GetRolesAsync(id);

            using(var transaction = db.Database.BeginTransaction())
            {
                foreach(var login in logins)
                {
                    await UserManager.RemoveLoginAsync(login.UserId, new UserLoginInfo(login.LoginProvider, login.ProviderKey));
                }

                if(rolesForUser.Count() > 0)
                {
                    foreach(var item in rolesForUser.ToList())
                    {
                        var result = await UserManager.RemoveFromRoleAsync(user.Id, item);
                    }
                }

                await UserManager.DeleteAsync(user);

                TempData["Message"] = "User Deleted Successfully. ";
                TempData["MessageValue"] = "1";

                transaction.Commit();
            }

            return RedirectToAction("All");

        }

    }
}