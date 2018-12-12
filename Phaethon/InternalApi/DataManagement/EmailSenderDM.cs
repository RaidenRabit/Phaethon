using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Web;
using InternalApi.DataManagement.IDataManagement;

namespace InternalApi.DataManagement
{
    internal class EmailSenderDM : IEmailSenderDM
    {
        public void SendEmail(string customerEmail, string customerName, string jobName, string jobDescription)
        {
            var fromAddress = new MailAddress("meansnoreply@gmail.com", "MeansNoReply");
            var toAddress = new MailAddress(customerEmail, customerName);
            const string fromPassword = "means1234";
            const string workingHours = "M-F: 9:00-18:00\nWeekends: closed!";
            string subject = jobName + " is DONE!";
            string body = "Dear " + customerName + ",\n" + 
                "Your order at Means has been finished, you may come pick it up any time between: " + workingHours + 
                "\nYour order's details are:\n" + jobDescription +
                "\n\n\nWith kind regards,\nMeans S/A";

            var smtp = new SmtpClient
            {
                Host = "smtp.gmail.com",
                Port = 587,
                EnableSsl = true,
                DeliveryMethod = SmtpDeliveryMethod.Network,
                UseDefaultCredentials = false,
                Credentials = new NetworkCredential(fromAddress.Address, fromPassword)
            };
            using (var message = new MailMessage(fromAddress, toAddress)
            {
                Subject = subject,
                Body = body
            })
            {
                try
                {
                    smtp.Send(message);
                }
                catch (Exception e)
                {
                    throw (e);
                }
            }
        }
    }
    
}