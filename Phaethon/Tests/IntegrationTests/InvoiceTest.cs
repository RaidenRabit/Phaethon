using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Core.Model;
using NUnit.Framework;
using System.Linq;
using System.Web;
using InternalApi.DataAccess;
using Newtonsoft.Json;

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
                Elements = ElementTest.GetElementsSeed()
            };

            using (var db = new DatabaseContext())
            {
                InvoiceDa invoiceDa = new InvoiceDa();
                Invoice oldInvoice = invoiceDa.GetInvoices(db, 1000, 0, "", 0, new DateTime(2000, 1, 1), DateTime.Now, "").SingleOrDefault(x => x.DocNumber.Equals(invoice.DocNumber));
                if (oldInvoice != null)
                {
                    invoice = oldInvoice;
                }
            }

            return invoice;
        }

        internal static Invoice CreateInvoice(Invoice invoice)
        {
            CompanyDa companyDa = new CompanyDa();
            RepresentativeDa representativeDa = new RepresentativeDa();
            ProductDa productDa = new ProductDa();
            ItemDa itemDa = new ItemDa();
            ElementDa elementDa = new ElementDa();
            InvoiceDa invoiceDa = new InvoiceDa();

            using (var db = new DatabaseContext())
            {
                companyDa.CreateOrUpdate(db, invoice.Receiver.Company);
                invoice.Receiver.Company_ID = invoice.Receiver.Company.ID;
                invoice.Receiver.Company = null;
                companyDa.CreateOrUpdate(db, invoice.Sender.Company);
                invoice.Sender.Company_ID = invoice.Sender.Company.ID;
                invoice.Sender.Company = null;
                representativeDa.CreateOrUpdate(db, invoice.Receiver);
                invoice.Receiver_ID = invoice.Receiver.ID;
                invoice.Receiver = null;
                representativeDa.CreateOrUpdate(db, invoice.Sender);
                invoice.Sender_ID = invoice.Sender.ID;
                invoice.Sender = null;
                List<Element> elements = invoice.Elements == null ? new List<Element>() : invoice.Elements.ToList();
                invoice.Elements = null;

                decimal total = elements.Sum(item => item.Item.IncomingPrice);
                decimal added = invoice.Transport;
                Invoice dbInvoice = invoiceDa.GetInvoice(db, invoice.ID);
                if (dbInvoice != null)
                {
                    added = added - dbInvoice.Transport;
                }
                if (total != 0)
                {
                    added = added / total;
                }

                invoiceDa.CreateOrUpdate(db, invoice);

                foreach (Element element in elements)
                {
                    productDa.CreateOrUpdate(db, element.Item.Product);
                    element.Item.Product_ID = element.Item.Product.ID;
                    element.Item.Product = null;
                    element.Item.IncomingPrice = element.Item.IncomingPrice * added + element.Item.IncomingPrice;
                    itemDa.CreateOrUpdate(db, element.Item);
                    element.Item_ID = element.Item.ID;
                    element.Item = null;
                    element.Invoice_ID = invoice.ID;
                    element.Invoice = null;
                    elementDa.CreateOrUpdate(db, element);
                }

                return invoice;
            }
        }
        
        //update
        #region CreateOrUpdate
        [Test]
        public async Task CreateOrUpdate_NewInvoiceObject_IsSuccessStatusCodeAndResponseTrue()
        {
            //Setup
            Invoice invoice = GetInvoiceSeed();
            if (invoice.ID != 0)
            {
                using (var db = new DatabaseContext())
                {
                    InvoiceDa invoiceDa = new InvoiceDa();
                    db.Invoices.Attach(invoice);
                    invoiceDa.Delete(db, invoice);
                    invoice = GetInvoiceSeed();
                }
            }

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
            if (invoice.ID != 0)
            {
                using (var db = new DatabaseContext())
                {
                    InvoiceDa invoiceDa = new InvoiceDa();
                    db.Invoices.Attach(invoice);
                    invoiceDa.Delete(db, invoice);
                    invoice = GetInvoiceSeed();
                }
            }
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
            Invoice oldInvoice = GetInvoiceSeed();
            if (oldInvoice.ID == 0)
            {
                oldInvoice = CreateInvoice(oldInvoice);
            }
            var parameters = HttpUtility.ParseQueryString(string.Empty);
            parameters["id"] = oldInvoice.ID.ToString();

            //Act
            var response = await _client.GetAsync("Invoice/GetInvoice?" + parameters);
            Invoice invoice = JsonConvert.DeserializeObject<Invoice>(await response.Content.ReadAsStringAsync());

            //Assert
            Assert.IsTrue(response.IsSuccessStatusCode);
            Assert.AreEqual(true, invoice.ID == oldInvoice.ID &&
                                  invoice.DocNumber.Equals(oldInvoice.DocNumber) &&
                                  invoice.PaymentDate.Equals(oldInvoice.PaymentDate) &&
                                  invoice.ReceptionDate.Equals(oldInvoice.ReceptionDate) &&
                                  invoice.PrescriptionDate.Equals(oldInvoice.PrescriptionDate) &&
                                  invoice.Transport == oldInvoice.Transport);//check if object received is the same
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
            Invoice oldInvoice = GetInvoiceSeed();
            if (oldInvoice.ID == 0)
            {
                using (var db = new DatabaseContext())
                {
                    InvoiceDa invoiceDa = new InvoiceDa();
                    invoiceDa.CreateOrUpdate(db, oldInvoice);
                }
            }
            int id = oldInvoice.ID;
            string json = JsonConvert.SerializeObject(id);
            var content = new StringContent(json);

            //Act
            var response = await _client.PostAsync("Invoice/Delete", content);
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
