using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using LibrARRRy.DAL;
using LibrARRRy.Models;

namespace LibrARRRy.Controllers
{
    public class StoragesController : Controller
    {
        private LibrARRRyContext db = new LibrARRRyContext();

        // GET: Storages
        //public ActionResult Index()
        //{
        //    var storages = db.Storages.Include(s => s.Book);
        //    return View(storages.ToList());
        //}

        // GET: Storages/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Storage storage = db.Storages.Find(id);
            if (storage == null)
            {
                return HttpNotFound();
            }
            return View(storage);
        }

        // GET: Storages/Create
        public ActionResult Create()
        {
            ViewBag.BookId = new SelectList(db.Books, "BookId", "Title");
            return View();
        }

        // POST: Storages/Create
        // Aby zapewnić ochronę przed atakami polegającymi na przesyłaniu dodatkowych danych, włącz określone właściwości, z którymi chcesz utworzyć powiązania.
        // Aby uzyskać więcej szczegółów, zobacz https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "BookStorageId,BookId,Amount,CurrentAmount")] Storage storage)
        {
            if (ModelState.IsValid)
            {
                storage.CurrentAmount = storage.Amount;
                db.Storages.Add(storage);
                db.SaveChanges();
                return RedirectToAction("All", "ManagePanel");
            }

            ViewBag.BookId = new SelectList(db.Books, "BookId", "Title", storage.BookId);
            return View(storage);
        }

        // GET: Storages/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Storage storage = db.Storages.Find(id);
            if (storage == null)
            {
                return HttpNotFound();
            }
            ViewBag.BookId = new SelectList(db.Books, "BookId", "Title", storage.BookId);
            return View(storage);
        }

        // POST: Storages/Edit/5
        // Aby zapewnić ochronę przed atakami polegającymi na przesyłaniu dodatkowych danych, włącz określone właściwości, z którymi chcesz utworzyć powiązania.
        // Aby uzyskać więcej szczegółów, zobacz https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "BookStorageId,BookId,Amount,CurrentAmount")] Storage storage)
        {
            if (ModelState.IsValid)
            {
                db.Entry(storage).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("All", "ManagePanel");
            }
            ViewBag.BookId = new SelectList(db.Books, "BookId", "Title", storage.BookId);
            return View(storage);
        }

        // GET: Storages/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Storage storage = db.Storages.Find(id);
            if (storage == null)
            {
                return HttpNotFound();
            }
            return View(storage);
        }

        // POST: Storages/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Storage storage = db.Storages.Find(id);
            db.Storages.Remove(storage);
            db.SaveChanges();
            return RedirectToAction("All", "ManagePanel");
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
