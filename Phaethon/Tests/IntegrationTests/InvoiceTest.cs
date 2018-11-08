﻿using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using Core.Model;
using Newtonsoft.Json;
using NUnit.Framework;
using NUnit.Framework.Internal;

namespace Tests.IntegrationTests
{
    public class InvoiceTest
    {
        private static Invoice GetInvoiceSeed()
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
                PaymentDate = DateTime.Now,
                PrescriptionDate = DateTime.Now,
                ReceptionDate = DateTime.Now,
                Receiver = receiver,
                Sender = sender,
                Transport = 5
            };

            return invoice;
        }
        
        #region Create
        [Test]
        public void Create_NewInvoiceObject_ObjectCreated()
        {
            //Setup
            HttpClient client = new HttpClient();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            
            //Act
            string json = client.GetAsync("http://localhost:64007/Invoice/GetInvoices?numOfRecords=10").Result.Content.ReadAsStringAsync().Result;
            int beforeInvoiceCount = JsonConvert.DeserializeObject<List<Invoice>>(json).Count;
            
            var result = client.PostAsJsonAsync("http://localhost:64007/Invoice/Create", GetInvoiceSeed()).Result;
            
            json = client.GetAsync("http://localhost:64007/Invoice/GetInvoices?numOfRecords=10").Result.Content.ReadAsStringAsync().Result;
            int afterInvoiceCount = JsonConvert.DeserializeObject<List<Invoice>>(json).Count;

            //Assert
            Assert.AreEqual(HttpStatusCode.OK, result.StatusCode);//check if executed
            Assert.AreEqual(beforeInvoiceCount + 1, afterInvoiceCount);//checks if invoice was added
        }

        [Test]
        public void Create_InvoiceObjectNull_NothingAddedInternalServerErrorReturned()
        {
            //Setup
            HttpClient client = new HttpClient();

            //Act
            string json = client.GetAsync("http://localhost:64007/Invoice/GetInvoices?numOfRecords=10").Result.Content.ReadAsStringAsync().Result;
            int beforeInvoiceCount = JsonConvert.DeserializeObject<List<Invoice>>(json).Count;
            
            var result = client.PostAsJsonAsync("http://localhost:64007/Invoice/Create", new Invoice()).Result;

            json = client.GetAsync("http://localhost:64007/Invoice/GetInvoices?numOfRecords=10").Result.Content.ReadAsStringAsync().Result;
            int afterInvoiceCount = JsonConvert.DeserializeObject<List<Invoice>>(json).Count;

            //Assert
            Assert.AreEqual(HttpStatusCode.InternalServerError, result.StatusCode);//check if executed
            Assert.AreEqual(beforeInvoiceCount, afterInvoiceCount);//checks if invoice was added
        }
        #endregion

        #region Read
        [Test]
        public void Read_CorrectID_SameObjectReturned()
        {
            //Setup
            HttpClient client = new HttpClient();
            string json = client.GetAsync("http://localhost:64007/Invoice/GetInvoices?numOfRecords=10").Result.Content.ReadAsStringAsync().Result;
            List<Invoice> invoices = JsonConvert.DeserializeObject<List<Invoice>>(json);
            if (invoices.Count < 1)//if no invoices create new
            {
                Create_NewInvoiceObject_ObjectCreated();

                json = client.GetAsync("http://localhost:64007/Invoice/GetInvoices?numOfRecords=10").Result.Content.ReadAsStringAsync().Result;
                invoices = JsonConvert.DeserializeObject<List<Invoice>>(json);
            }

            //Act
            var result = client.GetAsync("http://localhost:64007/Invoice/Read?id=" + invoices[0].ID).Result;
            json = result.Content.ReadAsStringAsync().Result;
            Invoice invoice = JsonConvert.DeserializeObject<Invoice>(json);

            //Assert
            Assert.AreEqual(HttpStatusCode.OK, result.StatusCode);//check if internal server error
            Assert.AreEqual(true, invoice.ID == invoices[0].ID &&
                                  invoice.DocNumber.Equals(invoices[0].DocNumber) &&
                                  invoice.PaymentDate.Equals(invoices[0].PaymentDate) &&
                                  invoice.ReceptionDate.Equals(invoices[0].ReceptionDate) &&
                                  invoice.PrescriptionDate.Equals(invoices[0].PrescriptionDate) &&
                                  invoice.Transport == invoices[0].Transport);//check if object received is the same
        }

        [Test]
        public void Read_WrongId_NullReturned()
        {
            //Setup
            HttpClient client = new HttpClient();

            //Act
            var result = client.GetAsync("http://localhost:64007/Invoice/Read?id=" + 0).Result;
            string json = result.Content.ReadAsStringAsync().Result;
            Invoice invoice = JsonConvert.DeserializeObject<Invoice>(json);

            //Assert
            Assert.AreEqual(HttpStatusCode.OK, result.StatusCode);//check if internal server error
            Assert.AreEqual(null, invoice);//check if object received is the sames
        }
        #endregion

        #region GetInvoices
        [Test]
        public void GetInvoices_MethodCalled_InvoicesReturned()
        {
            //Setup
            HttpClient client = new HttpClient();

            //Act
            var result = client.GetAsync("http://localhost:64007/Invoice/GetInvoices?numOfRecords=5").Result;
            string json = result.Content.ReadAsStringAsync().Result;
            List<Invoice> invoices = JsonConvert.DeserializeObject<List<Invoice>>(json);

            //Assert
            Assert.AreEqual(HttpStatusCode.OK, result.StatusCode);//check if internal server error
            Assert.AreNotEqual(null, invoices);//Check if result returned
        }
        #endregion

        #region Delete
        [Test]
        public void Delete_CorrectID_InvoiceDeleted()
        {
            //Setup
            HttpClient client = new HttpClient();
            string json = client.GetAsync("http://localhost:64007/Invoice/GetInvoices?numOfRecords=10").Result.Content.ReadAsStringAsync().Result;
            List<Invoice> invoices = JsonConvert.DeserializeObject<List<Invoice>>(json);
            if (invoices.Count < 1)//if no invoices create new
            {
                Create_NewInvoiceObject_ObjectCreated();

                json = client.GetAsync("http://localhost:64007/Invoice/GetInvoices?numOfRecords=10").Result.Content.ReadAsStringAsync().Result;
                invoices = JsonConvert.DeserializeObject<List<Invoice>>(json);
            }

            //Act
            var result = client.PostAsJsonAsync("http://localhost:64007/Invoice/Delete", invoices[0].ID).Result;

            json = client.GetAsync("http://localhost:64007/Invoice/GetInvoices?numOfRecords=10").Result.Content.ReadAsStringAsync().Result;
            int invoiceCount = JsonConvert.DeserializeObject<List<Invoice>>(json).Count;

            //Assert
            Assert.AreEqual(HttpStatusCode.OK, result.StatusCode);//check if internal server error
            Assert.AreEqual(invoices.Count - 1, invoiceCount);//check object removed
        }

        [Test]
        public void Delete_WrongId_InvoiceNotDeletedInternalServerErrorReturned()
        {
            //Setup
            HttpClient client = new HttpClient();
            string json = client.GetAsync("http://localhost:64007/Invoice/GetInvoices?numOfRecords=10").Result.Content.ReadAsStringAsync().Result;
            List<Invoice> invoices = JsonConvert.DeserializeObject<List<Invoice>>(json);

            //Act
            var result = client.PostAsJsonAsync("http://localhost:64007/Invoice/Delete", 0).Result;

            json = client.GetAsync("http://localhost:64007/Invoice/GetInvoices?numOfRecords=10").Result.Content.ReadAsStringAsync().Result;
            int invoiceCount = JsonConvert.DeserializeObject<List<Invoice>>(json).Count;

            //Assert
            Assert.AreEqual(HttpStatusCode.InternalServerError, result.StatusCode);//check if internal server error
            Assert.AreEqual(invoices.Count, invoiceCount);//check object removeds
        }
        #endregion
    }
}
