using MailKit.Net.Smtp;
using MailKit.Security;
using MimeKit;
using MimeKit.Text;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DoAnChuyenNganh_SQLServer.Service
{
    public interface IEmailService
    {
        void Send(string from, string to, string subject, string html);
    }

    public class EmailService
    {
        public static bool Send(string to, string subject, string html)
        {
            try
            {
                // create message
                var email = new MimeMessage();
                email.From.Add(MailboxAddress.Parse("bdqhoang@gmail.com"));
                email.To.Add(MailboxAddress.Parse(to));
                email.Subject = subject;
                email.Body = new TextPart(TextFormat.Html) { Text = html };

                // send email
                using (var smtp = new SmtpClient())
                {
                    smtp.CheckCertificateRevocation = false;
                    smtp.Connect("smtp.gmail.com", 587, SecureSocketOptions.StartTls);
                    smtp.Authenticate("bdqhoang@gmail.com", "qdodjliiihktkyap");
                    smtp.Send(email);
                    smtp.Disconnect(true);
                }
                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}