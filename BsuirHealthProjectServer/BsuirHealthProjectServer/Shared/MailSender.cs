using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace BsuirHealthProjectServer.Shared
{
    public class MailSender
    {
        private const string EMAIL = "testtask42@gmail.com";
        private const string PASSWORD = "testtask";

        public bool Send(string emailReceiver, string subject, string messageBody)
        {
            var from = new MailAddress(EMAIL, "Health&Food");
            var to = new MailAddress(emailReceiver);

            try
            {
                var smtp = new SmtpClient
                {
                    Host = "smtp.gmail.com",
                    Port = 587,
                    EnableSsl = true,
                    DeliveryMethod = SmtpDeliveryMethod.Network,
                    UseDefaultCredentials = false,
                    Credentials = new NetworkCredential(from.Address, PASSWORD)
                };
                var msg = new MailMessage(from, to)
                {
                    Subject = subject,
                    Body = messageBody,
                };
                smtp.Send(msg);
                return true;
            }
            catch (Exception)
            {
                //logging
                return false;
            }
        }
    }
}

