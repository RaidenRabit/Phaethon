using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Core.Model;
using Core.Model.Filters;
using InternalApi.DataManagement;
using InternalApi.DataManagement.IDataManagement;
using Newtonsoft.Json;
using NUnit.Framework;

namespace Tests.UnitTest
{
    public class EmailTest
    {
        private IEmailSenderDM _emailSenderDM;

        public EmailTest()
        {
            _emailSenderDM = new EmailSenderDM();
        }

        [Test]
        public void SendEmail_CorrectEmail_NoException()
        {
            //Setup
            string email = "bubriks@gmail.com";
            string name = "Ralfs";
            string job = "Fix phone screen";
            string description = "Screen has been cracked needs replacement";
            bool success = true;

            //Act
            try
            {
                _emailSenderDM.SendEmail(email, name, job, description);
            }
            catch
            {
                success = false;
            }

            //Assert
            Assert.IsTrue(success, "Message was sent successfully");
        }
    }
}
