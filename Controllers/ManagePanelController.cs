using LibrARRRy.DAL;
using LibrARRRy.Models;
using LibrARRRy.ViewModel;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
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
            var userManager = new UserManager<ApplicationUser>(userStore);

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
                user.Role = userManager.GetRoles(userStore.Users.First(s => s.Email == user.Email).Id);
            }

            confirmReaders = confirmReaders.Where(s => s.Role.Contains("reader")).ToList();

            dynamicObject.Readers = confirmReaders;
            return View(dynamicObject);
        }

        public async Task<ActionResult> ConfirmAsync(string id)
        {
            var userStore = new UserStore<ApplicationUser>(db);
            var userManager = new UserManager<ApplicationUser>(userStore);

            if (id == "")
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var user = await userManager.FindByIdAsync(id);

            user.EmailConfirmed = true;
            userManager.Update(user);
            return RedirectToAction("All");
        }

        public async Task<ActionResult> Delete(string id)
        {
            var userStore = new UserStore<ApplicationUser>(db);
            var userManager = new UserManager<ApplicationUser>(userStore);

            if (id == "")
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var user = await userManager.FindByIdAsync(id);
            var logins = user.Logins.ToList();
            var rolesForUser = await userManager.GetRolesAsync(id);

            using(var transaction = db.Database.BeginTransaction())
            {
                foreach(var login in logins)
                {
                    await userManager.RemoveLoginAsync(login.UserId, new UserLoginInfo(login.LoginProvider, login.ProviderKey));
                }

                if(rolesForUser.Count() > 0)
                {
                    foreach(var item in rolesForUser.ToList())
                    {
                        var result = await userManager.RemoveFromRoleAsync(user.Id, item);
                    }
                }

                await userManager.DeleteAsync(user);

                TempData["Message"] = "User Deleted Successfully. ";
                TempData["MessageValue"] = "1";

                transaction.Commit();
            }

            return RedirectToAction("All");

        }

    }
}