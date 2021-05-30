using LibrARRRy.DAL;
using LibrARRRy.Models;
using Quartz;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace LibrARRRy
{
    public class ApplyPenaltyJob : IJob
    {
        private LibrARRRyContext db = new LibrARRRyContext();

        public async Task Execute(IJobExecutionContext context)
        {
            List<Loan> loans = db.Loans.ToList();

            foreach(Loan l in loans)
            {
                // Check if book was returned and if its past due returning date
                if (l.ReturnedDate == null && l.LoanExpireDate.CompareTo(DateTime.Now) < 0)
                {
                    ApplicationUser user = db.Users.Where(u => u.Id == l.ReaderId).FirstOrDefault();

                    // Update penalty
                    if (user != null)
                    {
                        user.CashPenalty = true;
                        db.SaveChanges();
                    }
                }
            }
        }
    }
}