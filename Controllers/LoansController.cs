﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using LibrARRRy.DAL;
using LibrARRRy.Models;
using Quartz;
using Quartz.Impl;

namespace LibrARRRy.Controllers
{
    public class LoansController : Controller
    {
        private LibrARRRyContext db = new LibrARRRyContext();
        private readonly int collectionAfter = 3, loanDuration = 14; // In days

        // To schedule sending emails
        StdSchedulerFactory factory = new StdSchedulerFactory();

        public async Task ScheduleEmail(ApplicationUser user, bool collectionMessage, int loanId) // collection = false -> returning book message
        {
            string emailBody = collectionMessage ? "Hello, we just want to remind you to collect your book tomorrow!" : "Hello, we just want to remind you to return borrowed book!";

            IScheduler scheduler = await factory.GetScheduler();
            await scheduler.Start();

            IJobDetail job = JobBuilder.Create<SendMailJob>()
                .UsingJobData("userId", user.Id)
                .UsingJobData("body", emailBody)
                .UsingJobData("loanId", loanId.ToString())
                .UsingJobData("type", collectionMessage ? "c" : "r")
                .Build();

            //ITrigger trigger = TriggerBuilder.Create().StartAt(DateTimeOffset.Now.AddDays(collectionMessage ? collectionAfter - 1 : loanDuration + 1)).Build();
            ITrigger trigger = TriggerBuilder.Create().StartAt(DateTimeOffset.Now.AddSeconds(collectionMessage ? 20 : 40)).Build();

            await scheduler.ScheduleJob(job, trigger);
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
        public ActionResult Create([Bind(Include = "LoanId,BookId,ReaderId,LoanedDate,CollectionDate,LoanExpireDate,ReturnedDate")] Loan loan)
        {
            if (ModelState.IsValid)
            {
                db.Loans.Add(loan);
                db.SaveChanges();
                return RedirectToAction("All", "ManagePanel");
            }

            ViewBag.BookId = new SelectList(db.Books, "BookId", "Title", loan.BookId);
            ViewBag.ReaderId = new SelectList(db.Users, "Id", "Email", loan.ReaderId);
            return View(loan);
        }

        public async Task CreateFromCart(Book book, ApplicationUser user)
        {
            if (ModelState.IsValid)
            {
                IdentityManager im = new IdentityManager();
                db.Loans.Add(new Loan()
                {
                    BookId = book.BookId,
                    LoanedDate = DateTime.Now,
                    LoanExpireDate = DateTime.Now.AddDays(collectionAfter + loanDuration),
                    ReaderId = user.Id,
                    CollectionDate = DateTime.Now.AddDays(collectionAfter)
                });

                db.SaveChanges();

                await ScheduleEmail(user, true, db.Loans.ToList().LastOrDefault().LoanId);
                await ScheduleEmail(user, false, db.Loans.ToList().LastOrDefault().LoanId);
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
        public ActionResult Edit([Bind(Include = "LoanId,BookId,ReaderId,LoanedDate,CollectionDate,LoanExpireDate,ReturnedDate")] Loan loan)
        {
            if (ModelState.IsValid)
            {
                db.Entry(loan).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("All", "ManagePanel");
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
