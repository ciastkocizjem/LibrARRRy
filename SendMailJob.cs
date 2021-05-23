using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using LibrARRRy.Controllers;
using LibrARRRy.DAL;
using LibrARRRy.Models;
using Quartz;

namespace LibrARRRy
{
    public class SendMailJob : IJob
    {
        public async Task Execute(IJobExecutionContext context)
        {
            var accountController = DependencyResolver.Current.GetService<AccountController>();

            // Get userId and body
            JobDataMap map = context.JobDetail.JobDataMap;
            string userId = map.GetString("userId"),
                subject = "Reminder from LibrARRRy!",
                body = map.GetString("body");

            // Check if there is a need to send email
            int loanId = int.Parse(map.GetString("loanId"));
            string type = map.GetString("type");
            LibrARRRyContext db = new LibrARRRyContext();

            if (type == "d")
            {
                Loan loan = db.Loans.ToList().Where(l => l.LoanId == loanId).FirstOrDefault();
                if (loan.ReturnedDate != null)
                {
                    return;
                }
            }

            await accountController.UserManager.SendEmailAsync(userId, subject, body);
        }
    }
}