using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Web;

namespace LibrARRRy
{
    public static class EmailSender
    {
        public static void SendMail(string body, string subject, bool isBodyHtml, string email)
        {
            SmtpClient client = new SmtpClient();
            client.Port = 587;
            client.Host = "smtp.gmail.com";
            client.EnableSsl = true;
            client.DeliveryMethod = SmtpDeliveryMethod.Network;
            client.UseDefaultCredentials = false;
            client.Credentials = new NetworkCredential("librarrryemails@gmail.com", "Library123.");

            MailMessage mailMessage = new MailMessage();
            mailMessage.Body = body;
            mailMessage.Subject = subject;
            mailMessage.IsBodyHtml = isBodyHtml;
            mailMessage.To.Add(email);

            client.Send(mailMessage);
        }
    }
}