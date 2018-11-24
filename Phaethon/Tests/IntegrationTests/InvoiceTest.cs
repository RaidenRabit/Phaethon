using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Core.Model;
using NUnit.Framework;
using System.Linq;
using System.Web;
using Newtonsoft.Json;
using Tests.IntegrationTests;

namespace Tests.IntegrationTests
{
    public class InvoiceTest
    {
        //InternalTestFakeServerBase
        private static HttpClient _client;

        public InvoiceTest()
        {
            _client = new HttpClient();
            _client.BaseAddress = new Uri("http://localhost:64007/");
        }

        internal static Invoice GetInvoiceSeed()
        {
            Company receiverCompany = new Company
            {
                Location = "Nibevej 2344 location",
                Name = "shufle",
                RegNumber = "54555556",
                Address = "Nibevej 2344 Address",
                BankNumber = "DK34745624547"
            };

            Representative receiver = new Representative
            {
                Name = "Kabola",
                Company = receiverCompany
            };

            Company senderCompany = new Company
            {
                Location = "Wallstreet location",
                Name = "Pop-top-spain",
                RegNumber = "1233333",
                Address = "Wallstreet Address",
                BankNumber = "DK3477777777"
            };

            Representative sender = new Representative
            {
                Name = "Kasper",
                Company = senderCompany
            };

            Invoice invoice = new Invoice
            {
                DocNumber = "136381022",
                PaymentDate = new DateTime(2010, 1, 1),
                PrescriptionDate = new DateTime(2010, 1, 1),
                ReceptionDate = new DateTime(2010, 1, 1),
                Receiver = receiver,
                Sender = sender,
                Transport = 5,
                Elements = new ElementTest().GetElementsSeed().Result
            };

            return invoice;
        }
        
        //update
        //with items
        #region CreateOrUpdate
        [Test]
        public async Task CreateOrUpdate_NewInvoiceObject_IsSuccessStatusCodeAndResponseTrue()
        {
            //Setup
            Invoice invoice = GetInvoiceSeed();
            string json = JsonConvert.SerializeObject(invoice);
            var content = new StringContent(json);

            //Act
            var response = await _client.PostAsync("Invoice/CreateOrUpdate", content);
            var deserializedResponse = JsonConvert.DeserializeObject<bool>(await response.Content.ReadAsStringAsync());

            //Assert
            Assert.IsTrue(response.IsSuccessStatusCode);
            Assert.IsTrue(deserializedResponse);
        }

        [Test]
        public async Task CreateOrUpdate_NewInvoiceObjectNoElements_IsSuccessStatusCodeAndResponseTrue()
        {
            //Setup
            Invoice invoice = GetInvoiceSeed();
            invoice.Elements = null;
            string json = JsonConvert.SerializeObject(invoice);
            var content = new StringContent(json);

            //Act
            var response = await _client.PostAsync("Invoice/CreateOrUpdate", content);
            var deserializedResponse = JsonConvert.DeserializeObject<bool>(await response.Content.ReadAsStringAsync());

            //Assert
            Assert.IsTrue(response.IsSuccessStatusCode);
            Assert.IsTrue(deserializedResponse);
        }
        
        [Test]
        public async Task CreateOrUpdate_InvoiceObjectNull_IsSuccessStatusCodeAndResponseFalse()
        {
            //Setup
            Invoice invoice = null;
            string json = JsonConvert.SerializeObject(invoice);
            var content = new StringContent(json);

            //Act
            var response = await _client.PostAsync("Invoice/CreateOrUpdate", content);
            var deserializedResponse = JsonConvert.DeserializeObject<bool>(await response.Content.ReadAsStringAsync());

            //Assert
            Assert.IsTrue(response.IsSuccessStatusCode);
            Assert.IsFalse(deserializedResponse);
        }
        #endregion

        #region GetInvoice
        [Test]
        public async Task GetInvoice_CorrectID_IsSuccessStatusCodeAndSameObjectReturned()
        {
            //Setup
            var parameters = HttpUtility.ParseQueryString(string.Empty);
            parameters["numOfRecords"] = 1.ToString();
            parameters["selectedCompany"] = 0.ToString();
            parameters["name"] = "";
            parameters["selectedDate"] = 0.ToString();
            parameters["from"] = new DateTime(2000,1,11).ToString("dd/MM/yyyy");
            parameters["to"] = DateTime.Now.ToString("dd/MM/yyyy");
            parameters["docNumber"] = "";
            var response = await _client.GetAsync("Invoice/GetInvoices?" + parameters);
            List<Invoice> invoices = JsonConvert.DeserializeObject<List<Invoice>>(await response.Content.ReadAsStringAsync());
            parameters = HttpUtility.ParseQueryString(string.Empty);
            parameters["id"] = invoices[0].ID.ToString();

            //Act
            response = await _client.GetAsync("Invoice/GetInvoice?" + parameters);
            Invoice invoice = JsonConvert.DeserializeObject<Invoice>(await response.Content.ReadAsStringAsync());

            //Assert
            Assert.IsTrue(response.IsSuccessStatusCode);
            Assert.AreEqual(true, invoice.ID == invoices[0].ID &&
                                  invoice.DocNumber.Equals(invoices[0].DocNumber) &&
                                  invoice.PaymentDate.Equals(invoices[0].PaymentDate) &&
                                  invoice.ReceptionDate.Equals(invoices[0].ReceptionDate) &&
                                  invoice.PrescriptionDate.Equals(invoices[0].PrescriptionDate) &&
                                  invoice.Transport == invoices[0].Transport);//check if object received is the same
        }

        [Test]
        public async Task GetInvoice_WrongId_IsSuccessStatusCodeAndNullObjectReturned()
        {
            //Setup
            var parameters = HttpUtility.ParseQueryString(string.Empty);
            parameters["id"] = 0.ToString();

            //Act
            var response = await _client.GetAsync("Invoice/GetInvoice?" + parameters);
            Invoice invoice = JsonConvert.DeserializeObject<Invoice>(await response.Content.ReadAsStringAsync());

            //Assert
            Assert.IsTrue(response.IsSuccessStatusCode);
            Assert.AreEqual(null, invoice);
        }
        #endregion

        #region GetInvoices
        [Test]
        public async Task GetInvoices_MethodCalled_IsSuccessStatusCodeAndInvoicesReturned()
        {
            //Setup
            var parameters = HttpUtility.ParseQueryString(string.Empty);
            parameters["numOfRecords"] = 1.ToString();
            parameters["selectedCompany"] = 0.ToString();
            parameters["name"] = "";
            parameters["selectedDate"] = 0.ToString();
            parameters["from"] = new DateTime(2000, 1, 11).ToString("dd/MM/yyyy");
            parameters["to"] = DateTime.Now.ToString("dd/MM/yyyy");
            parameters["docNumber"] = "";

            //Act
            var response = await _client.GetAsync("Invoice/GetInvoices?" + parameters);
            List<Invoice> invoices = JsonConvert.DeserializeObject<List<Invoice>>(await response.Content.ReadAsStringAsync());

            //Assert
            Assert.IsTrue(response.IsSuccessStatusCode);
            Assert.AreNotEqual(null, invoices);
        }
        #endregion

        #region Delete
        [Test]
        public async Task Delete_CorrectID_IsSuccessStatusCodeAndInvoiceDeleted()
        {
            //Setup
            var parameters = HttpUtility.ParseQueryString(string.Empty);
            parameters["numOfRecords"] = 1.ToString();
            parameters["selectedCompany"] = 0.ToString();
            parameters["name"] = "";
            parameters["selectedDate"] = 0.ToString();
            parameters["from"] = new DateTime(2000, 1, 11).ToString("dd/MM/yyyy");
            parameters["to"] = DateTime.Now.ToString("dd/MM/yyyy");
            parameters["docNumber"] = "";
            var response = await _client.GetAsync("Invoice/GetInvoices?" + parameters);
            List<Invoice> invoices = JsonConvert.DeserializeObject<List<Invoice>>(await response.Content.ReadAsStringAsync());
            int id = invoices.Last().ID;
            string json = JsonConvert.SerializeObject(id);
            var content = new StringContent(json);

            //Act
            response = await _client.PostAsync("Invoice/Delete", content);
            var deserializedResponse = JsonConvert.DeserializeObject<bool>(await response.Content.ReadAsStringAsync());

            //Assert
            Assert.IsTrue(response.IsSuccessStatusCode);
            Assert.IsTrue(deserializedResponse);
        }

        [Test]
        public async Task Delete_WrongID_IsSuccessStatusCodeAndInvoiceNotDeleted()
        {
            //Setup
            int id = 0;
            string json = JsonConvert.SerializeObject(id);
            var content = new StringContent(json);

            //Act
            var response = await _client.PostAsync("Invoice/Delete", content);
            var deserializedResponse = JsonConvert.DeserializeObject<bool>(await response.Content.ReadAsStringAsync());

            //Assert
            Assert.IsTrue(response.IsSuccessStatusCode);
            Assert.IsFalse(deserializedResponse);
        }
        #endregion
    }
}
