using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Net.Http;
using System.Threading.Tasks;
using Core.Model;
using NUnit.Framework;
using System.Linq;
using System.Net;
using System.Web;
using InternalApi.DataAccess;
using Newtonsoft.Json;
using NUnit.Framework.Constraints;

namespace Tests.IntegrationTests
{
    public class InvoiceTest: IntegrationTestBase
    {
        private bool AreInvoicesEqual(Invoice firstInvoice, Invoice secondInvoice)
        {
            return firstInvoice.ID == secondInvoice.ID &&
                   firstInvoice.DocNumber.Equals(secondInvoice.DocNumber) &&
                   firstInvoice.RegNumber.Equals(secondInvoice.RegNumber) &&
                   firstInvoice.Incoming == secondInvoice.Incoming &&
                   firstInvoice.ReceptionDate.Equals(secondInvoice.ReceptionDate) &&
                   firstInvoice.CheckoutDate.Equals(secondInvoice.CheckoutDate) &&
                   firstInvoice.Transport == secondInvoice.Transport &&

                   firstInvoice.Receiver.ID == secondInvoice.Receiver.ID &&
                   firstInvoice.Sender.ID == secondInvoice.Sender.ID;
        }

        internal static Element GetElementSeed()
        {
            #region Tax group
            TaxGroup taxGroup = new TaxGroup
            {
                Name = "Test",
                Tax = 99
            };

            using (var db = new DatabaseContext())
            {
                TaxGroup oldTaxGroup = db.TaxGroups.SingleOrDefault(x => x.Name.Equals(taxGroup.Name));
                if (oldTaxGroup != null)
                {
                    taxGroup = oldTaxGroup;
                }
                else
                {
                    db.TaxGroups.Add(taxGroup);
                    db.SaveChanges();
                }
            }
            #endregion

            #region Product group
            ProductGroup productGroup = new ProductGroup
            {
                Margin = 99,
                Name = "Test"
            };

            using (var db = new DatabaseContext())
            {
                ProductGroup oldProductGroup = db.ProductGroups.SingleOrDefault(x => x.Name.Equals(productGroup.Name));
                if (oldProductGroup != null)
                {
                    productGroup = oldProductGroup;
                }
                else
                {
                    db.ProductGroups.Add(productGroup);
                    db.SaveChanges();
                }
            }
            #endregion

            #region Product
            Product product = new Product
            {
                Barcode = 0,
                Name = "Test",
                ProductGroup_ID = productGroup.ID
            };

            using (var db = new DatabaseContext())
            {
                Product oldProduct = db.Products.SingleOrDefault(x => x.Barcode == product.Barcode);
                if (oldProduct != null)
                {
                    product = oldProduct;
                }
                else
                {
                    db.Products.Add(product);
                    db.SaveChanges();
                }
            }

            product.ProductGroup = productGroup;
            #endregion

            #region Item
            Item item = new Item
            {
                Price = 100,
                Quantity = 1,
                SerNumber = "0",
                IncomingTaxGroup_ID = taxGroup.ID,
                Product_ID = product.ID
            };

            using (var db = new DatabaseContext())
            {
                Item oldItem = db.Items.SingleOrDefault(x => x.SerNumber.Equals(item.SerNumber));
                if (oldItem != null)
                {
                    item = oldItem;
                }
                else
                {
                    db.Items.Add(item);
                    db.SaveChanges();
                }
            }

            item.IncomingTaxGroup = taxGroup;
            item.Product = product;
            #endregion

            #region Address
            Address address = new Address
            {
                City = "TestCity",
                Street = "Nibevej",
                Number = "12A",
                Extra = "Room 22"
            };

            using (var db = new DatabaseContext())
            {
                Address oldAddress = db.Addresses.SingleOrDefault(x => x.City.Equals(address.City));
                if (oldAddress != null)
                {
                    address = oldAddress;
                }
                else
                {
                    db.Addresses.Add(address);
                    db.SaveChanges();
                }
            }
            #endregion

            #region Company
            Company company = new Company
            {
                ActualAddress_ID = address.ID,
                LegalAddress_ID = address.ID,
                Name = "Test",
                RegNumber = "0",
                BankName = "TestBank",
                BankNumber = "0"
            };

            using (var db = new DatabaseContext())
            {
                Company oldCompany = db.Companies.SingleOrDefault(x => x.Name.Equals(company.Name));
                if (oldCompany != null)
                {
                    company = oldCompany;
                }
                else
                {
                    db.Companies.Add(company);
                    db.SaveChanges();
                }
            }

            company.ActualAddress = address;
            company.LegalAddress = address;
            #endregion

            #region Representative
            Representative representative = new Representative
            {
                Name = "Test",
                Company_ID = company.ID
            };

            using (var db = new DatabaseContext())
            {
                Representative oldRepresentative = db.Representatives.SingleOrDefault(x => x.Name.Equals(company.Name));
                if (oldRepresentative != null)
                {
                    representative = oldRepresentative;
                }
                else
                {
                    db.Representatives.Add(representative);
                    db.SaveChanges();
                }
            }

            representative.Company = company;
            #endregion

            #region Invoice
            Invoice invoice = new Invoice
            {
                Incoming = true,
                DocNumber = "0",
                RegNumber = "0",
                CheckoutDate = new DateTime(2010, 1, 1),
                ReceptionDate = new DateTime(2010, 1, 1),
                Transport = 10,
                Sender_ID = representative.ID,
                Receiver_ID = representative.ID
            };

            using (var db = new DatabaseContext())
            {
                Invoice oldInvoice = db.Invoices.SingleOrDefault(x => x.DocNumber.Equals(invoice.DocNumber));
                if (oldInvoice != null)
                {
                    invoice = oldInvoice;
                }
                else
                {
                    db.Invoices.Add(invoice);
                    db.SaveChanges();
                }
            }

            invoice.Sender = representative;
            invoice.Receiver = representative;
            #endregion

            #region Element
            Element element = new Element
            {
                Item_ID = item.ID,
                Invoice_ID = invoice.ID
            };

            using (var db = new DatabaseContext())
            {
                Element oldElement = db.Elements.SingleOrDefault(x => x.Invoice_ID == invoice.ID && x.Item_ID == item.ID);
                if (oldElement != null)
                {
                    element = oldElement;
                }
                else
                {
                    db.Elements.Add(element);
                    db.SaveChanges();
                }
            }

            element.Invoice = invoice;
            element.Item = item;
            #endregion

            return element;
        }
        
        #region CreateOrUpdate
        [Test]
        public async Task CreateOrUpdate_NewInvoiceObject_IsSuccessStatusCodeAndResponseTrue()
        {
            //Setup
            Element element = GetElementSeed();
            Invoice invoice = element.Invoice;
            element.Invoice = null;
            invoice.Elements = new List<Element>() {element};
            using (var db = new DatabaseContext())
            {
                db.Invoices.Remove(db.Invoices.SingleOrDefault(x => x.ID == invoice.ID));
                db.SaveChanges();
            }

            //Act
            var response = await _internalClient.PostAsJsonAsync("Invoice/CreateOrUpdate", invoice);

            //Assert
            Assert.IsTrue(response.IsSuccessStatusCode, "Server responded with Success code");
        }

        [Test]
        public async Task CreateOrUpdate_UpdateInvoiceObject_IsSuccessStatusCodeAndResponseTrue()
        {
            //Setup
            Element element = GetElementSeed();
            Invoice invoice = element.Invoice;
            invoice.Transport = 99;

            //Act
            var response = await _internalClient.PostAsJsonAsync("Invoice/CreateOrUpdate", invoice);
            Invoice dbInvoice = null;
            using (var db = new DatabaseContext())
            {
                dbInvoice = db.Invoices
                    .Include(x => x.Sender)
                    .Include(x => x.Receiver)
                    .SingleOrDefault(x => x.ID == invoice.ID);
            }
            
            //Assert
            Assert.IsTrue(response.IsSuccessStatusCode, "Server responded with Success code");
            Assert.IsTrue(AreInvoicesEqual(invoice, dbInvoice), "Invoices are equal");//check if object received is the same
        }

        [Test]
        public async Task CreateOrUpdate_NewInvoiceObjectNoElements_IsSuccessStatusCodeAndResponseTrue()
        {
            //Setup
            Element element = GetElementSeed();
            Invoice invoice = element.Invoice;
            element.Invoice = null;
            using (var db = new DatabaseContext())
            {
                db.Invoices.Remove(db.Invoices.SingleOrDefault(x => x.ID == invoice.ID));
                db.SaveChanges();
            }

            //Act
            var response = await _internalClient.PostAsJsonAsync("Invoice/CreateOrUpdate", invoice);

            //Assert
            Assert.IsTrue(response.IsSuccessStatusCode, "Server responded with Success code");
        }

        [Test]
        public async Task CreateOrUpdate_UpdateInvoiceObjectNoElements_IsSuccessStatusCodeAndResponseTrue()
        {
            //Setup
            Element element = GetElementSeed();
            Invoice invoice = element.Invoice;
            invoice.Elements = null;

            //Act
            var response = await _internalClient.PostAsJsonAsync("Invoice/CreateOrUpdate", invoice);
            Invoice dbInvoice = null;
            using (var db = new DatabaseContext())
            {
                dbInvoice = db.Invoices
                    .Include(x => x.Sender)
                    .Include(x => x.Receiver)
                    .SingleOrDefault(x => x.ID == invoice.ID);
            }

            //Assert
            Assert.IsTrue(response.IsSuccessStatusCode, "Server responded with Success code");
            Assert.IsTrue(AreInvoicesEqual(invoice, dbInvoice), "Invoices are equal");//check if object received is the same
        }

        [Test]
        public async Task CreateOrUpdate_InvoiceObjectNull_IsSuccessStatusCodeAndResponseFalse()
        {
            //Setup
            Invoice invoice = null;

            //Act
            var response = await _internalClient.PostAsJsonAsync("Invoice/CreateOrUpdate", invoice);

            //Assert
            Assert.AreEqual(HttpStatusCode.BadRequest, response.StatusCode, "Server responded with bad request code");//check if internal server error
        }
        #endregion

        #region GetInvoice
        [Test]
        public async Task GetInvoice_CorrectID_IsSuccessStatusCodeAndSameObjectReturned()
        {
            //Setup
            Element element = GetElementSeed();
            Invoice oldInvoice = element.Invoice;
            var parameters = HttpUtility.ParseQueryString(string.Empty);
            parameters["id"] = oldInvoice.ID.ToString();

            //Act
            var response = await _internalClient.GetAsync("Invoice/GetInvoice?" + parameters);
            Invoice invoice = JsonConvert.DeserializeObject<Invoice>(await response.Content.ReadAsStringAsync());

            //Assert
            Assert.IsTrue(response.IsSuccessStatusCode, "Server responded with Success code");
            Assert.IsTrue(AreInvoicesEqual(oldInvoice, invoice), "Invoices are equal");//check if object received is the same
        }

        [Test]
        public async Task GetInvoice_WrongId_IsSuccessStatusCodeAndNullObjectReturned()
        {
            //Setup
            var parameters = HttpUtility.ParseQueryString(string.Empty);
            parameters["id"] = 0.ToString();

            //Act
            var response = await _internalClient.GetAsync("Invoice/GetInvoice?" + parameters);

            //Assert
            Assert.AreEqual(HttpStatusCode.BadRequest, response.StatusCode, "Server responded with bad request code");//check if internal server error
        }
        #endregion

        #region GetInvoices
        [Test]
        public async Task GetInvoices_MethodCalled_IsSuccessStatusCodeAndInvoicesReturned()
        {
            //Setup
            GetElementSeed();
            var parameters = HttpUtility.ParseQueryString(string.Empty);
            parameters["numOfRecords"] = 1.ToString();
            parameters["regNumber"] = "";
            parameters["docNumber"] = "";
            parameters["from"] = new DateTime(2000, 1, 1).ToString("dd/MM/yyyy");
            parameters["to"] = DateTime.Now.ToString("dd/MM/yyyy");
            parameters["company"] = "";
            parameters["sum"] = "0";

            //Act
            var response = await _internalClient.GetAsync("Invoice/GetInvoices?" + parameters);
            List<Invoice> invoices = JsonConvert.DeserializeObject<List<Invoice>>(await response.Content.ReadAsStringAsync());

            //Assert
            Assert.IsTrue(response.IsSuccessStatusCode, "Server responded with Success code");
            Assert.AreNotEqual(0, invoices.Count, "Invoices received");
        }
        #endregion

        #region Delete
        [Test]
        public async Task Delete_CorrectID_IsSuccessStatusCodeAndInvoiceDeleted()
        {
            //Setup
            Element element = GetElementSeed();
            Invoice invoice = element.Invoice;
            int id = invoice.ID;

            //Act
            var response = await _internalClient.PostAsJsonAsync("Invoice/Delete", id);

            //Assert
            Assert.IsTrue(response.IsSuccessStatusCode, "Server responded with Success code");
        }

        [Test]
        public async Task Delete_WrongID_IsSuccessStatusCodeAndInvoiceNotDeleted()
        {
            //Setup
            int id = 0;

            //Act
            var response = await _internalClient.PostAsJsonAsync("Invoice/Delete", id);

            //Assert
            Assert.AreEqual(HttpStatusCode.BadRequest, response.StatusCode, "Server responded with bad request code");//check if internal server error
        }
        #endregion
    }
}
