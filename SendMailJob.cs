using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using LibrARRRy.Controllers;
using LibrARRRy.DAL;
using LibrARRRy.Models;
using Microsoft.AspNet.Identity.EntityFramework;
using Quartz;

namespace LibrARRRy
{
    public class SendMailJob : IJob
    {
        public async Task Execute(IJobExecutionContext context)
        {
            // Get userId and body
            JobDataMap map = context.JobDetail.JobDataMap;
            string userId = map.GetString("userId"),
                subject = "Reminder from LibrARRRy!",
                body = map.GetString("body");
            
            // Check if there is a need to send email
            bool okay = int.TryParse(map.GetString("loanId"), out int loanId);
            if (okay)
            {
                string type = map.GetString("type");
                LibrARRRyContext db = new LibrARRRyContext();
                IdentityManager im = new IdentityManager();

                if (type == "r")
                {
                    Loan loan = db.Loans.ToList().Where(l => l.LoanId == loanId).FirstOrDefault();
                    if (loan.ReturnedDate != null)
                    {
                        return;
                    }
                }

                EmailSender.SendMail(body, subject, false, im.GetUserByID(userId).Email);
                Debug.WriteLine("sent");
            }
        }
    }
}