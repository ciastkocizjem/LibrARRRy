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
    [Authorize(Roles = "admin,worker")]
    public class LoansController : Controller
    {
        private LibrARRRyContext db = new LibrARRRyContext();
        private readonly int loanDuration = 14; // In days

        // GET: Loans
        public ActionResult Index()
        {
            var loans = db.Loans.Include(l => l.Book).Include(l => l.Reader);
            return View(loans.ToList());
        }

        // GET: Loans/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Loan loan = db.Loans.Find(id);
            if (loan == null)
            {
                return HttpNotFound();
            }
            return View(loan);
        }

        // GET: Loans/Create
        public ActionResult Create()
        {
            ViewBag.BookId = new SelectList(db.Books, "BookId", "Title");
            ViewBag.ReaderId = new SelectList(db.Users, "Id", "Email");
            return View();
        }

        // POST: Loans/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "LoanId,BookId,ReaderId,LoanedDate,LoanExpireDate,ReturnedDate")] Loan loan)
        {
            if (ModelState.IsValid)
            {
                db.Loans.Add(loan);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.BookId = new SelectList(db.Books, "BookId", "Title", loan.BookId);
            ViewBag.ReaderId = new SelectList(db.Users, "Id", "Email", loan.ReaderId);
            return View(loan);
        }

        // POST: Loans/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        //[HttpPost]
        public void CreateFromCart(Book book, ApplicationUser user)
        {
            if (ModelState.IsValid)
            {
                IdentityManager im = new IdentityManager();
                db.Loans.Add(new Loan() { BookId = book.BookId, LoanedDate = DateTime.Now, LoanExpireDate = DateTime.Now.AddDays(loanDuration), ReaderId = user.Id});
                db.SaveChanges();
            }
        }
        // GET: Loans/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Loan loan = db.Loans.Find(id);
            if (loan == null)
            {
                return HttpNotFound();
            }
            ViewBag.BookId = new SelectList(db.Books, "BookId", "Title", loan.BookId);
            ViewBag.ReaderId = new SelectList(db.Users, "Id", "Email", loan.ReaderId);
            return View(loan);
        }

        // POST: Loans/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "LoanId,BookId,ReaderId,LoanedDate,LoanExpireDate,ReturnedDate")] Loan loan)
        {
            if (ModelState.IsValid)
            {
                db.Entry(loan).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.BookId = new SelectList(db.Books, "BookId", "Title", loan.BookId);
            ViewBag.ReaderId = new SelectList(db.Users, "Id", "Email", loan.ReaderId);
            return View(loan);
        }

        public void EditFromBookLoans(Book book, ApplicationUser user)
        {
            if (ModelState.IsValid)
            {
                Loan toEdit = db.Loans.Where(l => l.BookId == book.BookId && l.ReaderId == user.Id && l.ReturnedDate == null).FirstOrDefault();
                toEdit.ReturnedDate = DateTime.Now;

                db.Entry(toEdit).State = EntityState.Modified;
                db.SaveChanges();
            }
        }


        // GET: Loans/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Loan loan = db.Loans.Find(id);
            if (loan == null)
            {
                return HttpNotFound();
            }
            return View(loan);
        }

        // POST: Loans/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Loan loan = db.Loans.Find(id);
            db.Loans.Remove(loan);
            db.SaveChanges();
            return RedirectToAction("Index");
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
