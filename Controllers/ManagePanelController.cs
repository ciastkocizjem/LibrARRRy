using LibrARRRy.DAL;
using LibrARRRy.Models;
using LibrARRRy.ViewModel;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Dynamic;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace LibrARRRy.Controllers
{
    [Authorize]
    public class ManagePanelController : Controller
    {
        private readonly LibrARRRyContext db = new LibrARRRyContext();

        private readonly List<string> holdedBooks = new List<string>() { "For two weeks", "For three weeks", "For month" };

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
            //ViewBag.Limit = Properties.Settings.Default.BooksLimit;
            //ViewBag.Limit = Environment.GetEnvironmentVariable("BooksLimit");
            ViewBag.Limit = db.AdminSettings.First().BorrowedBooksLimit;
            ViewBag.HoldedBooks = holdedBooks;
            ViewBag.SelectedHolded = db.AdminSettings.First().DetentionLimit;

            dynamic dynamicObject = new ExpandoObject();
            dynamicObject.Books = db.Books.ToList();
            dynamicObject.Authors = db.Authors.ToList();
            dynamicObject.Categories = db.Categories.ToList();
            dynamicObject.Tags = db.Tags.ToList();
            dynamicObject.Storages = db.Storages.ToList();
            dynamicObject.Loans = db.Loans.ToList();

            var confirmReaders = new List<ConfirmReadersViewModel>();
            var userStore = new UserStore<ApplicationUser>(db);

            foreach (var user in userStore.Users)
            {
                var r = new ConfirmReadersViewModel
                {
                    Id = user.Id,
                    Email = user.Email,
                    EmailConfirmed = user.EmailConfirmed, 
                    CashPenalty = user.CashPenalty
                };
                confirmReaders.Add(r);
            }
            foreach (var user in confirmReaders)
            {
                user.Role = UserManager.GetRoles(userStore.Users.First(s => s.Email == user.Email).Id);
            }

            confirmReaders = confirmReaders.Where(s => s.Role.Contains("reader")).ToList();

            ViewBag.PreviousUrl = "/"; 

            if (System.Web.HttpContext.Current.Request.UrlReferrer != null) { 
                string prev = (System.Web.HttpContext.Current.Request.UrlReferrer).LocalPath;
                ViewBag.PreviousUrl = prev;
            }

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

            if (user.EmailConfirmed == false) user.EmailConfirmed = true;

            UserManager.Update(user);
            return RedirectToAction("All");
        }

        public async Task<ActionResult> ChangePenaltyAsync(string id)
        {
            if (id == "")
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var user = await UserManager.FindByIdAsync(id);

            if (user.CashPenalty == true)
                user.CashPenalty = false;

            UserManager.Update(user);
            return RedirectToAction("All");
        }

        public void ChangeBorrowBooksLimit(int number, string selectedValue)
        {
            //Properties.Settings.Default["BooksLimit"] = number;
            //Properties.Settings.Default.Save();

            //Environment.SetEnvironmentVariable("BooksLimit", number.ToString());
            //using (StreamWriter writer = new StreamWriter(Server.MapPath(@"~/Content/BooksLimit.txt"), false))
            //{
            //    writer.WriteLine(number);
            //}

            int selectedInt = holdedBooks.IndexOf(selectedValue);

            AdminSettings settings = db.AdminSettings.First();
            settings.BorrowedBooksLimit = number;
            settings.DetentionLimit = selectedInt;
            db.SaveChanges();
        }

        public async Task<ActionResult> ConfirmWorkerAsync(string id)
        {
            if (id == "")
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var user = await UserManager.FindByIdAsync(id);

            if(user.EmailConfirmed == false) user.EmailConfirmed = true;

            UserManager.Update(user);
            return RedirectToAction("Workers");
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
        [Authorize(Roles = "admin")]
        public ActionResult Workers()
        {
            dynamic dynamicObject = new ExpandoObject();

            var confirmReaders = new List<ConfirmReadersViewModel>();
            var workers = new List<ConfirmReadersViewModel>();
            var toWorkers = new List<ConfirmReadersViewModel>();
            var userStore = new UserStore<ApplicationUser>(db);

            foreach (var user in userStore.Users)
            {
                var r = new ConfirmReadersViewModel
                {
                    Id = user.Id,
                    Email = user.Email,
                    EmailConfirmed = user.EmailConfirmed
                };
                confirmReaders.Add(r);
            }
            foreach (var user in confirmReaders)
            {
                user.Role = UserManager.GetRoles(userStore.Users.First(s => s.Email == user.Email).Id);
            }

            workers = confirmReaders.Where(s => s.Role.Contains("worker")).ToList();
            toWorkers = confirmReaders.Where(s => s.Role.Contains("reader")).ToList();

            dynamicObject.ToWorkers = toWorkers;
            dynamicObject.Users = workers;
            return View(dynamicObject);
        }

        public async Task<ActionResult> ChangeRole(string id)
        {
            var roleStore = new RoleStore<IdentityRole>(db);
            var roleMngr = new RoleManager<IdentityRole>(roleStore);

            if (id == "")
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var user = await UserManager.FindByIdAsync(id);
            ConfirmReadersViewModel model = new ConfirmReadersViewModel
            {
                Id = user.Id,
                Email = user.Email,
                EmailConfirmed = user.EmailConfirmed,
                Role = UserManager.GetRoles(user.Id)
            };

            ViewBag.Roles = new SelectList(roleMngr.Roles.ToList(), "Name", "Name");

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ChangeRole(ConfirmReadersViewModel user)
        {
            if (ModelState.IsValid)
            {
                var roles = await UserManager.GetRolesAsync(user.Id);
                await UserManager.RemoveFromRolesAsync(user.Id, roles.ToArray());

                IdentityManager im = new IdentityManager();
                im.AddUserToRole(user.Id, user.Role.First().ToString());

                return RedirectToAction("Workers");
            }
            return View(user);
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