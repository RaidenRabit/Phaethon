using Core.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using InternalApi.DataAccess;
using InternalApi.DataManagement.IDataManagement;

namespace InternalApi.DataManagement
{
    internal class InvoiceManagement: IInvoiceManagement
    {
        private readonly InvoiceDa _invoiceDa;

        internal InvoiceManagement()
        {
            _invoiceDa = new InvoiceDa();
        }

        public bool CreateOrUpdate(Invoice invoice)
        {
            CompanyDa companyDa = new CompanyDa();
            RepresentativeDa representativeDa = new RepresentativeDa();
            ProductDa productDa = new ProductDa();
            ItemDa itemDa = new ItemDa();
            ElementDa elementDa = new ElementDa();

            using (var db = new DatabaseContext())
            {
                using (var dbTransaction = db.Database.BeginTransaction())
                {
                    try
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
                        Invoice dbInvoice = _invoiceDa.GetInvoice(db, invoice.ID);
                        if (dbInvoice != null)
                        {
                            added = added - dbInvoice.Transport;
                        }
                        if (total != 0)
                        {
                            added = added / total;
                        }

                        _invoiceDa.CreateOrUpdate(db, invoice);
                        
                        foreach (Element element in elements)
                        {
                            if (element.Invoice_ID != -1)
                            {
                                element.Item.Product.ProductGroup_ID = element.Item.Product.ProductGroup.ID;
                                element.Item.Product.ProductGroup = null;
                                productDa.CreateOrUpdate(db, element.Item.Product);
                                element.Item.Product_ID = element.Item.Product.ID;
                                element.Item.Product = null;
                                element.Item.IncomingTaxGroup_ID = element.Item.IncomingTaxGroup.ID;
                                element.Item.IncomingTaxGroup = null;
                                element.Item.IncomingPrice = element.Item.IncomingPrice * added + element.Item.IncomingPrice;
                                itemDa.CreateOrUpdate(db, element.Item);
                                element.Invoice_ID = invoice.ID;
                                element.Invoice = null;
                                element.Item_ID = element.Item.ID;
                                element.Item = null;
                                elementDa.CreateOrUpdate(db, element);
                            }
                            else
                            {
                                if (element.Item.ID != 0)
                                {
                                    itemDa.Delete(db, element.Item.ID);
                                }
                            }
                        }

                        dbTransaction.Commit();
                        return true;
                    }
                    catch
                    {
                        dbTransaction.Rollback();
                        return false;
                    }
                }
            }
        }

        public Invoice GetInvoice(int id)
        {
            using (var db = new DatabaseContext())
            {
                return _invoiceDa.GetInvoice(db, id);
            }
        }

        public List<Invoice> GetInvoices(int numOfRecords, int selectedCompany, string name, int selectedDate, DateTime from, DateTime to, string docNumber)
        {
            using (var db = new DatabaseContext())
            {
                return _invoiceDa.GetInvoices(db, numOfRecords, selectedCompany, name, selectedDate, from, to, docNumber);
            }
        }

        public bool Delete(int id)
        {
            using (var db = new DatabaseContext())
            {
                Invoice invoice = _invoiceDa.GetInvoice(db, id);
                if (invoice == null)
                {
                    return false;
                }
                return _invoiceDa.Delete(db, invoice);
            }
        }
    }
}